using Library.Modules.Catalogue.Application.BookSearch;
using Library.Modules.Catalogue.Infrastructure.Indexes;
using Raven.Client.Documents.Session;

namespace Library.Modules.Catalogue.Infrastructure.Queries;

public static class BookSearchDocumentQueryExtensions
{
    public static IAsyncDocumentQuery<BookMultiSearch.Result> Untransformed(this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookSearchSource searchSource, BookSearchType searchType, string term)
    {
        var q = BookSearchQueryBuilder.Init(query, term)
            .Build(searchSource, searchType);

        return q;
    }
    
    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithNotOperator(this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookSearchSource searchSource, BookSearchType searchType, string term)
    {
        var q = BookSearchQueryBuilder.Init(query.Not, term)
            .Build(searchSource, searchType);

        return q;
    }
    
    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithAndAlsoOperator(this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookSearchSource searchSource, BookSearchType searchType, string term)
    {
        var q = BookSearchQueryBuilder.Init(query.AndAlso(), term)
            .Build(searchSource, searchType);

        return q;
    }
    
    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithOrElseOperator(this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookSearchSource searchSource, BookSearchType searchType, string term)
    {
        var q = BookSearchQueryBuilder.Init(query.OrElse(), term)
            .Build(searchSource, searchType);

        return q;
    }
    
    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithAndAlsoNotOperator(this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookSearchSource searchSource, BookSearchType searchType, string term)
    {
        var q = BookSearchQueryBuilder.Init(query.AndAlso().Not, term)
            .Build(searchSource, searchType);

        return q;
    }
    
    public static IAsyncDocumentQuery<BookMultiSearch.Result> WithOrElseNotOperator(this IAsyncDocumentQuery<BookMultiSearch.Result> query, BookSearchSource searchSource, BookSearchType searchType, string term)
    {
        var q = BookSearchQueryBuilder.Init(query.OrElse().Not, term)
            .Build(searchSource, searchType);

        return q;
    }
}