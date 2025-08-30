using Comdirect.API;
using Comdirect.API.DataModels;
using Comdirect.ViewModels;

namespace Comdirect.ViewModelConverter;
public static class DocumentListViewModelConverter
{
    public static DocumentListViewModel ConvertToViewModel(this DocumentListResponse response)
    {
        DocumentListViewModel viewModel = new DocumentListViewModel();

        // For future pagination
        viewModel.StartIndex = response.paging.index ?? 0;
        viewModel.FetchCount = response.aggregated.matchesInThisResponse;

        viewModel.TotalDocuments = response.paging.matches;
        viewModel.TotalUnreadDocuments = response.aggregated.unreadMessages;
        viewModel.OldestEntryDate = ConverterHelper.ParseDate(response.aggregated.dateOldestEntry);

        foreach (var item in response.values)
        {
            viewModel.Documents.Add(item.ConvertToDocumentViewModel());
        }

        return viewModel;
    }

    private static DocumentViewModel ConvertToDocumentViewModel(this Document document)
    {
        DocumentViewModel viewModel = new DocumentViewModel();
        
        viewModel.DocumentId = document.documentId;
        viewModel.Name = document.name;
        viewModel.CreationDate = ConverterHelper.ParseDate(document.dateCreation);
        viewModel.MimeType = document.mimeType;
        viewModel.IsAdvertisment = document.advertisement;
        viewModel.IsArchived = document.documentMetaData.archived;
        viewModel.IsRead = document.documentMetaData.alreadyRead;
        viewModel.ReadDate = ConverterHelper.ParseDate(document.documentMetaData.dateRead);
        viewModel.CategoryID = document.categoryId;
        if (Constants.DOCUMENT_CATEGORIES.ContainsKey(document.categoryId))
            viewModel.CategoryName = Constants.DOCUMENT_CATEGORIES[document.categoryId];
        else
            viewModel.CategoryName = $"Unbekannte Kategorie ({document.categoryId})";

        return viewModel;
    }
}
