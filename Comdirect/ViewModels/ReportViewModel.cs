namespace Comdirect.ViewModels;
public class ReportViewModel
{
    public int AccountsCount { get; set; }
    public decimal TotalBalanceInEuro { get; set; }
    public decimal AvailableCashAmountInEuro { get; set; }
    public List<ReportDepotViewModel> Depots { get; set; } = [];
    public List<ReportCardViewModel> Cards { get; set; } = [];
    public List<ReportAccountViewModel> Accounts { get; set; } = [];
}
