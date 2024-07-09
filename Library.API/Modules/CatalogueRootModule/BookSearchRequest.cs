using System.ComponentModel.DataAnnotations;
using Library.Modules.Catalogue.Application.BookSearch;

namespace Library.API.Modules.CatalogueRootModule;

public class BookSearchRequest
{
    [Required]
    public string Term { get; set; }
    
    [Required]
    public BookSearchType? SearchType { get; set; }

    [Required]
    public BookSearchSource? SearchSource { get; set; }
    
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}