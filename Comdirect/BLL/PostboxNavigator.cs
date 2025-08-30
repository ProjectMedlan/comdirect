using Comdirect.ViewModels;

namespace Comdirect.BLL;
/// <summary>
/// Class for navigating through the postbox (with cache);
/// </summary>
internal class PostboxNavigator
{
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int TotalCountWithFilter { get; set; }
    public int TotalPagesWithFilter { get; set; }
    public int ItemsPerPage { get; set; }
    public int CurrentPage { get; set; }
    public bool OnlyNewFilter { get; set; }

    public event Action<List<DocumentViewModel>>? OnDocumentsToViewChanged;
    public event Action<string>? CurrentProgressChanged;

    public List<DocumentViewModel> DocumentCache { get; set; }

    public PostboxNavigator(int totalCount, int itemsPerPage)
    {
        ItemsPerPage = itemsPerPage;
        CurrentPage = 1;
        OnlyNewFilter = true;
        TotalCount = totalCount;
        TotalPages = Math.Max(1, (int)Math.Ceiling((double)TotalCount / ItemsPerPage));
        TotalCountWithFilter = 0;
        TotalPagesWithFilter = 1;
        DocumentCache = new List<DocumentViewModel>();
    }

    private void PushChanges()
    {
        if (OnlyNewFilter)
        {
            var items = DocumentCache.Where(x => !x.IsRead).Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();
            OnDocumentsToViewChanged?.Invoke(items);
            CurrentProgressChanged?.Invoke($"Seite {CurrentPage}/{TotalPagesWithFilter}");
        }
        else
        {
            var items = DocumentCache.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();
            OnDocumentsToViewChanged?.Invoke(items);
            CurrentProgressChanged?.Invoke($"Seite {CurrentPage}/{TotalPages}");
        }
    }

    public void Refresh()
    {
        // Items could now be 'read' - relod
        TotalCountWithFilter = DocumentCache.Where(x => !x.IsRead).Count();
        TotalPagesWithFilter = Math.Max(1, (int)Math.Ceiling((double)TotalCountWithFilter / ItemsPerPage));
        PushChanges();
    }

    public void SetFilter(bool onlyNew)
    {
        OnlyNewFilter = onlyNew;
        CurrentPage = 1;
        PushChanges();

    }
    public void AddDocuments(List<DocumentViewModel> documents)
    {
        DocumentCache.AddRange(documents);

        // Calc filter counter
        TotalCountWithFilter = DocumentCache.Where(x => !x.IsRead).Count();
        TotalPagesWithFilter = Math.Max(1, (int)Math.Ceiling((double)TotalCountWithFilter / ItemsPerPage));
    }

    public void NextPage()
    {
        int maxPage = OnlyNewFilter ? TotalCountWithFilter : TotalCount;

        if (CurrentPage < maxPage)
        {
            CurrentPage++;
            PushChanges();
        }
    }

    public void PreviousPage()
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
            PushChanges();
        }
    }

    public bool HasDataForNextPage()
    {
        if (OnlyNewFilter)
        {
            // Last page?
            if (CurrentPage == TotalPagesWithFilter)
                return true;

            return DocumentCache.Where(x => !x.IsRead).Count() > CurrentPage * ItemsPerPage;
        }
        else
        {
            return DocumentCache.Count > CurrentPage * ItemsPerPage;
        }
    }
}
