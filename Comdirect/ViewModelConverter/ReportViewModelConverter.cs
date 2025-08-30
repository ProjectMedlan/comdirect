using Comdirect.API;
using Comdirect.API.DataModels;
using Comdirect.ViewModels;

namespace Comdirect.ViewModelConverter;
public static class ReportViewModelConverter
{
    public static ReportViewModel ConvertToViewModel(this ReportResponse response)
    {
        ReportViewModel viewModel = new ReportViewModel();

        viewModel.AccountsCount = response.paging.matches;
        viewModel.TotalBalanceInEuro = ConverterHelper.ParseDecimal(response.aggregated.balanceEUR.value);
        viewModel.AvailableCashAmountInEuro = ConverterHelper.ParseDecimal(response.aggregated.availableCashAmountEUR.value);

        foreach (var item in response.values)
        {
            if (item.productType == Constants.PRODUCT_TYPE_DEPOT)
            {
                viewModel.Depots.Add(item.ConvertToDepotViewModel());
            }
            else if (item.productType == Constants.PRODUCT_TYPE_CARD)
            {
                viewModel.Cards.Add(item.ConvertToCardViewModel());
            }
            else if (item.productType == Constants.PRODUCT_TYPE_ACCOUNT)
            {
                viewModel.Accounts.Add(item.ConvertToAccountViewModel());
            }
        }

        return viewModel;
    }

    private static ReportDepotViewModel ConvertToDepotViewModel(this Report report)
    {
        ReportDepotViewModel viewModel = new ReportDepotViewModel();
        viewModel.DepotId = report.balance.depotId;
        viewModel.LastUpdate = ConverterHelper.ParseDate(report.balance.dateLastUpdate);
        viewModel.PreviousDayValue = ConverterHelper.ParseDecimal(report.balance.prevDayValue.value);
        return viewModel;
    }

    private static ReportCardViewModel ConvertToCardViewModel(this Report report)
    {
        ReportCardViewModel viewModel = new ReportCardViewModel();
        viewModel.CardId = report.balance.cardId;
        viewModel.CardType = report.balance.card.cardType.text;
        viewModel.CardStatus = Constants.CARD_STATUS[report.balance.card.status];
        viewModel.HolderName = report.balance.card.holderName;
        viewModel.CardLimitInEuro = ConverterHelper.ParseDecimal(report.balance.card.cardLimit.value);
        viewModel.CardBalanceInEuro = ConverterHelper.ParseDecimal(report.balance.balance.value);
        viewModel.AvailableCashAmountInEuro = ConverterHelper.ParseDecimal(report.balance.availableCashAmount.value);
        return viewModel;
    }

    private static ReportAccountViewModel ConvertToAccountViewModel(this Report report)
    {
        ReportAccountViewModel viewModel = new ReportAccountViewModel();
        viewModel.AccountId = report.balance.accountId;
        viewModel.AccountDescription = Constants.ACCOUNT_TYPES[report.balance.account.accountType.key];
        viewModel.IBAN = report.balance.account.iban;
        viewModel.CreditLimitInEuro = ConverterHelper.ParseDecimal(report.balance.account.creditLimit.value);
        viewModel.BalanceInEuro = ConverterHelper.ParseDecimal(report.balance.balanceEUR.value);
        viewModel.AvailableCashAmountInEuro = ConverterHelper.ParseDecimal(report.balance.availableCashAmountEUR.value);
        return viewModel;
    }
}
