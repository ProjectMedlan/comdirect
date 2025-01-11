using Comdirect.API;

namespace Comdirect;
public partial class FrmTanConfirmation : Form
{
    private readonly Size SMALL_SIZE = new Size(300, 150);
    private readonly Size LARGE_SIZE = new Size(450, 280);

    public FrmTanConfirmation()
    {
        InitializeComponent();
    }

    public void Init(string mode, string challenge)
    {
        // I guess this is not supported anymore
        if (mode == Constants.TAN_TYPE_MOBILE_TAN)
        {
            this.Size = SMALL_SIZE;
            // challenge = Mobile Phone Number
            lblInfo.Text = "Bitte geben Sie die TAN ein, "+Environment.NewLine+"die an ihre Mobilfunknummer gesendet wurde.";
            picBoxPhotoTan.Visible = false;
            txtTAN.Focus();
        }

        if (mode == Constants.TAN_TYPE_PUSH_TAN)
        {
            this.Size = SMALL_SIZE;
            lblInfo.Text = "Bitte geben Sie die Transaktion in "+Environment.NewLine+"Ihrer Phototan App frei.";
            lblInfo.Size = new Size(lblInfo.Size.Width, lblInfo.Size.Height*2);
            lblTAN.Visible = false;
            txtTAN.Visible = false;
            picBoxPhotoTan.Visible = false;
        }

        if (mode == Constants.TAN_TYPE_PHOTO_TAN)
        {
            this.Size = LARGE_SIZE;
            lblInfo.Text = "Bitte geben Sie die Photo TAN ein.";
            // Decode Base 64 String in pic
            byte[] imageBytes = Convert.FromBase64String(challenge);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                picBoxPhotoTan.Image = Image.FromStream(ms);
            }
            txtTAN.Focus();
        }
    }

    public string GetTan()
    {
        return txtTAN.Text;
    }
}
