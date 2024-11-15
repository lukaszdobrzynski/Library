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

    private static IAsyncDocumentQuery<BookMultiSearch.Result> GetQueryFromParameters(IAsyncDocumentSession session, SearchBooksQueryParameters parameters)
    {
        var query = session.Advanced.AsyncDocumentQuery<BookMultiSearch.Result, BookMultiSearch>();
        var currentQuery = parameters.MainQuery.IsNegated
            ? query.WithNotOperator(parameters.MainQuery.SearchSource, parameters.MainQuery.SearchType, parameters.MainQuery.Term)
            : query.Untransformed(parameters.MainQuery.SearchSource, parameters.MainQuery.SearchType, parameters.MainQuery.Term);
        
        foreach (var additionalQuery in parameters.AdditionalQueries)
        {
            currentQuery = additionalQuery.Operator switch
            {
                BookSearchQueryOperator.And => 
                    additionalQuery.IsNegated ? 
                        currentQuery.WithAndAlsoNotOperator(additionalQuery.SearchSource, additionalQuery.SearchType, additionalQuery.Term) : 
                        currentQuery.WithAndAlsoOperator(additionalQuery.SearchSource, additionalQuery.SearchType, additionalQuery.Term),
                BookSearchQueryOperator.Or => 
                    additionalQuery.IsNegated ? 
                        currentQuery.WithOrElseNotOperator(additionalQuery.SearchSource, additionalQuery.SearchType, additionalQuery.Term) :
                        currentQuery.WithOrElseOperator(additionalQuery.SearchSource, additionalQuery.SearchType, additionalQuery.Term),
                _ => throw new ArgumentOutOfRangeException($"Unrecognized {nameof(BookSearchQueryOperator)}: {additionalQuery.Operator}.")
            };
        }
        
        var skip = (parameters.PageNumber * parameters.PageSize) - parameters.PageSize;

        return currentQuery.Skip(skip)
            .Take(parameters.PageSize);
    }
}