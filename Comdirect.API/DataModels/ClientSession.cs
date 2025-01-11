﻿namespace Comdirect.API.DataModels;
public class ClientSession
{
    public string sessionId { get; init; }
    public string requestId { get; init; }

    public ClientSession()
    {
        sessionId = Guid.CreateVersion7().ToString();
        requestId = DateTime.Now.ToString("HHmmssfff");
    }
}