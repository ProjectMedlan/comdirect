namespace Comdirect.ViewModels;
public class DepotDetailsViewModel : ProfitLossViewModelBase
{
    public string? DepotId { get; set; }
    public decimal PreviousDayValue { get; set; }
}
