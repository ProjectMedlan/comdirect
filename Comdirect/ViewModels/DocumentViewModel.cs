
namespace Comdirect.ViewModels;
public class DocumentViewModel
{
    public string? DocumentId { get; set; }
    public string? Name { get; set; }
    public DateOnly? CreationDate { get; set; }
    public string? MimeType { get; set; }
    public bool IsRead { get; set; }
    public DateOnly? ReadDate { get; set; }
    public  bool IsArchived { get; set; }
    public bool IsAdvertisment { get; set; }
    public int CategoryID { get; set; }
    public string? CategoryName { get; set; }

}
