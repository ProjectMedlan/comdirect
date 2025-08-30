namespace Comdirect.API.DataModels;
public class DepotResponseList
{
    public ComdirectPaging paging { get; set; } = null!;
    public DepotResponse[] values { get; set; } = [];
}

public class DepotResponse
{
    public string depotId { get; set; } = null!;
    public string depotDisplayId { get; set; } = null!;
    public string clientId { get; set; } = null!;
    public string depotType { get; set; } = null!;
    public string defaultSettlementAccountId { get; set; } = null!;
    public object[] settlementAccountIds { get; set; } = [];
    public string targetMarket { get; set; } = null!;
}
