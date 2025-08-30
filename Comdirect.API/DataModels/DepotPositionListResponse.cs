namespace Comdirect.API.DataModels;

public class DepotPositionListResponse
{
    public ComdirectPaging paging { get; set; } = null!;
    public DepotPositionListAggregated aggregated { get; set; } = null!;
    public DepotPosition[] values { get; set; } = [];
}

public class DepotPositionListAggregated
{
    public DepotPositionListDepot depot { get; set; } = null!;
    public Balance prevDayValue { get; set; } = null!;
    public Balance currentValue { get; set; } = null!;
    public Balance purchaseValue { get; set; } = null!;
    public Balance profitLossPurchaseAbs { get; set; } = null!;
    public string profitLossPurchaseRel { get; set; } = null!;
    public Balance profitLossPrevDayAbs { get; set; } = null!;
    public string profitLossPrevDayRel { get; set; } = null!;
    public bool purchaseValuesAlterable { get; set; }
}

public class DepotPositionListDepot
{
    public string depotId { get; set; } = null!;
    public string depotDisplayId { get; set; } = null!;
    public string clientId { get; set; } = null!;
    public string depotType { get; set; } = null!;
    public string defaultSettlementAccountId { get; set; } = null!;
    public object[] settlementAccountIds { get; set; } = [];
    public string targetMarket { get; set; } = null!;
}

public class DepotPosition
{
    public string depotId { get; set; } = null!;
    public string positionId { get; set; } = null!;
    public string wkn { get; set; } = null!;
    public string custodyType { get; set; } = null!;
    public Balance quantity { get; set; } = null!;
    public Balance availableQuantity { get; set; } = null!;
    public DepotPositionPrice currentPrice { get; set; } = null!;
    public Balance purchasePrice { get; set; } = null!;
    public DepotPositionPrice prevDayPrice { get; set; } = null!;
    public Balance currentValue { get; set; } = null!;
    public Balance purchaseValue { get; set; } = null!;
    public Balance profitLossPurchaseAbs { get; set; } = null!;
    public string profitLossPurchaseRel { get; set; } = null!;
    public Balance profitLossPrevDayAbs { get; set; } = null!;
    public string profitLossPrevDayRel { get; set; } = null!;
    public Balance profitLossPrevDayTotalAbs { get; set; } = null!;
    public ComdirectInstrument instrument { get; set; } = null!;
    public object version { get; set; } = null!;
    public string hedgeability { get; set; } = null!;
    public Balance availableQuantityToHedge { get; set; } = null!;
    public bool currentPriceDeterminable { get; set; }
    public bool hasIntraDayExecutedOrder { get; set; }
}

public class DepotPositionPrice
{
    public Balance price { get; set; } = null!;
    public DateTime? priceDateTime { get; set; }
    public DepotPositionListVenue? venue { get; set; }
}

public class DepotPositionListVenue
{
    public string name { get; set; } = null!;
    public string venueId { get; set; } = null!;
    public string country { get; set; } = null!;
    public string type { get; set; } = null!;
}