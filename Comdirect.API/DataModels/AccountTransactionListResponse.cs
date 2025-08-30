namespace Comdirect.API.DataModels;

public class AccountTransactionListResponse
{
    public ComdirectPaging paging { get; set; } = null!;
    public AccountTransactionAggregated aggregated { get; set; } = null!;
    public AccountTransaction[] values { get; set; } = [];
}

public class AccountTransactionAggregated
{
    public object account { get; set; } = null!;
    public string accountId { get; set; } = null!;
    public string bookingDateEarliestTransaction { get; set; } = null!;
    public string referenceEarliestTransaction { get; set; } = null!;
    public bool earliestTransactionIncluded { get; set; }
    public DateTime pagingTimestamp { get; set; }
    public AccountTransactionAccounttransactiontype[] accountTransactionTypes { get; set; } = [];
}

public class AccountTransactionAccounttransactiontype
{
    public string transactionType { get; set; } = null!;
    public string transactionTypeDisplayName { get; set; } = null!;
}

public class AccountTransaction
{
    public string reference { get; set; } = null!;
    public string bookingStatus { get; set; } = null!;
    public string bookingDate { get; set; } = null!;
    public Balance transactionValue { get; set; } = null!;
    public AccountTransactionRemitter remitter { get; set; } = null!;
    public object debtor { get; set; } = null!;
    public AccountTransactionCreditor creditor { get; set; } = null!;
    public string valutaDate { get; set; } = null!;
    public object directDebitCreditorId { get; set; } = null!;
    public object directDebitMandateId { get; set; } = null!;
    public string endToEndReference { get; set; } = null!;
    public bool newTransaction { get; set; }
    public string[] remittanceInfo { get; set; } = [];
    public string transactionType { get; set; } = null!;
    public string transactionTypeDisplayName { get; set; } = null!;
    public string categoryDisplayName { get; set; } = null!;
    public string transactionDirection { get; set; } = null!;
}

public class AccountTransactionRemitter
{
    public string holderName { get; set; } = null!;
}

public class AccountTransactionCreditor
{
    public string holderName { get; set; } = null!;
    public string iban { get; set; } = null!;
    public string bic { get; set; } = null!;
}