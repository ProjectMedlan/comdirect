﻿namespace Comdirect.API.DataModels;
public class DepotResponseList
{
    public ComdirectPaging paging { get; set; }
    public DepotResponse[] values { get; set; }
}

public class DepotResponse
{
    public string depotId { get; set; }
    public string depotDisplayId { get; set; }
    public string clientId { get; set; }
    public string depotType { get; set; }
    public string defaultSettlementAccountId { get; set; }
    public object[] settlementAccountIds { get; set; }
    public string targetMarket { get; set; }
}
