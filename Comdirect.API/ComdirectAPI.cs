using Comdirect.API.DataModels;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using System.Net;
using System.Text;
using System.Text.Json;

#region Undocumented API Information

// Es gibt aber ein paar Tricks, die es noch nicht in die Doku geschafft haben.
// Ihr könnt mit den Query-Parametern „min-bookingDate“ und „max-bookingDate“ ein Zeitfenster festlegen.
// Zum Beispiel können mit einer Anfrage mit diesem Query-Teil: „min - bookingDate = 2020 - 10 - 01 & max - bookingDate = 2020 - 11 - 01“
// die Umsätze des Monats Oktober abgerufen werden. Es kann auch nur der Parameter „min-bookingDate“ verwendet werden, um ein Anfangsdatum festzulegen.

// Mit dem Parameter „paging-count“ kann die Größe der Ergebnismenge verändert werden (Standardwert ist 20)
// und mit „paging-first“ kann der Startindex der abgefragten Menge (Standard ist hier 0) beeinflusst werden.

#endregion

namespace Comdirect.API;

/// <summary>Wrapper for the API</summary>
public class ComdirectAPI
{
    private string _username; // Zugangsnummer
    private string _password; // PIN
    private string _client_id; // Client ID
    private string _client_secret; // Client Secret
    private LoginResponse? _loginResponse;
    private ClientSession _clientSession;
    private ClientRequestId _clientRequestId;
    private ServerSession? _serverSession;
    private TanChallengeResponse? _tanChallengeResponse;
    private CDSecondaryResponse? _cdSecondaryResponse;
    private const string API_BASE_URL = "https://api.comdirect.de";

    // Logging
    public Action<string>? OnNewInfoLogMessage;
    public Action<string>? OnNewDebugLogMessage;
    public Action<int>? OnSessionTimeoutChanged;

    // HTTPClient
    HttpClient _httpClient;

