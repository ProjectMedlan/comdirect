using Comdirect.API;
using Comdirect.API.DataModels;
using Comdirect.ViewModels;

namespace Comdirect.ViewModelConverter;
public static class DepotTransactionViewModelConverter
{
    public static DepotTransactionViewModel ConvertToViewModel(this DepotTransaction response)
    {
        DepotTransactionViewModel viewModel = new DepotTransactionViewModel();

        viewModel.Booked = response.bookingStatus == Constants.BOOKING_STATUS_BOOKED;
        viewModel.BookingDate = ConverterHelper.ParseDate(response.bookingDate);
        viewModel.BusinessDate = ConverterHelper.ParseDate(response.businessDate);
        viewModel.Incoming = response.transactionDirection == Constants.TRANSACTION_DIRECTION_INCOMING;
        viewModel.Quantity = ConverterHelper.ParseDecimal(response.quantity.value);
        viewModel.QuantityUnit = response.quantity.unit;
        viewModel.ExecutionPrice = ConverterHelper.ParseDecimal(response.executionPrice.value);
        viewModel.TransactionValue = ConverterHelper.ParseDecimal(response.transactionValue.value);
        viewModel.TransactionType = response.transactionType;
        viewModel.Instrument = InstrumentViewModelConverter.ConvertToViewModel(response.instrument);
        return viewModel;
    }
}

   