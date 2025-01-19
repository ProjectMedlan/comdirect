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
    public DepotPositionListPrevdayvalue prevDayValue { get; set; }
    public DepotPositionListCurrentvalue currentValue { get; set; }
    public DepotPositionListPurchasevalue purchaseValue { get; set; }
    public DepotPositionListProfitlosspurchaseabs profitLossPurchaseAbs { get; set; }
    public string profitLossPurchaseRel { get; set; }
    public DepotPositionListProfitlossprevdayabs profitLossPrevDayAbs { get; set; }
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

public class DepotPositionListPrevdayvalue
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class DepotPositionListCurrentvalue
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class DepotPositionListPurchasevalue
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class DepotPositionListProfitlosspurchaseabs
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class DepotPositionListProfitlossprevdayabs
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class DepotPosition
{
    public string depotId { get; set; }
    public string positionId { get; set; }
    public string wkn { get; set; }
    public string custodyType { get; set; }
    public DepotPositionListQuantity quantity { get; set; }
    public DepotPositionListAvailablequantity availableQuantity { get; set; }
    public DepotPositionListCurrentprice currentPrice { get; set; }
    public DepotPositionListPurchaseprice purchasePrice { get; set; }
    public DepotPositionListPrevdayprice prevDayPrice { get; set; }
    public DepotPositionListCurrentvalue1 currentValue { get; set; }
    public DepotPositionListPurchasevalue1 purchaseValue { get; set; }
    public DepotPositionListProfitlosspurchaseabs1 profitLossPurchaseAbs { get; set; }
    public string profitLossPurchaseRel { get; set; }
    public DepotPositionListProfitlossprevdayabs1 profitLossPrevDayAbs { get; set; }
    public string profitLossPrevDayRel { get; set; }
    public DepotPositionListProfitlossprevdaytotalabs profitLossPrevDayTotalAbs { get; set; }
    public ComdirectInstrument instrument { get; set; }
    public object version { get; set; }
    public string hedgeability { get; set; }
    public DepotPositionListAvailablequantitytohedge availableQuantityToHedge { get; set; }
    public bool currentPriceDeterminable { get; set; }
    public bool hasIntraDayExecutedOrder { get; set; }
}

public class DepotPositionListQuantity
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class DepotPositionListAvailablequantity
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class DepotPositionListCurrentprice
{
    public DepotPositionListPrice price { get; set; }
    public DateTime priceDateTime { get; set; }
    public DepotPositionListVenue? venue { get; set; }
}

public class DepotPositionListPrice
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class DepotPositionListVenue
{
    public string name { get; set; }
    public string venueId { get; set; }
    public string country { get; set; }
    public string type { get; set; }
}

public class DepotPositionListPurchaseprice
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class DepotPositionListPrevdayprice
{
    public DepotPositionListPrice price { get; set; }
    public DateTime priceDateTime { get; set; }
    public DepotPositionListVenue? venue { get; set; }
}

public class DepotPositionListCurrentvalue1
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class DepotPositionListPurchasevalue1
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class DepotPositionListProfitlosspurchaseabs1
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class DepotPositionListProfitlossprevdayabs1
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class DepotPositionListProfitlossprevdaytotalabs
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class DepotPositionListAvailablequantitytohedge
{
    public string value { get; set; }
    public string unit { get; set; }
}
