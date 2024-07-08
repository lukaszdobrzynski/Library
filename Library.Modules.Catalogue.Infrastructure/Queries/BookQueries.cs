using Library.Modules.Catalogue.Application.BookSearch;
using Library.Modules.Catalogue.Application.Contracts;
using Library.Modules.Catalogue.Infrastructure.Indexes;
using Library.Modules.Catalogue.Models;

namespace Library.Modules.Catalogue.Infrastructure.Queries;

public class BookQueries : IBookQueries
{
    private readonly IDocumentStoreHolder _documentStoreHolder;
    
    public BookQueries(IDocumentStoreHolder documentStoreHolder)
    {
        _documentStoreHolder = documentStoreHolder;
    }

    public async Task<BookSearchQueryResult> GetSearchResults(BookSearchQueryParameters queryParameters)
    {
        var skip = (queryParameters.PageNumber * queryParameters.PageSize) - queryParameters.PageSize;
        
        using (var session = _documentStoreHolder.OpenAsyncSession())
        {
            var query = session.Advanced.AsyncDocumentQuery<BookMultiSearch.Result, BookMultiSearch>();

            if (queryParameters.SearchType == BookSearchType.ExactPhrase)
            {
                query.WhereLucene(nameof(BookMultiSearch.Result.QueryExactPhrase), $"\"{queryParameters.Term.ToLower()}\"");
            }

            if (queryParameters.SearchType == BookSearchType.AnyTerm)
            {
                query.Search(x => x.Query, queryParameters.Term);
            }

            if (queryParameters.SearchType == BookSearchType.BeginsWith)
            {
                query.WhereStartsWith(x => x.Query, queryParameters.Term);
            }
            
            var result = await query.Statistics(out var stats)
                .OfType<Book>()
                .Skip(skip)
                .Take(queryParameters.PageSize)
                .ToListAsync();

            return new BookSearchQueryResult
            {
                Books = result,
                TotalResults = (int)stats.TotalResults
            };
        }
    }
}