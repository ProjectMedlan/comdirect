namespace Comdirect.API.DataModels;

public class ReportResponse
{
    public ComdirectPaging paging { get; set; }
    public ReportAggregated aggregated { get; set; }
    public Report[] values { get; set; }
}

public class ReportAggregated
{
    public Balance balanceEUR { get; set; }
    public Balance availableCashAmountEUR { get; set; }
}

public class Report
{
    public string productId { get; set; }
    public string productType { get; set; }
    public string targetClientId { get; set; }
    public string clientConnectionType { get; set; }
    public ReportBalance balance { get; set; }
}

public class ReportBalance
{
    public ReportAccount account { get; set; }
    public string accountId { get; set; }
    public Balance balance { get; set; }
    public Balance balanceEUR { get; set; }
    public Balance availableCashAmount { get; set; }
    public Balance availableCashAmountEUR { get; set; }
    public string cardId { get; set; }
    public ReportCard card { get; set; }
    public string depotId { get; set; }
    public ReportDepot depot { get; set; }
    public string dateLastUpdate { get; set; }
    public Balance prevDayValue { get; set; }
}

public class ReportAccount
{
    public string accountId { get; set; }
    public string accountDisplayId { get; set; }
    public string currency { get; set; }
    public ReportAccounttype accountType { get; set; }
    public string iban { get; set; }
    public Balance creditLimit { get; set; }
}

public class ReportAccounttype
{
    public string key { get; set; }
    public string text { get; set; }
}

public class ReportCard
{
    public string cardId { get; set; }
    public ReportCardtype cardType { get; set; }
    public string status { get; set; }
    public Balance cardLimit { get; set; }
    public string holderName { get; set; }
    public bool nexiCard { get; set; }
    public bool replacementCardPossible { get; set; }
}

public class ReportCardtype
{
    public string key { get; set; }
    public string text { get; set; }
}

public class ReportDepot
{
    public string depotId { get; set; }
    public string depotDisplayId { get; set; }
    public string clientId { get; set; }
    public string defaultSettlementAccountId { get; set; }
    public object[] settlementAccountIds { get; set; }
}

