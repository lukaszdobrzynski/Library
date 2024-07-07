using Library.Modules.Catalogue.Application.Contracts;

namespace Library.Modules.Catalogue.Application.BookSearch;

public class SearchAllBooksQuery : IQuery<SearchAllBooksResultDto>
{
    public string Term { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }
}