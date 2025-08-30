namespace Comdirect.API.DataModels;

public class AccountBalanceListResponse
{
    public ComdirectPaging paging { get; set; }
    public AccountBalanceResponse[] values { get; set; }
}

public class AccountBalanceResponse
{
    public AccountBalanceAccount account { get; set; }
    public string accountId { get; set; }
    public Balance balance { get; set; }
    public Balance balanceEUR { get; set; }
    public Balance availableCashAmount { get; set; }
    public Balance availableCashAmountEUR { get; set; }
}

public class AccountBalanceAccount
{
    public string accountId { get; set; }
    public string accountDisplayId { get; set; }
    public string currency { get; set; }
    public string clientId { get; set; }
    public AccountBalanceAccounttype accountType { get; set; }
    public string iban { get; set; }
    public string bic { get; set; }
    public Balance creditLimit { get; set; }
}

public class AccountBalanceAccounttype
{
    public string key { get; set; }
    public string text { get; set; }
}