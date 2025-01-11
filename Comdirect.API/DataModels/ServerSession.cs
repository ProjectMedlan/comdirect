namespace Comdirect.API.DataModels;
public class ServerSession
{
    public string? identifier { get; set; }
    public bool sessionTanActive { get; set; }
    public bool activated2FA { get; set; }
}
