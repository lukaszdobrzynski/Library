using Library.Modules.Catalogue.Application.BookSearch;
using Library.Modules.Catalogue.Infrastructure.Indexes;
using Raven.Client.Documents.Session;

namespace Library.Modules.Catalogue.Infrastructure.Queries;

public class BookDateSearchQueryBuilder
{
    private readonly IAsyncDocumentQuery<BookMultiSearch.Result> _query;
    
    private BookDateSearchQueryBuilder(IAsyncDocumentQuery<BookMultiSearch.Result> query)
    {
        _query = query;
    }

    public static BookDateSearchQueryBuilder Init(IAsyncDocumentQuery<BookMultiSearch.Result> query)
    {
        return new(query);
    }

    public IAsyncDocumentQuery<BookMultiSearch.Result> BuildRangeQuery(BookDateSearchSource searchSource, DateTime startDate, DateTime endDate)
    {
        return _query.WhereBetween(searchSource.ToString(), startDate, endDate);
    }

    public IAsyncDocumentQuery<BookMultiSearch.Result> BuildSequenceQuery(BookDateSearchSource searchSource,
        BookSearchDateSequenceOperator sequenceOperator, DateTime date)
    {
        return sequenceOperator switch
        {
            BookSearchDateSequenceOperator.After => _query.WhereGreaterThan(searchSource.ToString(), date),
            BookSearchDateSequenceOperator.Before => _query.WhereLessThan(searchSource.ToString(), date),
            _ => throw new ArgumentException($"Unrecognized {nameof(BookSearchDateSequenceOperator)}: {sequenceOperator}.")
        };
    }
}