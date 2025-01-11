namespace Comdirect.API.DataModels;

public class ReportResponse
{
    public ReportPaging paging { get; set; }
    public ReportAggregated aggregated { get; set; }
    public Report[] values { get; set; }
}

public class ReportPaging
{
    public int index { get; set; }
    public int matches { get; set; }
}

public class ReportAggregated
{
    public ReportBalanceeur balanceEUR { get; set; }
    public ReportAvailablecashamounteur availableCashAmountEUR { get; set; }
}

public class ReportBalanceeur
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class ReportAvailablecashamounteur
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class Report
{
    public string productId { get; set; }
    public string productType { get; set; }
    public string targetClientId { get; set; }
    public string clientConnectionType { get; set; }
    public Balance balance { get; set; }
}

public class Balance
{
    public ReportAccount account { get; set; }
    public string accountId { get; set; }
    public ReportBalance1 balance { get; set; }
    public ReportBalanceeur1 balanceEUR { get; set; }
    public ReportAvailablecashamount availableCashAmount { get; set; }
    public ReportAvailablecashamounteur1 availableCashAmountEUR { get; set; }
    public string cardId { get; set; }
    public ReportCard card { get; set; }
    public string depotId { get; set; }
    public ReportDepot depot { get; set; }
    public string dateLastUpdate { get; set; }
    public ReportPrevdayvalue prevDayValue { get; set; }
}

public class ReportAccount
{
    public string accountId { get; set; }
    public string accountDisplayId { get; set; }
    public string currency { get; set; }
    public ReportAccounttype accountType { get; set; }
    public string iban { get; set; }
    public ReportCreditlimit creditLimit { get; set; }
}

public class ReportAccounttype
{
    public string key { get; set; }
    public string text { get; set; }
}

public class ReportCreditlimit
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class ReportBalance1
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class ReportBalanceeur1
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class ReportAvailablecashamount
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class ReportAvailablecashamounteur1
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class ReportCard
{
    public string cardId { get; set; }
    public ReportCardtype cardType { get; set; }
    public string status { get; set; }
    public ReportCardlimit cardLimit { get; set; }
    public string holderName { get; set; }
    public bool nexiCard { get; set; }
    public bool replacementCardPossible { get; set; }
}

public class ReportCardtype
{
    public string key { get; set; }
    public string text { get; set; }
}

public class ReportCardlimit
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class ReportDepot
{
    public string depotId { get; set; }
    public string depotDisplayId { get; set; }
    public string clientId { get; set; }
    public string defaultSettlementAccountId { get; set; }
    public object[] settlementAccountIds { get; set; }
}

public class ReportPrevdayvalue
{
    public string value { get; set; }
    public string unit { get; set; }
}
