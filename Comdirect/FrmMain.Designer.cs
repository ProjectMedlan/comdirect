namespace Comdirect
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            btnLogin = new Button();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            grpLogin = new GroupBox();
            txtClientSecret = new TextBox();
            txtClientID = new TextBox();
            lblClientSecret = new Label();
            lblClientID = new Label();
            btnSessionTimeout = new Button();
            lblSessionTimeoutText = new Label();
            lblSessionTimeout = new Label();
            lblCustomerID = new Label();
            lblPIN = new Label();
            grpAccounts = new GroupBox();
            lvwAccounts = new ListView();
            chName = new ColumnHeader();
            chValue = new ColumnHeader();
            chInfo = new ColumnHeader();
            rtbLog = new RichTextBox();
            tabCtrl = new TabControl();
            tabPageAccountTransactions = new TabPage();
            lvwAccountTransactions = new ListView();
            chAccountTransactionDate = new ColumnHeader();
            chAccountTransactionSenderReceiver = new ColumnHeader();
            chAccountTransactionValue = new ColumnHeader();
            chAccountTransactionType = new ColumnHeader();
            tabPageDepotTransactions = new TabPage();
            lvwDepotTransactions = new ListView();
            chDepotTransactionBookingDate = new ColumnHeader();
            chDepotTransactionShortname = new ColumnHeader();
            chDepotTransactionQuantity = new ColumnHeader();
            chDepotTransactionTransactionValue = new ColumnHeader();
            chDepotTransactionTransactionType = new ColumnHeader();
            tabPageDepotPositions = new TabPage();
            lvwDepotPositions = new ListView();
            chDepotPositionName = new ColumnHeader();
            chDepotPositionQuantity = new ColumnHeader();
            chDepotPositionCurrentValue = new ColumnHeader();
            chDepotPositionTotalChange = new ColumnHeader();
            tabPagePostbox = new TabPage();
            lblPostBoxProgress = new Label();
            btnPostboxPrevious = new Button();
            btnPostboxNext = new Button();
            btnDownloadDocuments = new Button();
            cbOnlyNew = new CheckBox();
            lvwPostBox = new ListView();
            chCheckbox = new ColumnHeader();
            chDocumentDate = new ColumnHeader();
            chDocumentName = new ColumnHeader();
            ssMain = new StatusStrip();
            tsslInfo = new ToolStripStatusLabel();
            timSession = new System.Windows.Forms.Timer(components);
            grpLogin.SuspendLayout();
            grpAccounts.SuspendLayout();
            tabCtrl.SuspendLayout();
            tabPageAccountTransactions.SuspendLayout();
            tabPageDepotTransactions.SuspendLayout();
            tabPageDepotPositions.SuspendLayout();
            tabPagePostbox.SuspendLayout();
            ssMain.SuspendLayout();
            SuspendLayout();
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(232, 72);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(72, 52);
            btnLogin.TabIndex = 0;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(121, 72);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(100, 23);
            txtUsername.TabIndex = 1;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(121, 101);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(100, 23);
            txtPassword.TabIndex = 2;
            txtPassword.KeyPress += txtPassword_KeyPress;
            // 
            // grpLogin
            // 
            grpLogin.Controls.Add(txtClientSecret);
            grpLogin.Controls.Add(txtClientID);
            grpLogin.Controls.Add(lblClientSecret);
            grpLogin.Controls.Add(lblClientID);
            grpLogin.Controls.Add(btnSessionTimeout);
            grpLogin.Controls.Add(lblSessionTimeoutText);
            grpLogin.Controls.Add(lblSessionTimeout);
            grpLogin.Controls.Add(lblCustomerID);
            grpLogin.Controls.Add(lblPIN);
            grpLogin.Controls.Add(txtUsername);
            grpLogin.Controls.Add(btnLogin);
            grpLogin.Controls.Add(txtPassword);
            grpLogin.Location = new Point(12, 12);
            grpLogin.Name = "grpLogin";
            grpLogin.Size = new Size(310, 159);
            grpLogin.TabIndex = 3;
            grpLogin.TabStop = false;
            grpLogin.Text = "Login";
            // 
            // txtClientSecret
            // 
            txtClientSecret.Location = new Point(121, 43);
            txtClientSecret.Name = "txtClientSecret";
            txtClientSecret.Size = new Size(183, 23);
            txtClientSecret.TabIndex = 12;
            txtClientSecret.UseSystemPasswordChar = true;
            // 
            // txtClientID
            // 
            txtClientID.Location = new Point(121, 16);
            txtClientID.Name = "txtClientID";
            txtClientID.Size = new Size(183, 23);
            txtClientID.TabIndex = 11;
            txtClientID.UseSystemPasswordChar = true;
            // 
            // lblClientSecret
            // 
            lblClientSecret.AutoSize = true;
            lblClientSecret.Location = new Point(6, 46);
            lblClientSecret.Name = "lblClientSecret";
            lblClientSecret.Size = new Size(73, 15);
            lblClientSecret.TabIndex = 10;
            lblClientSecret.Text = "Client Secret";
            // 
            // lblClientID
            // 
            lblClientID.AutoSize = true;
            lblClientID.Location = new Point(6, 19);
            lblClientID.Name = "lblClientID";
            lblClientID.Size = new Size(54, 15);
            lblClientID.TabIndex = 9;
            lblClientID.Text = "Client-ID";
            // 
            // btnSessionTimeout
            // 
            btnSessionTimeout.Enabled = false;
            btnSessionTimeout.Location = new Point(232, 130);
            btnSessionTimeout.Name = "btnSessionTimeout";
            btnSessionTimeout.Size = new Size(72, 23);
            btnSessionTimeout.TabIndex = 8;
            btnSessionTimeout.Text = "Verlängern";
            btnSessionTimeout.UseVisualStyleBackColor = true;
            btnSessionTimeout.Click += btnSessionTimeout_Click;
            // 
            // lblSessionTimeoutText
            // 
            lblSessionTimeoutText.AutoSize = true;
            lblSessionTimeoutText.Location = new Point(6, 134);
            lblSessionTimeoutText.Name = "lblSessionTimeoutText";
            lblSessionTimeoutText.Size = new Size(92, 15);
            lblSessionTimeoutText.TabIndex = 6;
            lblSessionTimeoutText.Text = "Session-Timout:";
            // 
            // lblSessionTimeout
            // 
            lblSessionTimeout.AutoSize = true;
            lblSessionTimeout.Location = new Point(121, 134);
            lblSessionTimeout.Name = "lblSessionTimeout";
            lblSessionTimeout.Size = new Size(16, 15);
            lblSessionTimeout.TabIndex = 7;
            lblSessionTimeout.Text = "...";
            // 
            // lblCustomerID
            // 
            lblCustomerID.AutoSize = true;
            lblCustomerID.Location = new Point(6, 75);
            lblCustomerID.Name = "lblCustomerID";
            lblCustomerID.Size = new Size(99, 15);
            lblCustomerID.TabIndex = 4;
            lblCustomerID.Text = "Zugangsnummer";
            // 
            // lblPIN
            // 
            lblPIN.AutoSize = true;
            lblPIN.Location = new Point(6, 104);
            lblPIN.Name = "lblPIN";
            lblPIN.Size = new Size(26, 15);
            lblPIN.TabIndex = 5;
            lblPIN.Text = "PIN";
            // 
            // grpAccounts
            // 
            grpAccounts.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpAccounts.Controls.Add(lvwAccounts);
            grpAccounts.Location = new Point(328, 12);
            grpAccounts.Name = "grpAccounts";
            grpAccounts.Size = new Size(580, 159);
            grpAccounts.TabIndex = 0;
            grpAccounts.TabStop = false;
            grpAccounts.Text = "Kontenübersicht (Konto auswählen, um die Details zu laden)";
            // 
            // lvwAccounts
            // 
            lvwAccounts.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lvwAccounts.Columns.AddRange(new ColumnHeader[] { chName, chValue, chInfo });
            lvwAccounts.FullRowSelect = true;
            lvwAccounts.GridLines = true;
            lvwAccounts.Location = new Point(6, 22);
            lvwAccounts.MultiSelect = false;
            lvwAccounts.Name = "lvwAccounts";
            lvwAccounts.Size = new Size(568, 131);
            lvwAccounts.TabIndex = 4;
            lvwAccounts.UseCompatibleStateImageBehavior = false;
            lvwAccounts.View = View.Details;
            lvwAccounts.SelectedIndexChanged += lvwAccounts_SelectedIndexChanged;
            lvwAccounts.MouseMove += listview_MouseMove;
            // 
            // chName
            // 
            chName.Text = "Konto";
            chName.Width = 220;
            // 
            // chValue
            // 
            chValue.Text = "Aktueller Wert";
            chValue.TextAlign = HorizontalAlignment.Right;
            chValue.Width = 120;
            // 
            // chInfo
            // 
            chInfo.Text = "Info";
            chInfo.Width = 200;
            // 
            // rtbLog
            // 
            rtbLog.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rtbLog.BackColor = SystemColors.Window;
            rtbLog.Location = new Point(16, 451);
            rtbLog.Name = "rtbLog";
            rtbLog.ReadOnly = true;
            rtbLog.Size = new Size(892, 189);
            rtbLog.TabIndex = 0;
            rtbLog.Text = "";
            // 
            // tabCtrl
            // 
            tabCtrl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabCtrl.Controls.Add(tabPageAccountTransactions);
            tabCtrl.Controls.Add(tabPageDepotTransactions);
            tabCtrl.Controls.Add(tabPageDepotPositions);
            tabCtrl.Controls.Add(tabPagePostbox);
            tabCtrl.Location = new Point(12, 177);
            tabCtrl.Name = "tabCtrl";
            tabCtrl.SelectedIndex = 0;
            tabCtrl.Size = new Size(896, 272);
            tabCtrl.TabIndex = 4;
            // 
            // tabPageAccountTransactions
            // 
            tabPageAccountTransactions.Controls.Add(lvwAccountTransactions);
            tabPageAccountTransactions.Location = new Point(4, 24);
            tabPageAccountTransactions.Name = "tabPageAccountTransactions";
            tabPageAccountTransactions.Padding = new Padding(3);
            tabPageAccountTransactions.Size = new Size(888, 244);
            tabPageAccountTransactions.TabIndex = 0;
            tabPageAccountTransactions.Text = "Konto-Transaktionen";
            tabPageAccountTransactions.UseVisualStyleBackColor = true;
            // 
            // lvwAccountTransactions
            // 
            lvwAccountTransactions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvwAccountTransactions.Columns.AddRange(new ColumnHeader[] { chAccountTransactionDate, chAccountTransactionSenderReceiver, chAccountTransactionValue, chAccountTransactionType });
            lvwAccountTransactions.FullRowSelect = true;
            lvwAccountTransactions.GridLines = true;
            lvwAccountTransactions.Location = new Point(6, 35);
            lvwAccountTransactions.MultiSelect = false;
            lvwAccountTransactions.Name = "lvwAccountTransactions";
            lvwAccountTransactions.Size = new Size(876, 203);
            lvwAccountTransactions.TabIndex = 0;
            lvwAccountTransactions.UseCompatibleStateImageBehavior = false;
            lvwAccountTransactions.View = View.Details;
            lvwAccountTransactions.MouseMove += listview_MouseMove;
            // 
            // chAccountTransactionDate
            // 
            chAccountTransactionDate.Text = "Buchungsdatum";
            chAccountTransactionDate.Width = 100;
            // 
            // chAccountTransactionSenderReceiver
            // 
            chAccountTransactionSenderReceiver.Text = "Sender / Empfänger";
            chAccountTransactionSenderReceiver.Width = 350;
            // 
            // chAccountTransactionValue
            // 
            chAccountTransactionValue.Text = "Betrag";
            chAccountTransactionValue.TextAlign = HorizontalAlignment.Right;
            chAccountTransactionValue.Width = 100;
            // 
            // chAccountTransactionType
            // 
            chAccountTransactionType.Text = "Transaktionsart";
            chAccountTransactionType.Width = 200;
            // 
            // tabPageDepotTransactions
            // 
            tabPageDepotTransactions.Controls.Add(lvwDepotTransactions);
            tabPageDepotTransactions.Location = new Point(4, 24);
            tabPageDepotTransactions.Name = "tabPageDepotTransactions";
            tabPageDepotTransactions.Padding = new Padding(3);
            tabPageDepotTransactions.Size = new Size(888, 244);
            tabPageDepotTransactions.TabIndex = 2;
            tabPageDepotTransactions.Text = "Depot-Transaktionen";
            tabPageDepotTransactions.UseVisualStyleBackColor = true;
            // 
            // lvwDepotTransactions
            // 
            lvwDepotTransactions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvwDepotTransactions.Columns.AddRange(new ColumnHeader[] { chDepotTransactionBookingDate, chDepotTransactionShortname, chDepotTransactionQuantity, chDepotTransactionTransactionValue, chDepotTransactionTransactionType });
            lvwDepotTransactions.FullRowSelect = true;
            lvwDepotTransactions.GridLines = true;
            lvwDepotTransactions.Location = new Point(6, 35);
            lvwDepotTransactions.MultiSelect = false;
            lvwDepotTransactions.Name = "lvwDepotTransactions";
            lvwDepotTransactions.ShowItemToolTips = true;
            lvwDepotTransactions.Size = new Size(876, 203);
            lvwDepotTransactions.TabIndex = 0;
            lvwDepotTransactions.UseCompatibleStateImageBehavior = false;
            lvwDepotTransactions.View = View.Details;
            lvwDepotTransactions.MouseMove += listview_MouseMove;
            // 
            // chDepotTransactionBookingDate
            // 
            chDepotTransactionBookingDate.Text = "Buchungsdatum";
            chDepotTransactionBookingDate.Width = 120;
            // 
            // chDepotTransactionShortname
            // 
            chDepotTransactionShortname.Text = "Wertpapier";
            chDepotTransactionShortname.Width = 300;
            // 
            // chDepotTransactionQuantity
            // 
            chDepotTransactionQuantity.Text = "Anzahl";
            chDepotTransactionQuantity.TextAlign = HorizontalAlignment.Right;
            chDepotTransactionQuantity.Width = 80;
            // 
            // chDepotTransactionTransactionValue
            // 
            chDepotTransactionTransactionValue.Text = "Gesamtwert";
            chDepotTransactionTransactionValue.TextAlign = HorizontalAlignment.Right;
            chDepotTransactionTransactionValue.Width = 120;
            // 
            // chDepotTransactionTransactionType
            // 
            chDepotTransactionTransactionType.Text = "Transaktionsart";
            chDepotTransactionTransactionType.Width = 120;
            // 
            // tabPageDepotPositions
            // 
            tabPageDepotPositions.Controls.Add(lvwDepotPositions);
            tabPageDepotPositions.Location = new Point(4, 24);
            tabPageDepotPositions.Name = "tabPageDepotPositions";
            tabPageDepotPositions.Padding = new Padding(3);
            tabPageDepotPositions.Size = new Size(888, 244);
            tabPageDepotPositions.TabIndex = 3;
            tabPageDepotPositions.Text = "Depot-Positionen";
            tabPageDepotPositions.UseVisualStyleBackColor = true;
            // 
            // lvwDepotPositions
            // 
            lvwDepotPositions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvwDepotPositions.Columns.AddRange(new ColumnHeader[] { chDepotPositionName, chDepotPositionQuantity, chDepotPositionCurrentValue, chDepotPositionTotalChange });
            lvwDepotPositions.FullRowSelect = true;
            lvwDepotPositions.GridLines = true;
            lvwDepotPositions.Location = new Point(6, 35);
            lvwDepotPositions.Name = "lvwDepotPositions";
            lvwDepotPositions.Size = new Size(876, 203);
            lvwDepotPositions.TabIndex = 0;
            lvwDepotPositions.UseCompatibleStateImageBehavior = false;
            lvwDepotPositions.View = View.Details;
            lvwDepotPositions.MouseMove += listview_MouseMove;
            // 
            // chDepotPositionName
            // 
            chDepotPositionName.Text = "Titel";
            chDepotPositionName.Width = 400;
            // 
            // chDepotPositionQuantity
            // 
            chDepotPositionQuantity.Text = "Anzahl";
            chDepotPositionQuantity.TextAlign = HorizontalAlignment.Right;
            chDepotPositionQuantity.Width = 80;
            // 
            // chDepotPositionCurrentValue
            // 
            chDepotPositionCurrentValue.Text = "Aktueller Wert";
            chDepotPositionCurrentValue.TextAlign = HorizontalAlignment.Right;
            chDepotPositionCurrentValue.Width = 100;
            // 
            // chDepotPositionTotalChange
            // 
            chDepotPositionTotalChange.Text = "Absolute Änderung";
            chDepotPositionTotalChange.TextAlign = HorizontalAlignment.Right;
            chDepotPositionTotalChange.Width = 120;
            // 
            // tabPagePostbox
            // 
            tabPagePostbox.Controls.Add(lblPostBoxProgress);
            tabPagePostbox.Controls.Add(btnPostboxPrevious);
            tabPagePostbox.Controls.Add(btnPostboxNext);
            tabPagePostbox.Controls.Add(btnDownloadDocuments);
            tabPagePostbox.Controls.Add(cbOnlyNew);
            tabPagePostbox.Controls.Add(lvwPostBox);
            tabPagePostbox.Location = new Point(4, 24);
            tabPagePostbox.Name = "tabPagePostbox";
            tabPagePostbox.Padding = new Padding(3);
            tabPagePostbox.Size = new Size(888, 244);
            tabPagePostbox.TabIndex = 1;
            tabPagePostbox.Text = "Postbox";
            tabPagePostbox.UseVisualStyleBackColor = true;
            // 
            // lblPostBoxProgress
            // 
            lblPostBoxProgress.AutoSize = true;
            lblPostBoxProgress.Location = new Point(87, 13);
            lblPostBoxProgress.Name = "lblPostBoxProgress";
            lblPostBoxProgress.Size = new Size(16, 15);
            lblPostBoxProgress.TabIndex = 6;
            lblPostBoxProgress.Text = "...";
            // 
            // btnPostboxPrevious
            // 
            btnPostboxPrevious.Location = new Point(6, 8);
            btnPostboxPrevious.Name = "btnPostboxPrevious";
            btnPostboxPrevious.Size = new Size(75, 23);
            btnPostboxPrevious.TabIndex = 5;
            btnPostboxPrevious.Text = "Zu&rück";
            btnPostboxPrevious.UseVisualStyleBackColor = true;
            btnPostboxPrevious.Click += btnPostboxPrevious_Click;
            // 
            // btnPostboxNext
            // 
            btnPostboxNext.Location = new Point(211, 9);
            btnPostboxNext.Name = "btnPostboxNext";
            btnPostboxNext.Size = new Size(75, 23);
            btnPostboxNext.TabIndex = 4;
            btnPostboxNext.Text = "Wei&ter";
            btnPostboxNext.UseVisualStyleBackColor = true;
            btnPostboxNext.Click += btnPostboxNext_Click;
            // 
            // btnDownloadDocuments
            // 
            btnDownloadDocuments.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDownloadDocuments.Enabled = false;
            btnDownloadDocuments.Location = new Point(761, 9);
            btnDownloadDocuments.Name = "btnDownloadDocuments";
            btnDownloadDocuments.Size = new Size(121, 23);
            btnDownloadDocuments.TabIndex = 2;
            btnDownloadDocuments.Text = "Herunterladen";
            btnDownloadDocuments.UseVisualStyleBackColor = true;
            btnDownloadDocuments.Click += btnDownloadDocuments_Click;
            // 
            // cbOnlyNew
            // 
            cbOnlyNew.AutoSize = true;
            cbOnlyNew.Checked = true;
            cbOnlyNew.CheckState = CheckState.Checked;
            cbOnlyNew.Location = new Point(596, 11);
            cbOnlyNew.Name = "cbOnlyNew";
            cbOnlyNew.Size = new Size(159, 19);
            cbOnlyNew.TabIndex = 1;
            cbOnlyNew.Text = "Nur ungelesene anzeigen";
            cbOnlyNew.UseVisualStyleBackColor = true;
            cbOnlyNew.CheckedChanged += cbOnlyNew_CheckedChanged;
            // 
            // lvwPostBox
            // 
            lvwPostBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvwPostBox.CheckBoxes = true;
            lvwPostBox.Columns.AddRange(new ColumnHeader[] { chCheckbox, chDocumentDate, chDocumentName });
            lvwPostBox.FullRowSelect = true;
            lvwPostBox.GridLines = true;
            lvwPostBox.Location = new Point(6, 37);
            lvwPostBox.Name = "lvwPostBox";
            lvwPostBox.Size = new Size(876, 204);
            lvwPostBox.TabIndex = 0;
            lvwPostBox.UseCompatibleStateImageBehavior = false;
            lvwPostBox.View = View.Details;
            lvwPostBox.ColumnClick += lvwPostBox_ColumnClick;
            lvwPostBox.ItemChecked += lvwPostBox_ItemChecked;
            lvwPostBox.MouseMove += listview_MouseMove;
            // 
            // chCheckbox
            // 
            chCheckbox.Text = "";
            chCheckbox.Width = 20;
            // 
            // chDocumentDate
            // 
            chDocumentDate.Text = "Datum";
            chDocumentDate.Width = 100;
            // 
            // chDocumentName
            // 
            chDocumentName.Text = "Name";
            chDocumentName.Width = 500;
            // 
            // ssMain
            // 
            ssMain.Items.AddRange(new ToolStripItem[] { tsslInfo });
            ssMain.Location = new Point(0, 651);
            ssMain.Name = "ssMain";
            ssMain.Size = new Size(920, 22);
            ssMain.TabIndex = 5;
            // 
            // tsslInfo
            // 
            tsslInfo.Name = "tsslInfo";
            tsslInfo.Size = new Size(0, 17);
            // 
            // timSession
            // 
            timSession.Interval = 1000;
            timSession.Tick += timSession_Tick;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(920, 673);
            Controls.Add(ssMain);
            Controls.Add(tabCtrl);
            Controls.Add(rtbLog);
            Controls.Add(grpAccounts);
            Controls.Add(grpLogin);
            Name = "FrmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Mein Comdirect";
            FormClosing += FrmMain_FormClosing;
            Load += FrmMain_Load;
            Shown += FrmMain_Shown;
            grpLogin.ResumeLayout(false);
            grpLogin.PerformLayout();
            grpAccounts.ResumeLayout(false);
            tabCtrl.ResumeLayout(false);
            tabPageAccountTransactions.ResumeLayout(false);
            tabPageDepotTransactions.ResumeLayout(false);
            tabPageDepotPositions.ResumeLayout(false);
            tabPagePostbox.ResumeLayout(false);
            tabPagePostbox.PerformLayout();
            ssMain.ResumeLayout(false);
            ssMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnLogin;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private GroupBox grpLogin;
        private GroupBox grpAccounts;
        private ListView lvwAccounts;
        private ColumnHeader chName;
        private ColumnHeader chValue;
        private RichTextBox rtbLog;
        private ColumnHeader chInfo;
        private Label lblCustomerID;
        private Label lblPIN;
        private TabControl tabCtrl;
        private TabPage tabPageAccountTransactions;
        private TabPage tabPagePostbox;
        private CheckBox cbOnlyNew;
        private ListView lvwPostBox;
        private ColumnHeader chDocumentDate;
        private ColumnHeader chDocumentName;
        private Button btnDownloadDocuments;
        private StatusStrip ssMain;
        private ToolStripStatusLabel tsslInfo;
        private System.Windows.Forms.Timer timSession;
        private Button btnSessionTimeout;
        private Label lblSessionTimeoutText;
        private Label lblSessionTimeout;
        private ListView lvwAccountTransactions;
        private ColumnHeader chCheckbox;
        private TabPage tabPageDepotTransactions;
        private TabPage tabPageDepotPositions;
        private ColumnHeader chAccountTransactionDate;
        private ColumnHeader chAccountTransactionSenderReceiver;
        private ColumnHeader chAccountTransactionValue;
        private ColumnHeader chAccountTransactionType;
        private ListView lvwDepotTransactions;
        private ColumnHeader chDepotTransactionBookingDate;
        private ColumnHeader chDepotTransactionShortname;
        private ColumnHeader chDepotTransactionQuantity;
        private ColumnHeader chDepotTransactionTransactionType;
        private ColumnHeader chDepotTransactionTransactionValue;
        private ListView lvwDepotPositions;
        private ColumnHeader chDepotPositionName;
        private ColumnHeader chDepotPositionQuantity;
        private ColumnHeader chDepotPositionCurrentValue;
        private ColumnHeader chDepotPositionTotalChange;
        private TextBox txtClientSecret;
        private TextBox txtClientID;
        private Label lblClientSecret;
        private Label lblClientID;
        private Label lblPostBoxProgress;
        private Button btnPostboxPrevious;
        private Button btnPostboxNext;
    }
}
