using Comdirect.API.DataModels;
using Comdirect.ViewModels;

namespace Comdirect.ViewModelConverter;
public static class DepotPositionViewModelConverter
{
    public static DepotPositionViewModel ConvertToViewModel(this DepotPosition response)
    {
        DepotPositionViewModel viewModel = new DepotPositionViewModel();

        viewModel.DepotId = response.depotId;
        viewModel.PositionId = response.positionId;

        viewModel.Quantity = ConverterHelper.ParseDecimal(response.quantity.value);
        viewModel.AvailableQuantity = ConverterHelper.ParseDecimal(response.availableQuantity.value);
        viewModel.PurchasePrice = ConverterHelper.ParseDecimal(response.purchasePrice.value);

        viewModel.CurrentPrice = ConverterHelper.ParseDecimal(response.currentPrice.price.value);
        viewModel.CurrentPriceTimestamp = response.currentPrice.priceDateTime;
        viewModel.CurrentPriceVenue = response.currentPrice.venue.name;

        viewModel.CurrentPrice = ConverterHelper.ParseDecimal(response.prevDayPrice.price.value);
        viewModel.CurrentPriceTimestamp = response.prevDayPrice.priceDateTime;
        viewModel.CurrentPriceVenue = response.prevDayPrice.venue.name;

        viewModel.CurrentValue = ConverterHelper.ParseDecimal(response.currentValue.value);
        viewModel.PurchaseValue = ConverterHelper.ParseDecimal(response.purchaseValue.value);
        viewModel.TotalProfitOrLoss = ConverterHelper.ParseDecimal(response.profitLossPurchaseAbs.value);
        viewModel.TotalProfitOrLossPercentage = ConverterHelper.ParseDecimal(response.profitLossPurchaseRel);
        viewModel.PreviousDayProfitOrLoss = ConverterHelper.ParseDecimal(response.profitLossPrevDayAbs.value);
        viewModel.PreviousDayProfitOrLossPercentage = ConverterHelper.ParseDecimal(response.profitLossPrevDayRel);

        viewModel.Instrument = new InstrumentViewModel();
        viewModel.Instrument.InstrumentId = response.instrument.instrumentId;
        viewModel.Instrument.WKN = response.instrument.wkn;
        viewModel.Instrument.ISIN = response.instrument.isin;
        viewModel.Instrument.Mnemonic = response.instrument.mnemonic;
        viewModel.Instrument.Name = response.instrument.name;
        viewModel.Instrument.ShortName = response.instrument.shortName;
        viewModel.Instrument.InstrumentType = response.instrument.staticData.instrumentType;

        return viewModel;
    }
}

