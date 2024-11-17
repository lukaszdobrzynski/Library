using Library.Modules.Catalogue.Application.BookSearch;
using Library.Modules.Catalogue.Models;

namespace Library.Modules.Catalogue.Application.Contracts;

public interface IBookQueries
{
    Task<BookSearchQueryResult> GetMultiSearchResult(BookSearchQueryParameters queryParameters);
}