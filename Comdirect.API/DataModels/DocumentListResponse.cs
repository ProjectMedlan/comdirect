namespace Comdirect.API.DataModels;
public class DocumentListResponse
{
    public ComdirectPaging paging { get; set; } = null!;
    public DocumentListAggregated aggregated { get; set; } = null!;
    public Document[] values { get; set; } = []!;
}

public class DocumentListAggregated
{
    public int unreadMessages { get; set; }
    public string dateOldestEntry { get; set; } = null!;
    public int matchesInThisResponse { get; set; }
    public bool allowedToSeeAllDocuments { get; set; }
}

public class Document
{
    public string documentId { get; set; } = null!;
    public string name { get; set; } = null!;
    public string dateCreation { get; set; } = null!;
    public string mimeType { get; set; } = null!;
    public bool deletable { get; set; }
    public bool advertisement { get; set; }
    public Documentmetadata documentMetaData { get; set; } = null!;
    public int categoryId { get; set; }
}

public class Documentmetadata
{
    public bool archived { get; set; }
    public string dateRead { get; set; } = null!;
    public bool alreadyRead { get; set; }
    public bool predocumentExists { get; set; }
}
