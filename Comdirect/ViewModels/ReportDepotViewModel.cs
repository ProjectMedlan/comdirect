namespace Comdirect.ViewModels;
public class ReportDepotViewModel
{
    public string? DepotId { get; set; }
    public DateOnly? LastUpdate { get; set; }
    public decimal PreviousDayValue { get; set; }

    public ReportDepotViewModel()
    {
        DepotId = string.Empty;
        LastUpdate = null;
    }
}
