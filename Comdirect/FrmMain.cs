using Comdirect.API;
using Comdirect.API.DataModels;
using Comdirect.BLL;
using Comdirect.Shared;
using Comdirect.ViewModelConverter;
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
        private const int MAX_POSTBOX_ITEMS_PER_REQUEST = 20; // Max 20 items per request (default)

        private ReportViewModel? _lastReport;
        private readonly Dictionary<string, List<AccountTransactionViewModel>> _accountTransactionCache = [];
        private readonly Dictionary<string, List<DepotTransactionViewModel>> _depotTransactionCache = [];
        private readonly Dictionary<string, List<DepotPositionViewModel>> _depotPositionCache = [];
        private readonly Dictionary<string, List<DepotDetailsViewModel>> _depotDetailsCache = [];

        private PostboxNavigator? _postboxNavigator;

        private int _currentSessionTimeout = 0;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        private const int WM_VSCROLL = 277;
        private const int SB_PAGEBOTTOM = 7;

        private readonly ToolTip _listViewToolTip = new();
        private Point _listViewHoverPosition = new(-1, -1);
        private ListViewItem? _lastListViewItem = null;

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
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
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
            if (_comdirectAPI != null)
            {
                // Logout
                _comdirectAPI?.RevokeSession();
                _comdirectAPI = null;
                ResetUI();
                return;
            }

            if (await PerformFullLogin())
            {
                btnLogin.Text = "Logout";
                await LoadReport();
                await LoadPostBoxEntries(1);
                _postboxNavigator?.Refresh();
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

            if (string.IsNullOrEmpty(tan_type))
            {
                RaiseNewLogMessage("Login: Schritt 3: Tan-Type konnte nicht abgerufen werden");
                return false;
            }

            string tan = "";

            var tanConfirmation = new FrmTanConfirmation();
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

                var lvi = new ListViewItem
                {
                    Text = "Gesamt"
                };
                lvi.SubItems.Add(totalBalance.ToString("C"));
                lvi.SubItems.Add("Inkl. Kreditlimit: " + (totalBalance + totalCreditLimit).ToString("C"));
                lvwAccounts.Items.Add(lvi);
                lvwAccounts.ResumeLayout();
            }
        }

        private static ListViewItem ConvertToListItem(ReportAccountViewModel item)
        {
            var lvi = new ListViewItem
            {
                Tag = item,
                Text = item.AccountDescription
            };
            lvi.SubItems.Add(item.BalanceInEuro.ToString("C"));
            if (item.CreditLimitInEuro > 0)
                lvi.SubItems.Add("Dispo: " + item.CreditLimitInEuro.ToString("C"));
            return lvi;
        }

        private static ListViewItem ConvertToListItem(ReportCardViewModel item)
        {
            var lvi = new ListViewItem
            {
                Tag = item,
                Text = item.CardType
            };
            lvi.SubItems.Add(item.CardBalanceInEuro.ToString("C"));
            lvi.SubItems.Add("Status: " + item.CardStatus);
            return lvi;
        }

        private static ListViewItem ConvertToListItem(ReportDepotViewModel item)
        {
            var lvi = new ListViewItem
            {
                Tag = item,
                Text = "Wertpapierdepot"
            };
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

            var transactions = await LoadIntoCache<AccountTransactionListResponse, AccountTransaction, AccountTransactionViewModel>(
                _accountTransactionCache, accountId, forceReload,
                _comdirectAPI.GetAccountTransactions,
                response => response.values,
                transaction => transaction.ConvertToViewModel(),
                "Kontotransaktionen");
            if (transactions == null || transactions.Count == 0) return;

            // Show in ListView
            lvwAccountTransactions.Items.Clear();
            lvwAccountTransactions.SuspendLayout();
            foreach (var currentTransaction in _accountTransactionCache[accountId])
            {
                var lvi = new ListViewItem
                {
                    UseItemStyleForSubItems = false,
                    Tag = currentTransaction,
                    Text = currentTransaction.BookingDate.HasValue ? currentTransaction.BookingDate.Value.ToString("dd.MM.yyyy") : "Offen"
                };
                lvi.SubItems.Add(currentTransaction.Remitter ?? currentTransaction.Creditor ?? currentTransaction.CategoryDisplayName ?? "");
                lvi.SubItems.Add(currentTransaction.TransactionValue.ToString("C")).ForeColor = currentTransaction.TransactionValue > 0 ? Color.Green : Color.Red;
                lvi.SubItems.Add(currentTransaction.TransactionTypeDisplayName ?? "");
                lvi.ToolTipText = string.Join(Environment.NewLine, currentTransaction.RemittanceInfo);
                lvwAccountTransactions.Items.Add(lvi);
            }
            lvwAccountTransactions.ResumeLayout();
            tabCtrl.SelectedTab = tabPageAccountTransactions;

            string? tabLabel = _lastReport?.Accounts.FirstOrDefault(x => x.AccountId == accountId)?.AccountDescription;
            tabPageAccountTransactions.Text = tabLabel ?? "Konto-Transaktionen";
        }

        private async Task RefreshDepotTransactions(string depotId, bool forceReload)
        {
            if (_comdirectAPI == null) return;
            if (string.IsNullOrEmpty(depotId)) return;

            var transactions = await LoadIntoCache<DepotTransactionListResponse, DepotTransaction, DepotTransactionViewModel>(
                _depotTransactionCache, depotId, forceReload,
                _comdirectAPI.GetDepotTransactions,
                response => response.values,
                transaction => transaction.ConvertToViewModel(),
                "Depottransaktionen");
            if (transactions == null || transactions.Count == 0) return;

            // Show in ListView
            lvwDepotTransactions.Items.Clear();
            lvwDepotTransactions.SuspendLayout();
            foreach (var currentTransaction in _depotTransactionCache[depotId])
            {
                var lvi = new ListViewItem
                {
                    Tag = currentTransaction,
                    Text = currentTransaction.BookingDate.HasValue ? currentTransaction.BookingDate.Value.ToString("dd.MM.yyyy") : "Offen"
                };
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

            var positions = await LoadIntoCache<DepotPositionListResponse, DepotPosition, DepotPositionViewModel>(
                _depotPositionCache, depotId, forceReload,
                _comdirectAPI.GetDepotPositions,
                response => response.values,
                position => position.ConvertToViewModel(),
                "Depotpositionen",
                response => _depotDetailsCache[depotId] = [response.aggregated.ConvertToViewModel()]);
            if (positions == null || positions.Count == 0) return;

            // Set the Depot ToolTip on the Account Listview Depts Entry
            ListViewItem? depotItem = lvwAccounts.Items.Cast<ListViewItem>().FirstOrDefault(x => x.Tag is ReportDepotViewModel && ((ReportDepotViewModel)x.Tag!).DepotId == depotId);
            DepotDetailsViewModel? depotDetails = _depotDetailsCache[depotId].FirstOrDefault();
            if ((depotItem != null) && (depotDetails != null))
            {
                var depotToolTip = new StringBuilder();
                depotToolTip.AppendLine($"Aktueller Wert: {depotDetails.CurrentValue.ToString("C")}");
                depotToolTip.AppendLine($"Vortag: {depotDetails.PreviousDayValue.ToString("C")}");
                depotToolTip.AppendLine($"Kaufwert: {depotDetails.PurchaseValue.ToString("C")}");
                depotToolTip.AppendLine($"Veränderung: {depotDetails.TotalProfitOrLoss.ToString("C")}");
                depotItem.ToolTipText = depotToolTip.ToString();
            }


            // Show in ListView
            lvwDepotPositions.Items.Clear();
            lvwDepotPositions.SuspendLayout();
            List<DepotPositionViewModel> sortedPositions = [.. _depotPositionCache[depotId].OrderBy(x => x.Instrument?.Name)];
            foreach (var currentPosition in sortedPositions)
            {
                var lvi = new ListViewItem
                {
                    UseItemStyleForSubItems = false,
                    Tag = currentPosition,
                    Text = currentPosition.Instrument?.Name
                };
                lvi.SubItems.Add(currentPosition.Quantity.ToString());
                lvi.SubItems.Add(currentPosition.CurrentValue.ToString("C"));
                lvi.SubItems.Add(currentPosition.TotalProfitOrLoss.ToString("C")).ForeColor = currentPosition.TotalProfitOrLoss > 0 ? Color.Green : Color.Red;
                lvwDepotPositions.Items.Add(lvi);
            }
            lvwDepotPositions.ResumeLayout();
        }

        /// <summary>
        /// Loads a list-based response into the given cache (respecting a reload flag), converts the
        /// items to their ViewModels and returns the cached list. Returns null if the API call fails.
        /// </summary>
        /// <param name="onLoaded">Optional hook to process the raw response (e.g. to fill an additional cache) when data was (re)loaded.</param>
        private async Task<List<TViewModel>?> LoadIntoCache<TResponse, TSource, TViewModel>(
            Dictionary<string, List<TViewModel>> cache,
            string key,
            bool forceReload,
            Func<string, Task<TResponse?>> fetch,
            Func<TResponse, IEnumerable<TSource>> itemsSelector,
            Func<TSource, TViewModel> converter,
            string logLabel,
            Action<TResponse>? onLoaded = null)
        {
            if (cache.TryGetValue(key, out var cached) && !forceReload)
                return cached;

            var response = await fetch(key);
            if (response == null) return null;

            onLoaded?.Invoke(response);

            var items = itemsSelector(response).Select(converter).ToList();
            cache[key] = items;
            RaiseNewLogMessage($"{items.Count} {logLabel} geladen");
            return items;
        }

        #endregion

        #region Postbox

        private async void btnPostboxNext_Click(object sender, EventArgs e)
        {
            if (_postboxNavigator == null) return;

            // Check if cache is filled for next page
            if (!_postboxNavigator.HasDataForNextPage())
            {
                btnPostboxNext.Enabled = false;
                await LoadPostBoxEntries(_postboxNavigator.CurrentPage + 1);
                btnPostboxNext.Enabled = true;
            }

            _postboxNavigator.NextPage();
        }

        private void btnPostboxPrevious_Click(object sender, EventArgs e)
        {
            if (_postboxNavigator == null) return;

            _postboxNavigator.PreviousPage();
        }

        private async Task LoadPostBoxEntries(int currentPage)
        {
            if (_comdirectAPI == null) return;

            var postboxData = await _comdirectAPI.GetDocuments((currentPage - 1) * MAX_POSTBOX_ITEMS_PER_REQUEST, MAX_POSTBOX_ITEMS_PER_REQUEST);
            if (postboxData != null)
            {
                var documentListViewModel = postboxData.ConvertToViewModel();

                if (_postboxNavigator == null)
                {
                    _postboxNavigator = new PostboxNavigator(documentListViewModel.TotalDocuments, MAX_POSTBOX_ITEMS_PER_REQUEST);
                    _postboxNavigator.OnDocumentsToViewChanged += ListPostboxDocuments;
                    _postboxNavigator.CurrentProgressChanged += PostboxProgressChanged;
                    RaiseNewLogMessage(Enumerations.LogTypes.Info, $"Sie haben {documentListViewModel.TotalUnreadDocuments} ungelesene Nachrichten in der Postbox");
                }

                _postboxNavigator.AddDocuments(documentListViewModel.Documents);
            }
        }

        private void PostboxProgressChanged(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(PostboxProgressChanged), message);
                return;
            }
            lblPostBoxProgress.Text = message;
        }

        private void ListPostboxDocuments(List<DocumentViewModel> currentList)
        {
            lvwPostBox.Items.Clear();
            lvwPostBox.SuspendLayout();
            foreach (var item in currentList)
            {
                var lvi = new ListViewItem
                {
                    Tag = item,
                    Text = string.Empty // No Content - just checkboxes
                };
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

        private async void btnDownloadDocuments_Click(object sender, EventArgs e)
        {
            btnDownloadDocuments.Enabled = false;
            lvwPostBox.Enabled = false;

            if (_comdirectAPI == null) return;

            var folderDialog = new FolderBrowserDialog
            {
                InitialDirectory = (_userSettings != null && !string.IsNullOrEmpty(_userSettings.DefaultDownloadDirectory)) ? _userSettings.DefaultDownloadDirectory : "",
                ShowNewFolderButton = true
            };
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
                string regSearch = new(Path.GetInvalidFileNameChars());
                var rg = new Regex(string.Format("[{0}]", Regex.Escape(regSearch)));
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
            _postboxNavigator?.Refresh(); // Force view to update in case I set some of the documents to read
            btnDownloadDocuments.Enabled = true;
            lvwPostBox.Enabled = true;
        }

        private void cbOnlyNew_CheckedChanged(object sender, EventArgs e)
        {
            _postboxNavigator?.SetFilter(cbOnlyNew.Checked);
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

        #region ListView ToolTip Fix
        // https://stackoverflow.com/questions/13069137/how-to-set-tooltip-for-a-listviewsubitem

        private void listview_MouseMove(object sender, MouseEventArgs e)
        {
            ListView listView = (ListView)sender;
            ListViewHitTestInfo info = listView.HitTest(e.X, e.Y);

            if (info.Item == null)
            {
                _listViewToolTip.RemoveAll();
                _lastListViewItem = null;
                return;
            }

            if (_listViewHoverPosition == e.Location) return;
            _listViewHoverPosition = e.Location;

            if (info.Item != _lastListViewItem)
            {
                _lastListViewItem = info.Item;
                _listViewToolTip.Show(info.Item.ToolTipText, listView, e.X, e.Y, 20000);
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
                this.Invoke(new Action<Enumerations.LogTypes, string>(RaiseNewLogMessage), logType, message);
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
