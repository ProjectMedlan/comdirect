namespace Comdirect.API.DataModels;

public class AccountBalanceListResponse
{
    public ComdirectPaging paging { get; set; } = null!;
    public AccountBalanceResponse[] values { get; set; } = [];
}

public class AccountBalanceResponse
{
    public AccountBalanceAccount account { get; set; } = null!;
    public string accountId { get; set; } = null!;
    public Balance balance { get; set; } = null!;
    public Balance balanceEUR { get; set; } = null!;
    public Balance availableCashAmount { get; set; } = null!;
    public Balance availableCashAmountEUR { get; set; } = null!;
}

public class AccountBalanceAccount
{
    public string accountId { get; set; } = null!;
    public string accountDisplayId { get; set; } = null!;
    public string currency { get; set; } = null!;
    public string clientId { get; set; } = null!;
    public AccountBalanceAccounttype accountType { get; set; } = null!;
    public string iban { get; set; } = null!;
    public string bic { get; set; } = null!;
    public Balance creditLimit { get; set; } = null!;
}

public class AccountBalanceAccounttype
{
    public string key { get; set; } = null!;
    public string text { get; set; } = null!;
}