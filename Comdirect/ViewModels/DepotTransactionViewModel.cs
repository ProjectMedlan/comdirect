namespace Comdirect.ViewModels;
public class DepotTransactionViewModel
{
    public bool Booked { get; set; }
    public DateOnly? BookingDate { get; set; }
    public DateOnly? BusinessDate { get; set; }
    public InstrumentViewModel? Instrument { get; set; }
    public decimal Quantity { get; set; }
    public string? QuantityUnit { get; set; }
    public decimal ExecutionPrice { get; set; }
    public decimal TransactionValue { get; set; }
    public bool Incoming { get; set; }
    public string? TransactionType { get; set; }

}
