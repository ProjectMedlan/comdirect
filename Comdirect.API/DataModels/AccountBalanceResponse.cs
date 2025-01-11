namespace Comdirect.API.DataModels;

public class AccountBalanceResponse
{
    public AccountBalanceAccount account { get; set; }
    public string accountId { get; set; }
    public AccountBalanceBalance balance { get; set; }
    public AccountBalanceBalanceeur balanceEUR { get; set; }
    public AccountBalanceAvailablecashamount availableCashAmount { get; set; }
    public AccountBalanceAvailablecashamounteur availableCashAmountEUR { get; set; }
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
    public AccountBalanceCreditlimit creditLimit { get; set; }
}

public class AccountBalanceAccounttype
{
    public string key { get; set; }
    public string text { get; set; }
}

public class AccountBalanceCreditlimit
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class AccountBalanceBalance
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class AccountBalanceBalanceeur
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class AccountBalanceAvailablecashamount
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class AccountBalanceAvailablecashamounteur
{
    public string value { get; set; }
    public string unit { get; set; }
}
