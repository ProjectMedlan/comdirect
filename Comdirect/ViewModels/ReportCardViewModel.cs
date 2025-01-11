namespace Comdirect.ViewModels;
public class ReportCardViewModel
{
    public string? CardId { get; set; }
    public string? CardType { get; set; }
    public string? CardStatus { get; set; }
    public string? HolderName { get; set; }
    public decimal CardLimitInEuro { get; set; }
    public decimal CardBalanceInEuro { get; set; }
    public decimal AvailableCashAmountInEuro { get; set; }
}
