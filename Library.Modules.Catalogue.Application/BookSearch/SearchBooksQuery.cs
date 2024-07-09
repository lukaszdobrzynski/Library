using Library.Modules.Catalogue.Application.Contracts;

namespace Library.Modules.Catalogue.Application.BookSearch;

public class SearchBooksQuery : IQuery<SearchBooksResultDto>
{
    public string Term { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public BookSearchType SearchType { get; set; }
    public BookSearchSource SearchSource { get; set; }
}