﻿using Library.Modules.Catalogue.Application.BookSearch;
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
        var currentQuery = parameters.MainQuery.IsNegated
            ? query.WithNotOperator(parameters.MainQuery.SearchSource, parameters.MainQuery.SearchType, parameters.MainQuery.Term)
            : query.Untransformed(parameters.MainQuery.SearchSource, parameters.MainQuery.SearchType, parameters.MainQuery.Term);

        foreach (var additionalQuery in parameters.AdditionalQueries)
        {
            currentQuery = additionalQuery switch
            {
                BookSearchAdditionalTextQueryParameters textQuery => ApplyTextQuery(currentQuery, textQuery),
                BookSearchAdditionalDateRangeQueryParameters dateRangeQuery => ApplyDateRangeQuery(currentQuery, dateRangeQuery),
                BookSearchAdditionalDateSequenceQueryParameters dateSequenceQuery => ApplyDateSequenceQuery(currentQuery, dateSequenceQuery),
                _ => throw new ArgumentException($"Unsupported query type: {additionalQuery.GetType().Name}")
            };
        }
        
        var skip = (parameters.PageNumber * parameters.PageSize) - parameters.PageSize;

        return currentQuery.Skip(skip)
            .Take(parameters.PageSize);
    }
    
    private static IAsyncDocumentQuery<BookMultiSearch.Result> ApplyTextQuery(IAsyncDocumentQuery<BookMultiSearch.Result> currentQuery, BookSearchAdditionalTextQueryParameters textQuery)
    {
        return textQuery.ConsecutiveQueryOperator switch
        {
            BookSearchConsecutiveQueryOperator.And =>
                currentQuery.WithAndAlsoOperator(textQuery.SearchSource, textQuery.SearchType, textQuery.Term),
            BookSearchConsecutiveQueryOperator.AndNot =>
                currentQuery.WithAndAlsoNotOperator(textQuery.SearchSource, textQuery.SearchType, textQuery.Term),
            BookSearchConsecutiveQueryOperator.Or =>
              currentQuery.WithOrElseOperator(textQuery.SearchSource, textQuery.SearchType, textQuery.Term),
            BookSearchConsecutiveQueryOperator.OrNot =>
                currentQuery.WithOrElseNotOperator(textQuery.SearchSource, textQuery.SearchType, textQuery.Term),
            _ => throw new ArgumentOutOfRangeException($"Unrecognized {nameof(BookSearchConsecutiveQueryOperator)}: {textQuery.ConsecutiveQueryOperator}.")
        };
    }
    
    private static IAsyncDocumentQuery<BookMultiSearch.Result> ApplyDateRangeQuery(IAsyncDocumentQuery<BookMultiSearch.Result> currentQuery, BookSearchAdditionalDateRangeQueryParameters dateQuery)
    {
        return dateQuery.ConsecutiveQueryOperator switch
        {
            BookSearchConsecutiveQueryOperator.And => 
               currentQuery.WithAndAlsoDateRangeOperator(dateQuery.SearchSource, dateQuery.FromDate, dateQuery.ToDate),
            BookSearchConsecutiveQueryOperator.AndNot =>
                currentQuery.WithAndAlsoNotDateRangeOperator(dateQuery.SearchSource, dateQuery.FromDate, dateQuery.ToDate),
            BookSearchConsecutiveQueryOperator.Or =>
                currentQuery.WithOrElseDateRangeOperator(dateQuery.SearchSource, dateQuery.FromDate, dateQuery.ToDate),
            BookSearchConsecutiveQueryOperator.OrNot =>
                currentQuery.WithOrElseNotDateRangeOperator(dateQuery.SearchSource, dateQuery.FromDate, dateQuery.ToDate),
            _ => throw new ArgumentOutOfRangeException($"Unrecognized {nameof(BookSearchConsecutiveQueryOperator)}: {dateQuery.ConsecutiveQueryOperator}.")
        };
    }
    
    private static IAsyncDocumentQuery<BookMultiSearch.Result> ApplyDateSequenceQuery(IAsyncDocumentQuery<BookMultiSearch.Result> currentQuery, BookSearchAdditionalDateSequenceQueryParameters dateQuery)
    {
        return dateQuery.ConsecutiveQueryOperator switch
        {
            BookSearchConsecutiveQueryOperator.And =>
                currentQuery.WithAndAlsoDateSequenceOperator(dateQuery.SearchSource, dateQuery.DateSequenceOperator, dateQuery.DateToCompare),
            BookSearchConsecutiveQueryOperator.AndNot =>
                currentQuery.WithAndAlsoNotDateSequenceOperator(dateQuery.SearchSource, dateQuery.DateSequenceOperator, dateQuery.DateToCompare),
            BookSearchConsecutiveQueryOperator.Or =>
                currentQuery.WithOrElseDateSequenceOperator(dateQuery.SearchSource, dateQuery.DateSequenceOperator, dateQuery.DateToCompare),
            BookSearchConsecutiveQueryOperator.OrNot =>
                currentQuery.WithOrElseNotDateSequenceOperator(dateQuery.SearchSource, dateQuery.DateSequenceOperator, dateQuery.DateToCompare),
            _ => throw new ArgumentOutOfRangeException($"Unrecognized {nameof(BookSearchConsecutiveQueryOperator)}: {dateQuery.ConsecutiveQueryOperator}.")
        };
    }
}