namespace Comdirect.ViewModels;
public class DepotDetailsViewModel
{
    public string? DepotId { get; set; }
    public decimal PreviousDayValue { get; set; }
    public decimal CurrentValue { get; set; }
    public decimal PurchaseValue { get; set; }
    public decimal TotalProfitOrLoss { get; set; }
    public decimal TotalProfitOrLossPercentage { get; set; }
    public decimal PreviousDayProfitOrLoss { get; set; }
    public decimal PreviousDayProfitOrLossPercentage { get; set; }
}
