﻿namespace Comdirect.API.DataModels;

public class LoginResponse
{
    public string? access_token { get; set; }
    public string? token_type { get; set; }
    public string? refresh_token { get; set; }
    public int expires_in { get; set; }
    public string? scope { get; set; }
    public string? kdnr { get; set; }
    public int bpid { get; set; }
    public long kontaktId { get; set; }
}
