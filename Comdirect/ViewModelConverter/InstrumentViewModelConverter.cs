using Comdirect.API.DataModels;
using Comdirect.ViewModels;

namespace Comdirect.ViewModelConverter;
internal static class InstrumentViewModelConverter
{
    public static InstrumentViewModel ConvertToViewModel(this ComdirectInstrument instrument)
    {
        InstrumentViewModel viewModel = new InstrumentViewModel();
        viewModel.InstrumentId = instrument.instrumentId;
        viewModel.WKN = instrument.wkn;
        viewModel.ISIN = instrument.isin;
        viewModel.Mnemonic = instrument.mnemonic;
        viewModel.Name = instrument.name;
        viewModel.ShortName = instrument.shortName;
        viewModel.InstrumentType = instrument.staticData.instrumentType;
        return viewModel;
    }
}
