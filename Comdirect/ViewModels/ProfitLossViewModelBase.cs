namespace Comdirect.ViewModels;

/// <summary>Gemeinsame Bewertungs- und Gewinn/Verlust-Felder für Depot-ViewModels.</summary>
public abstract class ProfitLossViewModelBase
{
    public decimal CurrentValue { get; set; }
    public decimal PurchaseValue { get; set; }
    public decimal TotalProfitOrLoss { get; set; }
    public decimal TotalProfitOrLossPercentage { get; set; }
    public decimal PreviousDayProfitOrLoss { get; set; }
    public decimal PreviousDayProfitOrLossPercentage { get; set; }
}
