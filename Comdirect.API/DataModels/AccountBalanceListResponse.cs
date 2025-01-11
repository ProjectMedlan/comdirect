namespace Comdirect.API.DataModels;

public class AccountBalanceListResponse
{
    public AccountBalancePaging paging { get; set; }
    public AccountBalanceResponse[] values { get; set; }
}

public class AccountBalancePaging
{
    public int index { get; set; }
    public int matches { get; set; }
}

