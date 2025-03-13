namespace Comdirect.API.DataModels;

public class AccountTransactionListResponse
{
    public ComdirectPaging paging { get; set; }
    public AccountTransactionAggregated aggregated { get; set; }
    public AccountTransaction[] values { get; set; }
}

public class AccountTransactionAggregated
{
    public object account { get; set; }
    public string accountId { get; set; }
    public string bookingDateEarliestTransaction { get; set; }
    public string referenceEarliestTransaction { get; set; }
    public bool earliestTransactionIncluded { get; set; }
    public DateTime pagingTimestamp { get; set; }
    public AccountTransactionAccounttransactiontype[] accountTransactionTypes { get; set; }
}

public class AccountTransactionAccounttransactiontype
{
    public string transactionType { get; set; }
    public string transactionTypeDisplayName { get; set; }
}

public class AccountTransaction
{
    public string reference { get; set; }
    public string bookingStatus { get; set; }
    public string bookingDate { get; set; }
    public Balance transactionValue { get; set; }
    public AccountTransactionRemitter remitter { get; set; }
    public object debtor { get; set; }
    public AccountTransactionCreditor creditor { get; set; }
    public string valutaDate { get; set; }
    public object directDebitCreditorId { get; set; }
    public object directDebitMandateId { get; set; }
    public string endToEndReference { get; set; }
    public bool newTransaction { get; set; }
    public string[] remittanceInfo { get; set; }
    public string transactionType { get; set; }
    public string transactionTypeDisplayName { get; set; }
    public string categoryDisplayName { get; set; }
    public string transactionDirection { get; set; }
}

public class AccountTransactionRemitter
{
    public string holderName { get; set; }
}

public class AccountTransactionCreditor
{
    public string holderName { get; set; }
    public string iban { get; set; }
    public string bic { get; set; }
}