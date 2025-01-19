namespace Comdirect.API.DataModels;
public class DepotTransactionListResponse
{
    public DepotTransactionPaging paging { get; set; }
    public DepotTransaction[] values { get; set; }
}

public class DepotTransactionPaging
{
    public int index { get; set; }
    public int matches { get; set; }
}

public class DepotTransaction
{
    public string transactionId { get; set; }
    public string bookingStatus { get; set; }
    public string bookingDate { get; set; }
    public string businessDate { get; set; }
    public DepotTransactionQuantity quantity { get; set; }
    public string instrumentId { get; set; }
    public ComdirectInstrument instrument { get; set; }
    public DepotTransactionExecutionprice executionPrice { get; set; }
    public DepotTransactionTransactionvalue transactionValue { get; set; }
    public string transactionDirection { get; set; }
    public string transactionType { get; set; }
}

public class DepotTransactionQuantity
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class DepotTransactionExecutionprice
{
    public string value { get; set; }
    public string unit { get; set; }
}

public class DepotTransactionTransactionvalue
{
    public string value { get; set; }
    public string unit { get; set; }
}
