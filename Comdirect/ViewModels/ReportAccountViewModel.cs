namespace Comdirect.ViewModels;
public class ReportAccountViewModel
{
    public string? AccountId { get; set; }
    public string? AccountDescription { get; set; }
    public string? IBAN { get; set; }
    public decimal CreditLimitInEuro { get; set; }
    public decimal BalanceInEuro { get; set; }
    public decimal AvailableCashAmountInEuro { get; set; }
}
