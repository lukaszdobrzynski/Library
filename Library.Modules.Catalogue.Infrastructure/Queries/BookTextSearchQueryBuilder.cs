using Library.Modules.Catalogue.Application.BookSearch;
using Library.Modules.Catalogue.Infrastructure.Indexes;
using Raven.Client.Documents.Session;

namespace Library.Modules.Catalogue.Infrastructure.Queries;

public class BookTextSearchQueryBuilder
{
    private readonly IAsyncDocumentQuery<BookMultiSearch.Result> _query;
    private readonly string _term;
    
    private BookTextSearchQueryBuilder(IAsyncDocumentQuery<BookMultiSearch.Result> query, string term)
    {
        _query = query;
        _term = term;
    }

    public static BookTextSearchQueryBuilder Init(IAsyncDocumentQuery<BookMultiSearch.Result> query, string term)
    {
        return new (query, term);
    }
        
    public IAsyncDocumentQuery<BookMultiSearch.Result> Build(BookTextSearchSource searchSource, BookTextSearchType searchType)
    {
        var q = searchSource switch
        {
            BookTextSearchSource.Anywhere => BuildSearchAnywhereQuery(searchType),
            BookTextSearchSource.Author => BuildSearchAuthorQuery(searchType),
            BookTextSearchSource.Title => BuildSearchTitleQuery(searchType),
            BookTextSearchSource.Isbn => BuildSearchIsbnQuery(searchType),
            BookTextSearchSource.PublishingHouse => BuildSearchPublishingHouseQuery(searchType),
            _ => throw new ArgumentOutOfRangeException($"Unrecognized {nameof(BookTextSearchSource)}: {searchSource}.")
        };

        return q;
    }

    private IAsyncDocumentQuery<BookMultiSearch.Result> BuildSearchAnywhereQuery(BookTextSearchType searchType)
    {
        switch (searchType)
        {
            case BookTextSearchType.ExactPhrase:
                _query.WhereEquals(nameof(BookMultiSearch.Result.ExactQuery),_term);
                return _query;
            case BookTextSearchType.AnyTerm:
                _query.Search(x => x.AnywhereQuery, _term);
                return _query;
            case BookTextSearchType.BeginsWith:
                _query.WhereStartsWith(nameof(BookMultiSearch.Result.ExactQuery), _term);
                return _query;
            default:
                throw new ArgumentException($"Unrecognized {nameof(BookTextSearchType)}: {searchType}.");
        }
    }

    private IAsyncDocumentQuery<BookMultiSearch.Result> BuildSearchAuthorQuery(BookTextSearchType searchType)
    {
        switch (searchType)
        {
            case BookTextSearchType.ExactPhrase:
                _query.WhereEquals(nameof(BookMultiSearch.Result.Author), _term);
                return _query;
            case BookTextSearchType.AnyTerm:
                _query.Search(x => x.AuthorQuery, _term);
                return _query;
            case BookTextSearchType.BeginsWith:
                _query.WhereStartsWith(nameof(BookMultiSearch.Result.Author), _term);
                return _query;
            default:
                throw new ArgumentException($"Unrecognized {nameof(BookTextSearchType)}: {searchType}.");
        }
    }

    private IAsyncDocumentQuery<BookMultiSearch.Result> BuildSearchTitleQuery(BookTextSearchType searchType)
    {
        switch (searchType)
        {
            case BookTextSearchType.ExactPhrase:
                _query.WhereEquals(nameof(BookMultiSearch.Result.Title), _term);
                return _query;
            case BookTextSearchType.AnyTerm:
                _query.Search(x => x.TitleQuery, _term);
                return _query;
            case BookTextSearchType.BeginsWith:
                _query.WhereStartsWith(nameof(BookMultiSearch.Result.Title), _term);
                return _query;
            default:
                throw new ArgumentException($"Unrecognized {nameof(BookTextSearchType)}: {searchType}.");
        }
    }
    
    private IAsyncDocumentQuery<BookMultiSearch.Result> BuildSearchPublishingHouseQuery(BookTextSearchType searchType)
    {
        switch (searchType)
        {
            case BookTextSearchType.ExactPhrase:
                _query.WhereEquals(nameof(BookMultiSearch.Result.PublishingHouse), _term);
                return _query;
            case BookTextSearchType.AnyTerm:
                _query.Search(x => x.PublishingHouseQuery, _term);
                return _query;
            case BookTextSearchType.BeginsWith:
                _query.WhereStartsWith(nameof(BookMultiSearch.Result.PublishingHouse), _term);
                return _query;
            default:
                throw new ArgumentException($"Unrecognized {nameof(BookTextSearchType)}: {searchType}.");
        }
    }

    private IAsyncDocumentQuery<BookMultiSearch.Result> BuildSearchIsbnQuery(BookTextSearchType searchType)
    {
        switch (searchType)
        {
            case BookTextSearchType.ExactPhrase:
                _query.WhereEquals(nameof(BookMultiSearch.Result.Isbn), _term);
                return _query;
            case BookTextSearchType.AnyTerm:
                _query.WhereRegex(x => x.Isbn, $".*{_term}.*");
                return _query;
            case BookTextSearchType.BeginsWith:
                _query.WhereStartsWith(nameof(BookMultiSearch.Result.Isbn), _term);
                return _query;
            default:
                throw new ArgumentException($"Unrecognized {nameof(BookTextSearchType)}: {searchType}.");
        }
    }
}