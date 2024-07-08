namespace Library.API.Modules.CatalogueRootModule;

public class BookSearchRequest
{
    public string Term { get; set; }
    public string SearchType { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}