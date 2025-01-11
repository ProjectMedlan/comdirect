using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comdirect.API.DataModels;
public class DocumentListResponse
{
    public DocumentListPaging paging { get; set; }
    public DocumentListAggregated aggregated { get; set; }
    public Document[] values { get; set; }
}

public class DocumentListPaging
{
    public int index { get; set; }
    public int matches { get; set; }
}

public class DocumentListAggregated
{
    public int unreadMessages { get; set; }
    public string dateOldestEntry { get; set; }
    public int matchesInThisResponse { get; set; }
    public bool allowedToSeeAllDocuments { get; set; }
}

public class Document
{
    public string documentId { get; set; }
    public string name { get; set; }
    public string dateCreation { get; set; }
    public string mimeType { get; set; }
    public bool deletable { get; set; }
    public bool advertisement { get; set; }
    public Documentmetadata documentMetaData { get; set; }
    public int categoryId { get; set; }
}

public class Documentmetadata
{
    public bool archived { get; set; }
    public string dateRead { get; set; }
    public bool alreadyRead { get; set; }
    public bool predocumentExists { get; set; }
}
