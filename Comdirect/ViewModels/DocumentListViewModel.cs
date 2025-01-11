namespace Comdirect.ViewModels;
public class DocumentListViewModel
{
    public int StartIndex { get; set; }
    public int FetchCount { get; set; }
    public int TotalDocuments { get; set; }
    public int TotalUnreadDocuments { get; set; }
    public DateOnly? OldestEntryDate { get; set; }
    public List<DocumentViewModel> Documents { get; set; }

    public DocumentListViewModel()
    {
        Documents = new List<DocumentViewModel>();
    }

}
