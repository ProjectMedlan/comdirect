using Comdirect.API.DataModels;
using Comdirect.ViewModels;

namespace Comdirect.ViewModelConverter;

public static class DepotDetailsViewModelConverter
{
    public static DepotDetailsViewModel ConvertToViewModel(this DepotPositionListAggregated response)
    {
        DepotDetailsViewModel viewModel = new DepotDetailsViewModel();

        viewModel.DepotId = response.depot.depotId;
        viewModel.PreviousDayValue = ConverterHelper.ParseDecimal(response.prevDayValue.value);
        viewModel.CurrentValue = ConverterHelper.ParseDecimal(response.currentValue.value);
        viewModel.PurchaseValue = ConverterHelper.ParseDecimal(response.purchaseValue.value);
        viewModel.TotalProfitOrLoss = ConverterHelper.ParseDecimal(response.profitLossPurchaseAbs.value);
        viewModel.TotalProfitOrLossPercentage = ConverterHelper.ParseDecimal(response.profitLossPurchaseRel);
        viewModel.PreviousDayProfitOrLoss = ConverterHelper.ParseDecimal(response.profitLossPrevDayAbs.value);
        viewModel.PreviousDayProfitOrLossPercentage = ConverterHelper.ParseDecimal(response.profitLossPrevDayRel);

        return viewModel;
    }
}

