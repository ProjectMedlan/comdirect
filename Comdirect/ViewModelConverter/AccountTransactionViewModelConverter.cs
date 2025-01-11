using Comdirect.API;
using Comdirect.API.DataModels;
using Comdirect.ViewModels;

namespace Comdirect.ViewModelConverter;
public static class AccountTransactionViewModelConverter
{
    public static AccountTransactionViewModel ConvertToViewModel(this AccountTransaction response)
    {
        AccountTransactionViewModel viewModel = new AccountTransactionViewModel();

        viewModel.Booked = response.bookingStatus == Constants.BOOKING_STATUS_BOOKED;
        if (viewModel.Booked)
        {
            viewModel.BookingDate = ConverterHelper.ParseDate(response.bookingDate);
        }
        viewModel.Incoming = response.transactionDirection == Constants.TRANSACTION_DIRECTION_INCOMING;
        viewModel.ValutaDate = ConverterHelper.ParseDate(response.valutaDate);
        viewModel.CategoryDisplayName = response.categoryDisplayName;
        viewModel.TransactionTypeDisplayName = response.transactionTypeDisplayName;
        viewModel.Remitter = response.remitter?.holderName;
        viewModel.Creditor = response.creditor?.holderName; ;
        viewModel.TransactionValue = ConverterHelper.ParseDecimal(response.transactionValue.value);
        response.remittanceInfo?.ToList().ForEach(viewModel.RemittanceInfo.Add);

        return viewModel;
    }
}
