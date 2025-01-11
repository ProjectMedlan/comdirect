namespace Comdirect.ViewModels;
public class AccountTransactionViewModel
{
    public bool Incoming { get; set; }
    public bool Booked { get; set; }
    public DateOnly? BookingDate { get; set; }
    public DateOnly? ValutaDate { get; set; }
    public string? TransactionTypeDisplayName { get; set; }
    public string? CategoryDisplayName { get; set; }
    public string? Remitter { get; set; }
    public List<string> RemittanceInfo { get; set; }
    public string? Creditor { get; set; }
    public decimal TransactionValue { get; set; }
    public AccountTransactionViewModel()
    {
        RemittanceInfo = new List<string>();
    }
}
