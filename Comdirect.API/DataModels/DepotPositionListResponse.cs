namespace Comdirect.API.DataModels;

public class DepotPositionListResponse
{
    public ComdirectPaging paging { get; set; }
    public DepotPositionListAggregated aggregated { get; set; }
    public DepotPosition[] values { get; set; }
}

public class DepotPositionListAggregated
{
    public DepotPositionListDepot depot { get; set; }
    public Balance prevDayValue { get; set; }
    public Balance currentValue { get; set; }
    public Balance purchaseValue { get; set; }
    public Balance profitLossPurchaseAbs { get; set; }
    public string profitLossPurchaseRel { get; set; }
    public Balance profitLossPrevDayAbs { get; set; }
    public string profitLossPrevDayRel { get; set; }
    public bool purchaseValuesAlterable { get; set; }
}

public class DepotPositionListDepot
{
    public string depotId { get; set; }
    public string depotDisplayId { get; set; }
    public string clientId { get; set; }
    public string depotType { get; set; }
    public string defaultSettlementAccountId { get; set; }
    public object[] settlementAccountIds { get; set; }
    public string targetMarket { get; set; }
}

public class DepotPosition
{
    public string depotId { get; set; }
    public string positionId { get; set; }
    public string wkn { get; set; }
    public string custodyType { get; set; }
    public Balance quantity { get; set; }
    public Balance availableQuantity { get; set; }
    public DepotPositionPrice currentPrice { get; set; }
    public Balance purchasePrice { get; set; }
    public DepotPositionPrice prevDayPrice { get; set; }
    public Balance currentValue { get; set; }
    public Balance purchaseValue { get; set; }
    public Balance profitLossPurchaseAbs { get; set; }
    public string profitLossPurchaseRel { get; set; }
    public Balance profitLossPrevDayAbs { get; set; }
    public string profitLossPrevDayRel { get; set; }
    public Balance profitLossPrevDayTotalAbs { get; set; }
    public ComdirectInstrument instrument { get; set; }
    public object version { get; set; }
    public string hedgeability { get; set; }
    public Balance availableQuantityToHedge { get; set; }
    public bool currentPriceDeterminable { get; set; }
    public bool hasIntraDayExecutedOrder { get; set; }
}

public class DepotPositionPrice
{
    public Balance price { get; set; }
    public DateTime priceDateTime { get; set; }
    public DepotPositionListVenue? venue { get; set; }
}

public class DepotPositionListVenue
{
    public string name { get; set; }
    public string venueId { get; set; }
    public string country { get; set; }
    public string type { get; set; }
}