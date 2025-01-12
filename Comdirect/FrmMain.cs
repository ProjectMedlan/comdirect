using Comdirect.API;
using Comdirect.ViewModelConverter;
using Comdirect.Shared;
using Comdirect.ViewModels;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Comdirect
{
    public partial class FrmMain : Form
    {
        private ComdirectAPI? _comdirectAPI;
        private UserSettings? _userSettings;

        private const Enumerations.LogTypes _logLevel = Enumerations.LogTypes.Debug | Enumerations.LogTypes.Info | Enumerations.LogTypes.Warning | Enumerations.LogTypes.Error;
        private const int AUTO_SESSION_REFRESH_LIMIT = 20;
        private const int BULK_REQUEST_RATE_LIMIT_DELAY = 250;

        private DocumentListViewModel? _lastDocumentList;
        private ReportViewModel? _lastReport;
        private Dictionary<string, List<AccountTransactionViewModel>> _accountTransactionCache = new Dictionary<string, List<AccountTransactionViewModel>>();

        // TODO: Add
        private Dictionary<string, List<DepotTransactionViewModel>> _depotTransactionCache = new Dictionary<string, List<DepotTransactionViewModel>>();
        private Dictionary<string, List<DepotPositionViewModel>> _depotPositionCache = new Dictionary<string, List<DepotPositionViewModel>>();
        private Dictionary<string, List<DepotDetailsViewModel>> _depotDetailsCache = new Dictionary<string, List<DepotDetailsViewModel>>();

        private int _currentSessionTimeout = 0;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        private const int WM_VSCROLL = 277;
        private const int SB_PAGEBOTTOM = 7;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            _userSettings = Program.Configuration.GetSection("UserSettings").Get<UserSettings>();
            txtClientID.Text = _userSettings?.ClientID;
            txtClientSecret.Text = _userSettings?.ClientSecret;
            txtUsername.Text = _userSettings?.Username;
            if (!string.IsNullOrEmpty(txtUsername.Text))
            {
                txtPassword.Focus();
            }
            else
            {
                txtUsername.Focus();
            }
        }

        #region Login
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if ((_comdirectAPI == null) && (await PerformFullLogin()))
            {
                btnLogin.Text = "Logout";
                await LoadReport();
                await RefreshPostBox(cbOnlyNew.Checked, true);
            }
            else
            {
                _comdirectAPI?.RevokeSession();
                _comdirectAPI = null;
                ResetUI();
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                btnLogin.PerformClick();
            }
        }

        private async Task<bool> PerformFullLogin()
        {
            if (_userSettings == null) return false;
            if (string.IsNullOrEmpty(txtClientID.Text)) return false;
            if (string.IsNullOrEmpty(txtClientSecret.Text)) return false;

            _comdirectAPI = new ComdirectAPI(txtUsername.Text, txtPassword.Text, txtClientID.Text, txtClientSecret.Text);
            _comdirectAPI.OnNewDebugLogMessage += RaiseNewDebugLogMessage;
            _comdirectAPI.OnNewInfoLogMessage += RaiseNewLogMessage;
            _comdirectAPI.OnSessionTimeoutChanged += OnSessionTimeoutChanged;

            bool getToken = await _comdirectAPI.GetAccessToken();
            RaiseNewLogMessage("Login: Schritt 1: " + (getToken ? "Erfolgreich" : "Fehlgeschlagen"));
            if (!getToken) return false;

            bool getSession = await _comdirectAPI.GetSessionStatus();
            RaiseNewLogMessage("Login: Schritt 2: " + (getSession ? "Erfolgreich" : "Fehlgeschlagen"));
            if (!getSession) return false;

            bool startTanChallenge = await _comdirectAPI.StartTanChallenge(Constants.TAN_TYPE_PUSH_TAN);
            RaiseNewLogMessage("Login: Schritt 3: " + (startTanChallenge ? "Erfolgreich" : "Fehlgeschlagen"));
            if (!startTanChallenge) return false;

            string? tan_type = _comdirectAPI.GetTanChallengeType();
            string? challenge = _comdirectAPI.GetTanChallenge();
            string tan = "";

            FrmTanConfirmation tanConfirmation = new FrmTanConfirmation();
            tanConfirmation.Init(tan_type, challenge);
            if (tanConfirmation.ShowDialog() != DialogResult.OK) return false;
            tan = tanConfirmation.GetTan();

            bool activated = await _comdirectAPI.ActivateSession(tan);
            RaiseNewLogMessage("Login: Schritt 4: " + (activated ? "Erfolgreich" : "Fehlgeschlagen"));
            if (!activated) return false;

            bool finalLogin = await _comdirectAPI.GetCDSecondaryFlowToken();
            RaiseNewLogMessage("Login: Schritt 5: " + (finalLogin ? "Erfolgreich" : "Fehlgeschlagen"));
            if (!finalLogin) return false;

            return true;
        }

        #endregion

        #region Load Report
        private async Task LoadReport()
        {
            if (_comdirectAPI == null) return;

            var reportData = await _comdirectAPI.GetReport();
            if (reportData != null)
            {
                _lastReport = reportData.ConvertToViewModel();
                RaiseNewDebugLogMessage($"Anzahl der Konten: {_lastReport.AccountsCount}");
                RaiseNewDebugLogMessage($"Gesamtwert: {_lastReport.TotalBalanceInEuro}");
                RaiseNewDebugLogMessage($"Verfügbar: {_lastReport.AvailableCashAmountInEuro}");

                lvwAccounts.Items.Clear();
                lvwAccounts.SuspendLayout();
                decimal totalBalance = 0;
                decimal totalCreditLimit = 0;
                foreach (var item in _lastReport.Accounts)
                {
                    lvwAccounts.Items.Add(ConvertToListItem(item));
                    totalBalance += item.BalanceInEuro;
                    totalCreditLimit += item.CreditLimitInEuro;
                }
                foreach (var item in _lastReport.Cards)
                {
                    lvwAccounts.Items.Add(ConvertToListItem(item));
                    totalBalance += item.CardBalanceInEuro;
                    totalCreditLimit += item.CardLimitInEuro;
                }
                foreach (var item in _lastReport.Depots)
                {
                    lvwAccounts.Items.Add(ConvertToListItem(item));
                    totalBalance += item.PreviousDayValue;
                }

                ListViewItem lvi = new ListViewItem();
                lvi.Text = "Gesamt";
                lvi.SubItems.Add(totalBalance.ToString("C"));
                lvi.SubItems.Add("Inkl. Kreditlimit: " + (totalBalance + totalCreditLimit).ToString("C"));
                lvwAccounts.Items.Add(lvi);
                lvwAccounts.ResumeLayout();
            }
        }

        private ListViewItem ConvertToListItem(ReportAccountViewModel item)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Tag = item;
            lvi.Text = item.AccountDescription;
            lvi.SubItems.Add(item.BalanceInEuro.ToString("C"));
            if (item.CreditLimitInEuro > 0)
                lvi.SubItems.Add("Dispo: " + item.CreditLimitInEuro.ToString("C"));
            return lvi;
        }

        private ListViewItem ConvertToListItem(ReportCardViewModel item)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Tag = item;
            lvi.Text = item.CardType;
            lvi.SubItems.Add(item.CardBalanceInEuro.ToString("C"));
            lvi.SubItems.Add("Status: " + item.CardStatus);
            return lvi;
        }

        private ListViewItem ConvertToListItem(ReportDepotViewModel item)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Tag = item;
            lvi.Text = "Wertpapierdepot";
            lvi.SubItems.Add(item.PreviousDayValue.ToString("C"));
            lvi.SubItems.Add("Stand: " + item.LastUpdate?.ToString("dd.MM.yyyy"));
            return lvi;
        }

        #endregion

        #region Account Transactions / Depot Transactions / Depot Positions
        private async void lvwAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwAccounts.SelectedIndices.Count != 1) return;
            var tag = lvwAccounts.Items[lvwAccounts.SelectedIndices[0]].Tag;
            if (tag == null) return;

            if (tag is ReportAccountViewModel accountTag)
            {
                if (accountTag.AccountId == null) return;

                await RefreshAccountTransactions(accountTag.AccountId, false);
            }
            else if (tag is ReportDepotViewModel depotTag)
            {
                if (depotTag.DepotId == null) return;
                await RefreshDepotTransactions(depotTag.DepotId, false);
                await RefreshDepotPositions(depotTag.DepotId, false);
            }
        }

        private async Task RefreshAccountTransactions(string accountId, bool forceReload)
        {
            if (_comdirectAPI == null) return;
            if (string.IsNullOrEmpty(accountId)) return;

            // Get Data from API or Cache
            if (!_accountTransactionCache.ContainsKey(accountId) || forceReload)
            {
                var transactionsList = await _comdirectAPI.GetAccountTransactions(accountId);
                if (transactionsList == null) return;

                // Remove old values from cache
                if (_accountTransactionCache.ContainsKey(accountId))
                    _accountTransactionCache.Remove(accountId);

                // Add new list to cache
                _accountTransactionCache.TryAdd(accountId, new List<AccountTransactionViewModel>());

                // Convert to ViewModel
                foreach (var transactionResponse in transactionsList.values)
                {
                    _accountTransactionCache[accountId].Add(transactionResponse.ConvertToViewModel());
                }

                RaiseNewLogMessage($"{transactionsList.values.Length} Kontotransaktionen geladen");
            }

            if (_accountTransactionCache[accountId].Count == 0) return;

            // Show in ListView
            lvwAccountTransactions.Items.Clear();
            lvwAccountTransactions.SuspendLayout();
            foreach (var currentTransaction in _accountTransactionCache[accountId])
            {
                ListViewItem lvi = new ListViewItem();
                lvi.UseItemStyleForSubItems = false;
                lvi.Tag = currentTransaction;
                lvi.Text = currentTransaction.BookingDate.HasValue ? currentTransaction.BookingDate.Value.ToString("dd.MM.yyyy") : "Offen";
                lvi.SubItems.Add(currentTransaction.Remitter ?? currentTransaction.Creditor ?? currentTransaction.CategoryDisplayName ?? "");
                lvi.SubItems.Add(currentTransaction.TransactionValue.ToString("C")).ForeColor = currentTransaction.TransactionValue > 0 ? Color.Green : Color.Red;
                lvi.SubItems.Add(currentTransaction.TransactionTypeDisplayName ?? "");
                lvi.ToolTipText = string.Join(Environment.NewLine, currentTransaction.RemittanceInfo);
                lvwAccountTransactions.Items.Add(lvi);
            }
            lvwAccountTransactions.ResumeLayout();
            tabCtrl.SelectedTab = tabPageAccountTransactions;

            string tabLabel = _lastReport.Accounts.FirstOrDefault(x => x.AccountId == accountId)?.AccountDescription;
            tabPageAccountTransactions.Text = tabLabel ?? "Konto-Transaktionen";
        }

        private async Task RefreshDepotTransactions(string depotId, bool forceReload)
        {
            if (_comdirectAPI == null) return;
            if (string.IsNullOrEmpty(depotId)) return;

            // Get Data from API or Cache
            if (!_depotTransactionCache.ContainsKey(depotId) || forceReload)
            {
                var transactionsList = await _comdirectAPI.GetDepotTransactions(depotId);
                if (transactionsList == null) return;

                // Remove old values from cache
                if (_depotTransactionCache.ContainsKey(depotId))
                    _depotTransactionCache.Remove(depotId);

                // Add new list to cache
                _depotTransactionCache.TryAdd(depotId, new List<DepotTransactionViewModel>());

                // Convert to ViewModel
                foreach (var transactionResponse in transactionsList.values)
                {
                    _depotTransactionCache[depotId].Add(transactionResponse.ConvertToViewModel());
                }

                RaiseNewLogMessage($"{transactionsList.values.Length} Depottransaktionen geladen");
            }

            if (_depotTransactionCache[depotId].Count == 0) return;

            // Show in ListView
            lvwDepotTransactions.Items.Clear();
            lvwDepotTransactions.SuspendLayout();
            foreach (var currentTransaction in _depotTransactionCache[depotId])
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Tag = currentTransaction;
                lvi.Text = currentTransaction.BookingDate.HasValue ? currentTransaction.BookingDate.Value.ToString("dd.MM.yyyy") : "Offen";
                lvi.SubItems.Add(currentTransaction.Instrument?.ShortName ?? "");
                lvi.SubItems.Add(currentTransaction.Quantity.ToString());
                lvi.SubItems.Add(currentTransaction.TransactionValue.ToString("C")).ForeColor = currentTransaction.TransactionValue > 0 ? Color.Green : Color.Red;
                lvi.SubItems.Add(!string.IsNullOrEmpty(currentTransaction.TransactionType) ? Constants.DEPOT_TRANSACTION_TYPES[currentTransaction.TransactionType] : string.Empty);
                lvwDepotTransactions.Items.Add(lvi);
            }
            lvwDepotTransactions.ResumeLayout();
            tabCtrl.SelectedTab = tabPageDepotTransactions;
        }

        private async Task RefreshDepotPositions(string depotId, bool forceReload)
        {
            if (_comdirectAPI == null) return;
            if (string.IsNullOrEmpty(depotId)) return;

            // Get Data from API or Cache
            if (!_depotPositionCache.ContainsKey(depotId) || forceReload)
            {
                var positionsList = await _comdirectAPI.GetDepotPositions(depotId);
                if (positionsList == null) return;

                // Remove old values from cache
                if (_depotPositionCache.ContainsKey(depotId))
                    _depotPositionCache.Remove(depotId);

                if (_depotDetailsCache.ContainsKey(depotId))
                    _depotDetailsCache.Remove(depotId);

                // Add new list to cache
                _depotPositionCache.TryAdd(depotId, new List<DepotPositionViewModel>());
                _depotDetailsCache.TryAdd(depotId, new List<DepotDetailsViewModel>());

                // Convert to ViewModel
                _depotDetailsCache[depotId].Add(positionsList.aggregated.ConvertToViewModel());
                foreach (var positionResponse in positionsList.values)
                {
                    _depotPositionCache[depotId].Add(positionResponse.ConvertToViewModel());
                }

                RaiseNewLogMessage($"{positionsList.values.Length} Depotpositionen geladen");
            }

            if (_depotPositionCache[depotId].Count == 0) return;

            // Set the Depot ToolTip on the Account Listview Depts Entry
            ListViewItem depotItem = lvwAccounts.Items.Cast<ListViewItem>().FirstOrDefault(x => x.Tag is ReportDepotViewModel && ((ReportDepotViewModel)x.Tag!).DepotId == depotId);
            DepotDetailsViewModel depotDetails = _depotDetailsCache[depotId].FirstOrDefault();
            if ((depotItem != null) && (depotDetails != null))
            {
                StringBuilder depotToolTip = new StringBuilder();
                depotToolTip.AppendLine($"Aktueller Wert: {depotDetails.CurrentValue.ToString("C")}");
                depotToolTip.AppendLine($"Vortag: {depotDetails.PreviousDayValue.ToString("C")}");
                depotToolTip.AppendLine($"Kaufwert: {depotDetails.PurchaseValue.ToString("C")}");
                depotToolTip.AppendLine($"Veränderung: {depotDetails.TotalProfitOrLoss.ToString("C")}");
                depotItem.ToolTipText = depotToolTip.ToString();
            }


            // Show in ListView
            lvwDepotPositions.Items.Clear();
            lvwDepotPositions.SuspendLayout();
            List<DepotPositionViewModel> sortedPositions = _depotPositionCache[depotId].OrderBy(x => x.Instrument?.Name).ToList();
            foreach (var currentPosition in sortedPositions)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.UseItemStyleForSubItems = false;
                lvi.Tag = currentPosition;
                lvi.Text = currentPosition.Instrument?.Name;
                lvi.SubItems.Add(currentPosition.Quantity.ToString());
                lvi.SubItems.Add(currentPosition.CurrentValue.ToString("C"));
                lvi.SubItems.Add(currentPosition.TotalProfitOrLoss.ToString("C")).ForeColor = currentPosition.TotalProfitOrLoss > 0 ? Color.Green : Color.Red;
                lvwDepotPositions.Items.Add(lvi);
            }
            lvwDepotPositions.ResumeLayout();
        }

        #endregion

        #region Postbox

        private async Task RefreshPostBox(bool onlyNewEntries, bool forceReload)
        {
            if (_comdirectAPI == null) return;

            // Reload data from API or use cached data
            if (forceReload || _lastDocumentList == null)
            {
                var postboxData = await _comdirectAPI.GetDocuments();
                if (postboxData != null)
                {
                    var documentListViewModel = postboxData.ConvertToViewModel();
                    _lastDocumentList = documentListViewModel;
                    RaiseNewLogMessage(Enumerations.LogTypes.Info, $"Sie haben {_lastDocumentList.TotalUnreadDocuments} ungelesene Nachrichten in der Postbox");
                }
            }

            if (_lastDocumentList != null)
            {
                lvwPostBox.Items.Clear();
                lvwPostBox.SuspendLayout();
                foreach (var item in _lastDocumentList.Documents)
                {
                    if (onlyNewEntries && item.IsRead) continue;

                    ListViewItem lvi = new ListViewItem();
                    lvi.Tag = item;
                    lvi.Text = string.Empty; // No Content - just checkboxes
                    lvi.SubItems.Add(item.CreationDate?.ToString("dd.MM.yyyy"));
                    lvi.SubItems.Add(item.Name);

                    if (!item.IsRead)
                    {
                        lvi.Font = new Font(lvi.Font, FontStyle.Bold);
                    }

                    lvwPostBox.Items.Add(lvi);
                }
                lvwPostBox.ResumeLayout();
            }
        }

        private async void btnPostboxRefresh_Click(object sender, EventArgs e)
        {
            await RefreshPostBox(cbOnlyNew.Checked, true);
        }

        private async void btnDownloadDocuments_Click(object sender, EventArgs e)
        {
            btnDownloadDocuments.Enabled = false;
            lvwPostBox.Enabled = false;

            if (_comdirectAPI == null) return;

            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.InitialDirectory = (_userSettings != null && !string.IsNullOrEmpty(_userSettings.DefaultDownloadDirectory)) ? _userSettings.DefaultDownloadDirectory : "";
            folderDialog.ShowNewFolderButton = true;
            if (folderDialog.ShowDialog() != DialogResult.OK) return;
            var selectedDirectory = folderDialog.SelectedPath;

            int progress = 0;
            int count = lvwPostBox.CheckedItems.Count;
            foreach (ListViewItem item in lvwPostBox.CheckedItems)
            {
                SetStatusText($"Lade Dokument {++progress} / {count}");

                DocumentViewModel doc = (DocumentViewModel)item.Tag!;
                if (doc == null) continue;
                if (string.IsNullOrEmpty(doc.MimeType)) continue;

                // Download document
                var documentContent = await _comdirectAPI.GetDocument(doc.DocumentId, doc.MimeType);
                if (documentContent == null) continue;

                // Base-Filename
                string fileExtension = doc.MimeType.Contains("pdf") ? ".pdf" : ".html";
                string filename = doc.CreationDate?.ToString("yyyy-MM-dd") + " - " + doc.Name + fileExtension;

                // Replace illegale filename characters
                string regSearch = new string(Path.GetInvalidFileNameChars());
                Regex rg = new Regex(string.Format("[{0}]", Regex.Escape(regSearch)));
                filename = rg.Replace(filename, "");

                // Set selected directory & write file
                filename = Path.Combine(selectedDirectory, filename);
                File.WriteAllBytes(filename, documentContent);

                // Set document as read
                doc.IsRead = true;
                doc.ReadDate = DateOnly.FromDateTime(DateTime.Now);

                item.Checked = false;
                await Task.Delay(BULK_REQUEST_RATE_LIMIT_DELAY); // Manual throttling
            }

            SetStatusText("");
            await RefreshPostBox(cbOnlyNew.Checked, false); // Force view to update in case I set some of the documents to read
            btnDownloadDocuments.Enabled = true;
            lvwPostBox.Enabled = true;
        }

        private async void cbOnlyNew_CheckedChanged(object sender, EventArgs e)
        {
            await RefreshPostBox(cbOnlyNew.Checked, false);
        }

        private void lvwPostBox_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            btnDownloadDocuments.Enabled = lvwPostBox.CheckedItems.Count > 0;
        }

        private void lvwPostBox_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != 0) return;
            if (lvwPostBox.Items.Count == 0) return;

            bool setStateTo = !lvwPostBox.Items[0].Checked;
            foreach (ListViewItem item in lvwPostBox.Items)
            {
                item.Checked = setStateTo;
            }
        }

        #endregion

        #region Logging & Status
        private void RaiseNewLogMessage(string message)
        {
            RaiseNewLogMessage(Enumerations.LogTypes.Info, message);
        }

        private void RaiseNewDebugLogMessage(string message)
        {
            RaiseNewLogMessage(Enumerations.LogTypes.Debug, message);
        }

        private void RaiseNewLogMessage(Enumerations.LogTypes logType, string message)
        {
            if (!_logLevel.HasFlag(logType)) return;

            if (rtbLog.InvokeRequired)
            {
                this.Invoke(new Action<Enumerations.LogTypes, string>(RaiseNewLogMessage), new { logType, message });
                return;
            }

            rtbLog.Text += message + Environment.NewLine;
            SendMessage(rtbLog.Handle, WM_VSCROLL, (IntPtr)SB_PAGEBOTTOM, IntPtr.Zero);
        }

        private void SetStatusText(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(SetStatusText), message);
                return;
            }
            tsslInfo.Text = message;
        }

        #endregion

        #region Session Handling

        private void ResetUI()
        {
            _comdirectAPI = null;
            btnLogin.Text = "Login";
            btnSessionTimeout.Enabled = false;
            lblSessionTimeout.Text = "";
            timSession.Stop();
            lvwAccounts.Items.Clear();
            lvwPostBox.Items.Clear();
            lvwAccountTransactions.Items.Clear();
            lvwDepotPositions.Items.Clear();
            lvwAccountTransactions.Items.Clear();
            rtbLog.Clear();
            tabPageAccountTransactions.Text = "Konto-Transaktionen";
        }

        private void OnSessionTimeoutChanged(int expires_in)
        {
            btnSessionTimeout.Enabled = true;
            _currentSessionTimeout = expires_in;
            timSession.Stop();
            timSession.Start();
        }

        private async void timSession_Tick(object sender, EventArgs e)
        {
            _currentSessionTimeout--;
            lblSessionTimeout.Text = $"{_currentSessionTimeout} Sekunden";

            if ((_currentSessionTimeout <= AUTO_SESSION_REFRESH_LIMIT) && (_comdirectAPI != null))
            {
                RaiseNewLogMessage("Automatischer Session Refresh!");
                timSession.Stop();
                if (!await _comdirectAPI.RefreshSession())
                {
                    RaiseNewLogMessage("Refresh fehlgeschlagen - starte Logout!");
                    if (await _comdirectAPI.RevokeSession())
                    {
                        _comdirectAPI = null;
                        ResetUI();
                    }
                }
            }
        }

        private async void btnSessionTimeout_Click(object sender, EventArgs e)
        {
            if (_comdirectAPI == null) return;
            await _comdirectAPI.RefreshSession();
        }

        private async void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_comdirectAPI == null) return;
            await _comdirectAPI.RevokeSession();
        }

        #endregion

       
    }
}
