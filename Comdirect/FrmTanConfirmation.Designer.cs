namespace Comdirect;

partial class FrmTanConfirmation
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        btnOK = new Button();
        btnCancel = new Button();
        lblInfo = new Label();
        txtTAN = new TextBox();
        lblTAN = new Label();
        picBoxPhotoTan = new PictureBox();
        ((System.ComponentModel.ISupportInitialize)picBoxPhotoTan).BeginInit();
        SuspendLayout();
        // 
        // btnOK
        // 
        btnOK.DialogResult = DialogResult.OK;
        btnOK.Location = new Point(108, 73);
        btnOK.Name = "btnOK";
        btnOK.Size = new Size(90, 25);
        btnOK.TabIndex = 0;
        btnOK.Text = "&OK";
        btnOK.UseVisualStyleBackColor = true;
        // 
        // btnCancel
        // 
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new Point(12, 73);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(90, 25);
        btnCancel.TabIndex = 1;
        btnCancel.Text = "&Abbrechen";
        btnCancel.UseVisualStyleBackColor = true;
        // 
        // lblInfo
        // 
        lblInfo.AutoSize = true;
        lblInfo.Location = new Point(12, 9);
        lblInfo.Name = "lblInfo";
        lblInfo.Size = new Size(148, 15);
        lblInfo.TabIndex = 2;
        lblInfo.Text = "Bitte geben Sie die TAN ein";
        // 
        // txtTAN
        // 
        txtTAN.Location = new Point(98, 35);
        txtTAN.Name = "txtTAN";
        txtTAN.Size = new Size(100, 23);
        txtTAN.TabIndex = 3;
        // 
        // lblTAN
        // 
        lblTAN.AutoSize = true;
        lblTAN.Location = new Point(12, 38);
        lblTAN.Name = "lblTAN";
        lblTAN.Size = new Size(32, 15);
        lblTAN.TabIndex = 4;
        lblTAN.Text = "TAN:";
        // 
        // picBoxPhotoTan
        // 
        picBoxPhotoTan.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        picBoxPhotoTan.Location = new Point(209, 12);
        picBoxPhotoTan.Name = "picBoxPhotoTan";
        picBoxPhotoTan.Size = new Size(109, 83);
        picBoxPhotoTan.TabIndex = 5;
        picBoxPhotoTan.TabStop = false;
        // 
        // FrmTanConfirmation
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(330, 107);
        Controls.Add(picBoxPhotoTan);
        Controls.Add(lblTAN);
        Controls.Add(txtTAN);
        Controls.Add(lblInfo);
        Controls.Add(btnCancel);
        Controls.Add(btnOK);
        FormBorderStyle = FormBorderStyle.Fixed3D;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmTanConfirmation";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Session Freigabe";
        ((System.ComponentModel.ISupportInitialize)picBoxPhotoTan).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button btnOK;
    private Button btnCancel;
    private Label lblInfo;
    private TextBox txtTAN;
    private Label lblTAN;
    private PictureBox picBoxPhotoTan;
}