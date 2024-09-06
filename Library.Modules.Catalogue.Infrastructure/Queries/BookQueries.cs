using Library.Modules.Catalogue.Application.BookSearch;
using Library.Modules.Catalogue.Application.Contracts;
using Library.Modules.Catalogue.Infrastructure.Indexes;
using Library.Modules.Catalogue.Models;
using Raven.Client.Documents.Session;

namespace Library.Modules.Catalogue.Infrastructure.Queries;

public class BookQueries : IBookQueries
{
    private readonly IDocumentStoreHolder _documentStoreHolder;
    
    public BookQueries(IDocumentStoreHolder documentStoreHolder)
    {
        _documentStoreHolder = documentStoreHolder;
    }

    public async Task<BookSearchQueryResult> GetMultiSearchResult(SearchBooksQueryParameters queryParameters)
    {
        using (var session = _documentStoreHolder.OpenAsyncSession())
        {
            var query = GetQueryFromParameters(session, queryParameters);

            var result = await query.Statistics(out var stats)
                .OfType<Book>()
                .ToListAsync();

            return new BookSearchQueryResult
            {
                Books = result,
                TotalResults = (int)stats.TotalResults
            };
        }
    }

    private IAsyncDocumentQuery<BookMultiSearch.Result> GetQueryFromParameters(IAsyncDocumentSession session, SearchBooksQueryParameters parameters)
    {
        var query = session.Advanced.AsyncDocumentQuery<BookMultiSearch.Result, BookMultiSearch>();
        var queryBuilder = BookSearchQueryBuilder.Init(query, parameters.Term);
        
        var q = parameters.SearchSource switch
        {
            BookSearchSource.Anywhere => queryBuilder.BuildSearchAnywhereQuery(parameters.SearchType),
            BookSearchSource.Author => queryBuilder.BuildSearchAuthorQuery(parameters.SearchType),
            BookSearchSource.Title => queryBuilder.BuildSearchTitleQuery(parameters.SearchType),
            BookSearchSource.Isbn => queryBuilder.BuildSearchIsbnQuery(parameters.SearchType),
            _ => throw new ArgumentOutOfRangeException($"Unrecognized {nameof(BookSearchSource)}: {parameters.SearchSource}.")
        };
        
        var skip = (parameters.PageNumber * parameters.PageSize) - parameters.PageSize;

        return q.Skip(skip)
            .Take(parameters.PageSize);
    }
}