    public ComdirectAPI(string username, string password, string client_id, string client_secret)
    {
        _username = username;
        _password = password;
        _client_id = client_id;
        _client_secret = client_secret;
        _clientSession = new ClientSession();
        _clientRequestId = new ClientRequestId(_clientSession);

        // Setup HttpClient
        var retryPipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
        .AddRetry(new HttpRetryStrategyOptions
        {
            BackoffType = DelayBackoffType.Exponential,
            MaxRetryAttempts = 3
        })
        .Build();

        SocketsHttpHandler socketHandler = new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(15)
        };
#pragma warning disable EXTEXP0001 // Der Typ dient nur zu Testzwecken und kann in zukünftigen Aktualisierungen geändert oder entfernt werden. Unterdrücken Sie diese Diagnose, um fortzufahren.
        ResilienceHandler resilienceHandler = new ResilienceHandler(retryPipeline)
        {
            InnerHandler = socketHandler,
        };
#pragma warning restore EXTEXP0001 // Der Typ dient nur zu Testzwecken und kann in zukünftigen Aktualisierungen geändert oder entfernt werden. Unterdrücken Sie diese Diagnose, um fortzufahren.
        _httpClient = new HttpClient(resilienceHandler);
    }

    #region 2.Login

    private void AddBasicLoginHeaders(HttpRequestMessage request)
    {
        request.Headers.Add("Authorization", $"Bearer {_loginResponse?.access_token}");
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("x-http-request-info", JsonSerializer.Serialize(_clientRequestId));
    }

    /// <summary>Step 1: Generate AccessToken</summary>
    /// <returns>True when an access tokes was received</returns>
    public async Task<bool> GetAccessToken()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{API_BASE_URL}/oauth/token");
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"client_id", _client_id},
            {"client_secret", _client_secret},
            {"grant_type", "password"},
            {"username", _username},
            {"password", _password}
        });
        request.Content = content;
        var response = await _httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        if (responseContent == null) return false;

        _loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent!);
        return _loginResponse != null && !string.IsNullOrEmpty(_loginResponse.access_token);
    }

    /// <summary>Step 2: Get Session Status</summary>
    /// <returns>True when an session identifier was received</returns>
    public async Task<bool> GetSessionStatus()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{API_BASE_URL}/api/session/clients/user/v1/sessions");
        AddBasicLoginHeaders(request);

        var response = await _httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        if (responseContent == null) return false;
        _serverSession = JsonSerializer.Deserialize<ServerSession[]>(responseContent)?.FirstOrDefault();
        return (_serverSession != null) && !String.IsNullOrEmpty(_serverSession.identifier);
    }

    /// <summary>Step 3: Start TAN Challenge</summary>
    public async Task<bool> StartTanChallenge()
    {
        return await StartTanChallenge(string.Empty);
    }

    /// <summary>Step 3: Start TAN Challenge</summary>
    public async Task<bool> StartTanChallenge(string forceChallengeType)
    {
        if (_serverSession == null) return false; // Called in wrong order?
        if (string.IsNullOrEmpty(_serverSession.identifier)) return false; // Called without getting sesssion status?

        // Set values to true due to documentation
        _serverSession.sessionTanActive = true;
        _serverSession.activated2FA = true;

        var request = new HttpRequestMessage(HttpMethod.Post, $"{API_BASE_URL}/api/session/clients/user/v1/sessions/{_serverSession.identifier}/validate");
        AddBasicLoginHeaders(request);

        // Force Challenge Type
        if (!string.IsNullOrEmpty(forceChallengeType))
        {
            request.Headers.Add("x-once-authentication-info", "{\"typ\":\"" + forceChallengeType + "\"}");
        }

        request.Content = new StringContent(JsonSerializer.Serialize(_serverSession), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode != HttpStatusCode.Created) return false;

        string tan_challenge_data = response.Headers.GetValues("x-once-authentication-info").First().ToString();
        _tanChallengeResponse = JsonSerializer.Deserialize<TanChallengeResponse>(tan_challenge_data);
        return true;
    }

    /// <summary>Get the TAN Challenge Type</summary>
    public string? GetTanChallengeType()
    {
        return _tanChallengeResponse?.typ;
    }

    /// <summary>Get the TAN Challenge</summary>
    public string? GetTanChallenge()
    {
        return _tanChallengeResponse?.challenge;
    }

    public async Task<bool> ActivateSession(string tan)
    {
        if (_serverSession == null) return false; // Called in wrong order?
        if (string.IsNullOrEmpty(_serverSession.identifier)) return false; // Called without getting sesssion status?
        if (_tanChallengeResponse == null) return false; // Called in wrong order?

        var request = new HttpRequestMessage(HttpMethod.Patch, $"{API_BASE_URL}/api/session/clients/user/v1/sessions/{_serverSession.identifier}");
        AddBasicLoginHeaders(request);
        request.Headers.Add("x-once-authentication-info", "{\"id\": \"" + _tanChallengeResponse.id + "\"}");
        if (!string.IsNullOrEmpty(tan))
        {
            request.Headers.Add("x-once-authentication", tan);
        }
        request.Content = new StringContent(JsonSerializer.Serialize(_serverSession), Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request);
        return response.StatusCode == HttpStatusCode.OK;
    }

    /// <summary>Step 5: Get CD Secondary FlowToken</summary>
    /// <returns>True when an access tokes was received</returns>
    public async Task<bool> GetCDSecondaryFlowToken()
    {
        if (_loginResponse == null) return false;
        if (string.IsNullOrEmpty(_loginResponse.access_token)) return false;

        var request = new HttpRequestMessage(HttpMethod.Post, $"{API_BASE_URL}/oauth/token");
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"client_id", _client_id},
            {"client_secret", _client_secret},
            {"grant_type", "cd_secondary"},
            {"token", _loginResponse.access_token}
        });
        request.Content = content;
        var response = await _httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        if (responseContent == null) return false;

        _cdSecondaryResponse = JsonSerializer.Deserialize<CDSecondaryResponse>(responseContent!);
        if (_cdSecondaryResponse == null) return false;

        OnSessionTimeoutChanged?.Invoke(_cdSecondaryResponse.expires_in - 2); // Subtract 2 seconds as we don't want to wait until the last second to refresh it
        return _cdSecondaryResponse != null && !string.IsNullOrEmpty(_cdSecondaryResponse.access_token);
    }

    #endregion

    #region 3. Token Flow

    public async Task<bool> RevokeSession()
    {
        if (_cdSecondaryResponse == null) return false;
        if (string.IsNullOrEmpty(_cdSecondaryResponse.access_token)) return false;

        var request = new HttpRequestMessage(HttpMethod.Delete, $"{API_BASE_URL}/oauth/revoke");
        request.Headers.Add("Authorization", $"Bearer {_cdSecondaryResponse?.access_token}");
        request.Headers.Add("Accept", "application/json");

        var response = await _httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();
        return response.StatusCode == HttpStatusCode.NoContent;

    }
    public async Task<bool> RefreshSession()
    {
        if (_cdSecondaryResponse == null) return false;
        if (string.IsNullOrEmpty(_cdSecondaryResponse.refresh_token)) return false;

        var request = new HttpRequestMessage(HttpMethod.Post, $"{API_BASE_URL}/oauth/token");
        request.Headers.Add("Authorization", $"Bearer {_cdSecondaryResponse?.access_token}");
        request.Headers.Add("Accept", "application/json");
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"client_id", _client_id},
            {"client_secret", _client_secret},
            {"grant_type", "refresh_token"},
            {"refresh_token", _cdSecondaryResponse!.refresh_token}
        });
        request.Content = content;
        var response = await _httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();
        if (responseContent == null) return false;

        var responseData = JsonSerializer.Deserialize<RefreshTokenResponse>(responseContent);
        if (responseData == null) return false;

        // Set new values to _cdSecondaryResponse
        _cdSecondaryResponse.access_token = responseData.access_token;
        _cdSecondaryResponse.expires_in = responseData.expires_in;
        _cdSecondaryResponse.refresh_token = responseData.refresh_token;
        OnSessionTimeoutChanged?.Invoke(responseData.expires_in - 2); // Subtract 2 seconds as we don't want to wait until the last second to refresh it

        return true;
    }

    #endregion

    #region 4. Accounts
    public async Task<AccountBalanceListResponse?> GetAllAccountBalances()
    {
        if (_cdSecondaryResponse == null) return null;
        if (string.IsNullOrEmpty(_cdSecondaryResponse.access_token)) return null;

        var request = new HttpRequestMessage(HttpMethod.Get, $"{API_BASE_URL}/api/banking/clients/user/v2/accounts/balances");
        request.Headers.Add("Authorization", $"Bearer {_cdSecondaryResponse?.access_token}");
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("x-http-request-info", JsonSerializer.Serialize(_clientRequestId));

        var response = await _httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();
        if (response.StatusCode != HttpStatusCode.OK) return null;
        if (responseContent == null) return null;

        return JsonSerializer.Deserialize<AccountBalanceListResponse>(responseContent);
    }

    public async Task<AccountBalanceResponse?> GetAccountBalance(string accountId)
    {
        if (_cdSecondaryResponse == null) return null;
        if (string.IsNullOrEmpty(_cdSecondaryResponse.access_token)) return null;

        var request = new HttpRequestMessage(HttpMethod.Get, $"{API_BASE_URL}/api/banking/v2/accounts/{accountId}/balances");
        request.Headers.Add("Authorization", $"Bearer {_cdSecondaryResponse?.access_token}");
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("x-http-request-info", JsonSerializer.Serialize(_clientRequestId));

        var response = await _httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();
        if (response.StatusCode != HttpStatusCode.OK) return null;
        if (responseContent == null) return null;

        return JsonSerializer.Deserialize<AccountBalanceResponse>(responseContent);
    }

    public async Task<AccountTransactionListResponse?> GetAccountTransactions(string accountId)
    {
        if (_cdSecondaryResponse == null) return null;
        if (string.IsNullOrEmpty(_cdSecondaryResponse.access_token)) return null;

        var request = new HttpRequestMessage(HttpMethod.Get, $"{API_BASE_URL}/api/banking/v2/accounts/{accountId}/transactions");
        request.Headers.Add("Authorization", $"Bearer {_cdSecondaryResponse?.access_token}");
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("x-http-request-info", JsonSerializer.Serialize(_clientRequestId));

        var response = await _httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();
        if (response.StatusCode != HttpStatusCode.OK) return null;
        if (responseContent == null) return null;

        return JsonSerializer.Deserialize<AccountTransactionListResponse>(responseContent);
    }

    #endregion

    #region 5. Depot
    public async Task<DepotResponseList?> GetAllDepots()
    {
        if (_cdSecondaryResponse == null) return null;
        if (string.IsNullOrEmpty(_cdSecondaryResponse.access_token)) return null;

        var request = new HttpRequestMessage(HttpMethod.Get, $"{API_BASE_URL}/api/brokerage/clients/user/v3/depots");
        request.Headers.Add("Authorization", $"Bearer {_cdSecondaryResponse?.access_token}");
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("x-http-request-info", JsonSerializer.Serialize(_clientRequestId));

        var response = await _httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();
        if (response.StatusCode != HttpStatusCode.OK) return null;
        if (responseContent == null) return null;

        return JsonSerializer.Deserialize<DepotResponseList>(responseContent);
    }

    public async Task<DepotPositionListResponse?> GetDepotPositions(string depotId)
    {
        if (_cdSecondaryResponse == null) return null;
        if (string.IsNullOrEmpty(_cdSecondaryResponse.access_token)) return null;

        var request = new HttpRequestMessage(HttpMethod.Get, $"{API_BASE_URL}/api/brokerage/v3/depots/{depotId}/positions?with-attr=instrument");
        request.Headers.Add("Authorization", $"Bearer {_cdSecondaryResponse?.access_token}");
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("x-http-request-info", JsonSerializer.Serialize(_clientRequestId));

        var response = await _httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();
        if (response.StatusCode != HttpStatusCode.OK) return null;
        if (responseContent == null) return null;

        return JsonSerializer.Deserialize<DepotPositionListResponse>(responseContent);
    }

    public async Task<DepotTransactionListResponse?> GetDepotTransactions(string depotId)
    {
        if (_cdSecondaryResponse == null) return null;
        if (string.IsNullOrEmpty(_cdSecondaryResponse.access_token)) return null;

        var request = new HttpRequestMessage(HttpMethod.Get, $"{API_BASE_URL}/api/brokerage/v3/depots/{depotId}/transactions");
        request.Headers.Add("Authorization", $"Bearer {_cdSecondaryResponse?.access_token}");
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("x-http-request-info", JsonSerializer.Serialize(_clientRequestId));

        var response = await _httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();
        if (response.StatusCode != HttpStatusCode.OK) return null;
        if (responseContent == null) return null;

        return JsonSerializer.Deserialize<DepotTransactionListResponse>(responseContent);
    }

    public async Task<string?> GetDepotPosition(string depotId, string positionId)
    {
        // TODO

        // Endpoint: GET /brokerage/v3/depots/{depotId}/positions/{positionId}

        /*
        // Query-Parameter: 
         „with-attr=instrument“: es wird das Befüllen des Attributs ' instrument' bei der Depotposition veranlasst  
         „without-attr=depot“: es wird die Lieferung des Depot-Objekts unterdrückt  
         „without-attr=positions“: es wird die Lieferung des Position-Objekts unterdrückt
         */
        await Task.Delay(1);
        throw new NotImplementedException();
    }

    #endregion

    #region 9. Postbox

    public async Task<DocumentListResponse?> GetDocuments()
    {
        // Default values when paging-first & paging-count is not set
        int defaultStartValue = 0;
        int defaultPageSize = 20;

        return await GetDocuments(defaultStartValue, defaultPageSize);
    }

    public async Task<DocumentListResponse?> GetDocuments(int start, int count)
    {
        if (_cdSecondaryResponse == null) return null;
        if (string.IsNullOrEmpty(_cdSecondaryResponse.access_token)) return null;

        var request = new HttpRequestMessage(HttpMethod.Get, $"{API_BASE_URL}/api/messages/clients/user/v2/documents?paging-first={start}&paging-count={count}");
        request.Headers.Add("Authorization", $"Bearer {_cdSecondaryResponse?.access_token}");
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("x-http-request-info", JsonSerializer.Serialize(_clientRequestId));

        var response = await _httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();
        if (response.StatusCode != HttpStatusCode.OK) return null;
        if (responseContent == null) return null;

        return JsonSerializer.Deserialize<DocumentListResponse>(responseContent);
    }

    public async Task<byte[]?> GetDocument(string? documentId, string? mimetype)
    {
        if (_cdSecondaryResponse == null) return null;
        if (string.IsNullOrEmpty(_cdSecondaryResponse.access_token)) return null;
        if (string.IsNullOrEmpty(documentId)) return null;
        if (string.IsNullOrEmpty(mimetype)) return null;

        var request = new HttpRequestMessage(HttpMethod.Get, $"{API_BASE_URL}/api/messages/v2/documents/{documentId}");
        request.Headers.Add("Authorization", $"Bearer {_cdSecondaryResponse?.access_token}");
        request.Headers.Add("Accept", mimetype);
        request.Headers.Add("x-http-request-info", JsonSerializer.Serialize(_clientRequestId));

        var response = await _httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsByteArrayAsync();
        if (response.StatusCode != HttpStatusCode.OK) return null;
        if (responseContent == null) return null;

        return responseContent;
    }

    #endregion

    #region 10. Reports

    public async Task<ReportResponse?> GetReport()
    {
        if (_cdSecondaryResponse == null) return null;
        if (string.IsNullOrEmpty(_cdSecondaryResponse.access_token)) return null;

        var request = new HttpRequestMessage(HttpMethod.Get, $"{API_BASE_URL}/api/reports/participants/user/v1/allbalances");
        request.Headers.Add("Authorization", $"Bearer {_cdSecondaryResponse?.access_token}");
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("x-http-request-info", JsonSerializer.Serialize(_clientRequestId));

        var response = await _httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();
        if (response.StatusCode != HttpStatusCode.OK) return null;
        if (responseContent == null) return null;

        return JsonSerializer.Deserialize<ReportResponse>(responseContent);
    }

    #endregion
}
