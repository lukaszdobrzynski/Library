using Library.Modules.Catalogue.Application.BookSearch;
using Library.Modules.Catalogue.Infrastructure.Indexes;
using Raven.Client.Documents.Session;

namespace Library.Modules.Catalogue.Infrastructure.Queries;

public class BookSearchQueryBuilder
{
    private readonly IAsyncDocumentQuery<BookMultiSearch.Result> _query;

    private readonly string _term;

    private BookSearchQueryBuilder(IAsyncDocumentQuery<BookMultiSearch.Result> query, string term)
    {
        _query = query;
        _term = term;
    }

    public static BookSearchQueryBuilder Init(IAsyncDocumentQuery<BookMultiSearch.Result> query, string term) =>
        new (query, term);

    public IAsyncDocumentQuery<BookMultiSearch.Result> BuildSearchAnywhereQuery(BookSearchType searchType)
    {
        switch (searchType)
        {
            case BookSearchType.ExactPhrase:
                _query.WhereEquals(nameof(BookMultiSearch.Result.ExactQuery), _term);
                return _query;
            case BookSearchType.AnyTerm:
                _query.Search(x => x.Query, _term);
                return _query;
            case BookSearchType.BeginsWith:
                _query.WhereStartsWith(nameof(BookMultiSearch.Result.ExactQuery), _term);
                return _query;
            default:
                throw new ArgumentException($"Unrecognized {nameof(BookSearchType)}: {searchType}.");
        }
    }

    public IAsyncDocumentQuery<BookMultiSearch.Result> BuildSearchAuthorQuery(BookSearchType searchType)
    {
        switch (searchType)
        {
            case BookSearchType.ExactPhrase:
                _query.WhereEquals(nameof(BookMultiSearch.Result.Author), _term);
                return _query;
            case BookSearchType.AnyTerm:
                _query.ContainsAny(x => x.Author, _term);
                return _query;
            case BookSearchType.BeginsWith:
                _query.WhereStartsWith(nameof(BookMultiSearch.Result.Author), _term);
                return _query;
            default:
                throw new ArgumentException($"Unrecognized {nameof(BookSearchType)}: {searchType}.");
        }
    }

    public IAsyncDocumentQuery<BookMultiSearch.Result> BuildSearchTitleQuery(BookSearchType searchType)
    {
        switch (searchType)
        {
            case BookSearchType.ExactPhrase:
                _query.WhereEquals(nameof(BookMultiSearch.Result.Title), _term);
                return _query;
            case BookSearchType.AnyTerm:
                _query.ContainsAny(x => x.Title, _term);
                return _query;
            case BookSearchType.BeginsWith:
                _query.WhereStartsWith(nameof(BookMultiSearch.Result.Title), _term);
                return _query;
            default:
                throw new ArgumentException($"Unrecognized {nameof(BookSearchType)}: {searchType}.");
        }
    }

    public IAsyncDocumentQuery<BookMultiSearch.Result> BuildSearchIsbnQuery(BookSearchType searchType)
    {
        switch (searchType)
        {
            case BookSearchType.ExactPhrase:
                _query.WhereEquals(nameof(BookMultiSearch.Result.Isbn), _term);
                return _query;
            case BookSearchType.AnyTerm:
                _query.ContainsAny(x => x.Isbn, _term);
                return _query;
            case BookSearchType.BeginsWith:
                _query.WhereStartsWith(nameof(BookMultiSearch.Result.Isbn), _term);
                return _query;
            default:
                throw new ArgumentException($"Unrecognized {nameof(BookSearchType)}: {searchType}.");
        }
    }
}