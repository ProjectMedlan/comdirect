namespace Comdirect.ViewModels;
public class DepotPositionViewModel : ProfitLossViewModelBase
{
    public string? DepotId { get; set; }
    public string? PositionId { get; set; }
    public decimal Quantity { get; set; }
    public decimal AvailableQuantity { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public DateTime? CurrentPriceTimestamp { get; set; }
    public string? CurrentPriceVenue { get; set; }
    public decimal PreviousPrice { get; set; }
    public DateTime? PreviousPriceTimestamp { get; set; }
    public string? PreviousPriceVenue { get; set; }
    public InstrumentViewModel? Instrument { get; set; }
}
