using Library.Modules.Catalogue.Application.BookSearch;
using Library.Modules.Catalogue.Infrastructure.Indexes;
using Raven.Client.Documents.Session;

namespace Library.Modules.Catalogue.Infrastructure.Queries;

public static class BookSearchDocumentQueryExtensions
{
    public static IAsyncDocumentQuery<BookMultiSearch.Result> TextUntransformed(this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookTextSearchSource searchSource, BookTextSearchType searchType, string term)
    {
        var q = BookTextSearchQueryBuilder.Init(query, term)
            .Build(searchSource, searchType);

        return q;
    }
    
    public static IAsyncDocumentQuery<BookMultiSearch.Result> TextWithNotOperator(this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookTextSearchSource searchSource, BookTextSearchType searchType, string term)
    {
        var q = BookTextSearchQueryBuilder.Init(query.Not, term)
            .Build(searchSource, searchType);

        return q;
    }

    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithDateRangeOperator(
        this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookDateSearchSource searchSource, DateTime startDate,
        DateTime endDate)
    {
        var q = BookDateSearchQueryBuilder.Init(query)
            .BuildRangeQuery(searchSource, startDate, endDate);
        
        return q;
    }
    
    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithNotDateRangeOperator(
        this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookDateSearchSource searchSource, DateTime startDate,
        DateTime endDate)
    {
        var q = BookDateSearchQueryBuilder.Init(query.Not)
            .BuildRangeQuery(searchSource, startDate, endDate);
        
        return q;
    }

    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithDateSequenceOperator(
        this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookDateSearchSource searchSource,
        BookSearchDateSequenceOperator sequenceOperator, DateTime date)
    {
        var q = BookDateSearchQueryBuilder.Init(query)
            .BuildSequenceQuery(searchSource, sequenceOperator, date);
        
        return q;
    }
    
    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithNotDateSequenceOperator(
        this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookDateSearchSource searchSource,
        BookSearchDateSequenceOperator sequenceOperator, DateTime date)
    {
        var q = BookDateSearchQueryBuilder.Init(query.Not)
            .BuildSequenceQuery(searchSource, sequenceOperator, date);
        
        return q;
    }
    
    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithAndAlsoOperator(this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookTextSearchSource searchSource, BookTextSearchType searchType, string term)
    {
        var q = BookTextSearchQueryBuilder.Init(query.AndAlso(), term)
            .Build(searchSource, searchType);

        return q;
    }
    
    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithOrElseOperator(this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookTextSearchSource searchSource, BookTextSearchType searchType, string term)
    {
        var q = BookTextSearchQueryBuilder.Init(query.OrElse(), term)
            .Build(searchSource, searchType);

        return q;
    }
    
    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithAndAlsoNotOperator(this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookTextSearchSource searchSource, BookTextSearchType searchType, string term)
    {
        var q = BookTextSearchQueryBuilder.Init(query.AndAlso().Not, term)
            .Build(searchSource, searchType);

        return q;
    }
    
    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithOrElseNotOperator(this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookTextSearchSource searchSource, BookTextSearchType searchType, string term)
    {
        var q = BookTextSearchQueryBuilder.Init(query.OrElse().Not, term)
            .Build(searchSource, searchType);

        return q;
    }
    
    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithAndAlsoNotDateRangeOperator(this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookDateSearchSource searchSource,  DateTime startDate, DateTime endDate)
    {
        return BookDateSearchQueryBuilder.Init(query.AndAlso().Not)
            .BuildRangeQuery(searchSource, startDate, endDate);
    }
    
    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithAndAlsoDateRangeOperator(this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookDateSearchSource searchSource,  DateTime startDate, DateTime endDate)
    {
        return BookDateSearchQueryBuilder.Init(query.AndAlso())
            .BuildRangeQuery(searchSource, startDate, endDate);
    }
    
    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithOrElseNotDateRangeOperator(this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookDateSearchSource searchSource,  DateTime startDate, DateTime endDate)
    {
        return BookDateSearchQueryBuilder.Init(query.OrElse().Not)
            .BuildRangeQuery(searchSource, startDate, endDate);
    }
    
    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithOrElseDateRangeOperator(this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookDateSearchSource searchSource, DateTime startDate, DateTime endDate)
    {
        return BookDateSearchQueryBuilder.Init(query.OrElse())
            .BuildRangeQuery(searchSource, startDate, endDate);
    }
    
    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithAndAlsoNotDateSequenceOperator(this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookDateSearchSource searchSource, BookSearchDateSequenceOperator sequenceOperator, DateTime date)
    {
        return BookDateSearchQueryBuilder.Init(query.AndAlso().Not)
            .BuildSequenceQuery(searchSource, sequenceOperator, date);
    }
    
    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithAndAlsoDateSequenceOperator(this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookDateSearchSource searchSource, BookSearchDateSequenceOperator sequenceOperator, DateTime date)
    {
        return BookDateSearchQueryBuilder.Init(query.AndAlso())
            .BuildSequenceQuery(searchSource, sequenceOperator, date);
    }
    
    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithOrElseNotDateSequenceOperator(this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookDateSearchSource searchSource, BookSearchDateSequenceOperator sequenceOperator, DateTime date)
    {
        return BookDateSearchQueryBuilder.Init(query.OrElse().Not)
            .BuildSequenceQuery(searchSource, sequenceOperator, date);
    }
    
    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithOrElseDateSequenceOperator(this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookDateSearchSource searchSource, BookSearchDateSequenceOperator sequenceOperator, DateTime date)
    {
        return BookDateSearchQueryBuilder.Init(query.OrElse())
            .BuildSequenceQuery(searchSource, sequenceOperator, date);
    }
}