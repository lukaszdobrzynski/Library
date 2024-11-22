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

    public async Task<BookSearchQueryResult> GetMultiSearchResult(BookSearchQueryParameters queryParameters)
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

    private static IAsyncDocumentQuery<BookMultiSearch.Result> GetQueryFromParameters(IAsyncDocumentSession session, BookSearchQueryParameters parameters)
    {
        var query = session.Advanced.AsyncDocumentQuery<BookMultiSearch.Result, BookMultiSearch>();
        var currentQuery = ApplyMainQuery(query, parameters.MainQuery);

        foreach (var additionalQuery in parameters.AdditionalQueries)
        {
            currentQuery = additionalQuery switch
            {
                BookSearchTextAdditionalQueryParameters textQuery => ApplyTextAdditionalQuery(currentQuery, textQuery),
                BookSearchDateRangeAdditionalQueryParameters dateRangeQuery => ApplyDateRangeAdditionalQuery(currentQuery, dateRangeQuery),
                BookSearchDateSequenceAdditionalQueryParameters dateSequenceQuery => ApplyDateSequenceAdditionalQuery(currentQuery, dateSequenceQuery),
                _ => throw new ArgumentException($"Unsupported query type: {additionalQuery.GetType().Name}")
            };
        }
        
        var skip = (parameters.PageNumber * parameters.PageSize) - parameters.PageSize;

        return currentQuery.Skip(skip)
            .Take(parameters.PageSize);
    }

    private static IAsyncDocumentQuery<BookMultiSearch.Result> ApplyMainQuery(
        IAsyncDocumentQuery<BookMultiSearch.Result> currentQuery, BookSearchMainQueryParameters parameters)
    {
        return parameters switch
        {
            BookSearchTextMainQueryParameters textQueryParameters => ApplyMainTextQuery(currentQuery, textQueryParameters),
            BookSearchDateRangeMainQueryParameters dateRangeQueryParameters => ApplyMainDateRangeQuery(currentQuery, dateRangeQueryParameters),
            BookSearchDateSequenceMainQueryParameters dateSequenceQueryParameters => ApplyMainDateSequenceQuery(currentQuery, dateSequenceQueryParameters),
            _ => throw new ArgumentException($"Unsupported query type: {parameters.GetType().Name}")
        };
    }
    
    private static IAsyncDocumentQuery<BookMultiSearch.Result> ApplyMainTextQuery(IAsyncDocumentQuery<BookMultiSearch.Result> query, BookSearchTextMainQueryParameters parameters)
    {
        return parameters.IsNegated
            ? query.TextWithNotOperator(parameters.SearchSource, parameters.SearchType, parameters.Term)
            : query.TextUntransformed(parameters.SearchSource, parameters.SearchType, parameters.Term);
    }

    private static IAsyncDocumentQuery<BookMultiSearch.Result> ApplyMainDateRangeQuery(IAsyncDocumentQuery<BookMultiSearch.Result> query, BookSearchDateRangeMainQueryParameters parameters)
    {
        return parameters.IsNegated
            ? query.WithNotDateRangeOperator(parameters.SearchSource, parameters.FromDate, parameters.ToDate)
            : query.WithDateRangeOperator(parameters.SearchSource, parameters.FromDate, parameters.ToDate);
    }

    private static IAsyncDocumentQuery<BookMultiSearch.Result> ApplyMainDateSequenceQuery(IAsyncDocumentQuery<BookMultiSearch.Result> query, BookSearchDateSequenceMainQueryParameters parameters)
    {
        return parameters.IsNegated
            ? query.WithNotDateSequenceOperator(parameters.SearchSource, parameters.SequenceOperator,parameters.Date)
            : query.WithDateSequenceOperator(parameters.SearchSource, parameters.SequenceOperator,parameters.Date);
    }

    
    private static IAsyncDocumentQuery<BookMultiSearch.Result> ApplyTextAdditionalQuery(IAsyncDocumentQuery<BookMultiSearch.Result> currentQuery, BookSearchTextAdditionalQueryParameters textQuery)
    {
        return textQuery.QueryOperator switch
        {
            BookSearchConsecutiveQueryOperator.And =>
                currentQuery.WithAndAlsoOperator(textQuery.SearchSource, textQuery.SearchType, textQuery.Term),
            BookSearchConsecutiveQueryOperator.AndNot =>
                currentQuery.WithAndAlsoNotOperator(textQuery.SearchSource, textQuery.SearchType, textQuery.Term),
            BookSearchConsecutiveQueryOperator.Or =>
              currentQuery.WithOrElseOperator(textQuery.SearchSource, textQuery.SearchType, textQuery.Term),
            BookSearchConsecutiveQueryOperator.OrNot =>
                currentQuery.WithOrElseNotOperator(textQuery.SearchSource, textQuery.SearchType, textQuery.Term),
            _ => throw new ArgumentOutOfRangeException($"Unrecognized {nameof(BookSearchConsecutiveQueryOperator)}: {textQuery.QueryOperator}.")
        };
    }
    
    private static IAsyncDocumentQuery<BookMultiSearch.Result> ApplyDateRangeAdditionalQuery(IAsyncDocumentQuery<BookMultiSearch.Result> currentQuery, BookSearchDateRangeAdditionalQueryParameters dateQuery)
    {
        return dateQuery.QueryOperator switch
        {
            BookSearchConsecutiveQueryOperator.And => 
               currentQuery.WithAndAlsoDateRangeOperator(dateQuery.SearchSource, dateQuery.FromDate, dateQuery.ToDate),
            BookSearchConsecutiveQueryOperator.AndNot =>
                currentQuery.WithAndAlsoNotDateRangeOperator(dateQuery.SearchSource, dateQuery.FromDate, dateQuery.ToDate),
            BookSearchConsecutiveQueryOperator.Or =>
                currentQuery.WithOrElseDateRangeOperator(dateQuery.SearchSource, dateQuery.FromDate, dateQuery.ToDate),
            BookSearchConsecutiveQueryOperator.OrNot =>
                currentQuery.WithOrElseNotDateRangeOperator(dateQuery.SearchSource, dateQuery.FromDate, dateQuery.ToDate),
            _ => throw new ArgumentOutOfRangeException($"Unrecognized {nameof(BookSearchConsecutiveQueryOperator)}: {dateQuery.QueryOperator}.")
        };
    }
    
    private static IAsyncDocumentQuery<BookMultiSearch.Result> ApplyDateSequenceAdditionalQuery(IAsyncDocumentQuery<BookMultiSearch.Result> currentQuery, BookSearchDateSequenceAdditionalQueryParameters dateQuery)
    {
        return dateQuery.QueryOperator switch
        {
            BookSearchConsecutiveQueryOperator.And =>
                currentQuery.WithAndAlsoDateSequenceOperator(dateQuery.SearchSource, dateQuery.SequenceOperator, dateQuery.Date),
            BookSearchConsecutiveQueryOperator.AndNot =>
                currentQuery.WithAndAlsoNotDateSequenceOperator(dateQuery.SearchSource, dateQuery.SequenceOperator, dateQuery.Date),
            BookSearchConsecutiveQueryOperator.Or =>
                currentQuery.WithOrElseDateSequenceOperator(dateQuery.SearchSource, dateQuery.SequenceOperator, dateQuery.Date),
            BookSearchConsecutiveQueryOperator.OrNot =>
                currentQuery.WithOrElseNotDateSequenceOperator(dateQuery.SearchSource, dateQuery.SequenceOperator, dateQuery.Date),
            _ => throw new ArgumentOutOfRangeException($"Unrecognized {nameof(BookSearchConsecutiveQueryOperator)}: {dateQuery.QueryOperator}.")
        };
    }
}