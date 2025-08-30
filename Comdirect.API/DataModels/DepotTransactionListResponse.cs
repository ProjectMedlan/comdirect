namespace Comdirect.API.DataModels;
public class DepotTransactionListResponse
{
    public ComdirectPaging paging { get; set; } = null!;
    public DepotTransaction[] values { get; set; } = [];
}

public class DepotTransaction
{
    public string transactionId { get; set; } = null!;
    public string bookingStatus { get; set; } = null!;
    public string bookingDate { get; set; } = null!;
    public string businessDate { get; set; } = null!;
    public Balance quantity { get; set; } = null!;
    public string instrumentId { get; set; } = null!;
    public ComdirectInstrument instrument { get; set; } = null!;
    public Balance executionPrice { get; set; } = null!;
    public Balance transactionValue { get; set; } = null!;
    public string transactionDirection { get; set; } = null!;
    public string transactionType { get; set; } = null!;
}
