namespace Comdirect.API.DataModels;
public class DepotTransactionListResponse
{
    public ComdirectPaging paging { get; set; }
    public DepotTransaction[] values { get; set; }
}

public class DepotTransaction
{
    public string transactionId { get; set; }
    public string bookingStatus { get; set; }
    public string bookingDate { get; set; }
    public string businessDate { get; set; }
    public Balance quantity { get; set; }
    public string instrumentId { get; set; }
    public ComdirectInstrument instrument { get; set; }
    public Balance executionPrice { get; set; }
    public Balance transactionValue { get; set; }
    public string transactionDirection { get; set; }
    public string transactionType { get; set; }
}
