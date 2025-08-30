namespace Comdirect.API.DataModels;

public class ReportResponse
{
    public ComdirectPaging paging { get; set; } = null!;
    public ReportAggregated aggregated { get; set; } = null!;
    public Report[] values { get; set; } = [];
}

public class ReportAggregated
{
    public Balance balanceEUR { get; set; } = null!;
    public Balance availableCashAmountEUR { get; set; } = null!;
}

public class Report
{
    public string productId { get; set; } = null!;
    public string productType { get; set; } = null!;
    public string targetClientId { get; set; } = null!;
    public string clientConnectionType { get; set; } = null!;
    public ReportBalance balance { get; set; } = null!;
}

public class ReportBalance
{
    public ReportAccount account { get; set; } = null!;
    public string accountId { get; set; } = null!;
    public Balance balance { get; set; } = null!;
    public Balance balanceEUR { get; set; } = null!;
    public Balance availableCashAmount { get; set; } = null!;
    public Balance availableCashAmountEUR { get; set; } = null!;
    public string cardId { get; set; } = null!;
    public ReportCard card { get; set; } = null!;
    public string depotId { get; set; } = null!;
    public ReportDepot depot { get; set; } = null!;
    public string dateLastUpdate { get; set; } = null!;
    public Balance prevDayValue { get; set; } = null!;
}

public class ReportAccount
{
    public string accountId { get; set; } = null!;
    public string accountDisplayId { get; set; } = null!;
    public string currency { get; set; } = null!;
    public ReportAccounttype accountType { get; set; } = null!;
    public string iban { get; set; } = null!;
    public Balance creditLimit { get; set; } = null!;
}

public class ReportAccounttype
{
    public string key { get; set; } = null!;
    public string text { get; set; } = null!;
}

public class ReportCard
{
    public string cardId { get; set; } = null!;
    public ReportCardtype cardType { get; set; } = null!;
    public string status { get; set; } = null!;
    public Balance cardLimit { get; set; } = null!;
    public string holderName { get; set; } = null!;
    public bool nexiCard { get; set; }
    public bool replacementCardPossible { get; set; }
}

public class ReportCardtype
{
    public string key { get; set; } = null!;
    public string text { get; set; } = null!;
}

public class ReportDepot
{
    public string depotId { get; set; } = null!;
    public string depotDisplayId { get; set; } = null!;
    public string clientId { get; set; } = null!;
    public string defaultSettlementAccountId { get; set; } = null!;
    public object[] settlementAccountIds { get; set; } = [];
}

