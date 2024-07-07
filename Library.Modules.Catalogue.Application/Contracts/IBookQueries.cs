using Library.Modules.Catalogue.Models;

namespace Library.Modules.Catalogue.Application.Contracts;

public interface IBookQueries
{
    Task<BookMultiSearchQueryResult> GetMultiSearchResults(string term, int skip, int take);
}