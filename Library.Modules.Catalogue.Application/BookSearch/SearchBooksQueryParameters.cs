namespace Library.Modules.Catalogue.Application.BookSearch;

public class SearchBooksQueryParameters
{
    public SearchBooksMainQueryParameters MainQuery { get; set; }
    public List<SearchBooksAdditionalQueryParameters> AdditionalQueries { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }

    public static SearchBooksQueryParameters From(SearchBooksQuery query)
    {
        var mainQuery = SearchBooksMainQueryParameters.From(query.MainQuery);
        var additionalQueries = query.AdditionalQueries.Select(SearchBooksAdditionalQueryParameters.From)
            .ToList();

        return new SearchBooksQueryParameters
        {
            MainQuery = mainQuery,
            AdditionalQueries = additionalQueries,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };
    }
}

public class SearchBooksMainQueryParameters
{
    public string Term { get; set; }
    public BookSearchType SearchType { get; set; }
    public BookSearchSource SearchSource { get; set; }
    public bool IsNegated { get; set; }

    public static SearchBooksMainQueryParameters From(SearchBooksMainQuery mainQuery)
    {
        return new SearchBooksMainQueryParameters
        {
            Term = mainQuery.Term,
            SearchType = mainQuery.SearchType,
            SearchSource = mainQuery.SearchSource,
            IsNegated = mainQuery.IsNegated
        };
    }
}

public class SearchBooksAdditionalQueryParameters
{
    public string Term { get; set; }
    public BookSearchType SearchType { get; set; }
    public BookSearchSource SearchSource { get; set; }
    public BookSearchQueryOperator Operator { get; set; }
    public bool IsNegated { get; set; }

    public static SearchBooksAdditionalQueryParameters From(SearchBooksAdditionalQuery additionalQuery)
    {
        return new SearchBooksAdditionalQueryParameters
        {
            Term = additionalQuery.Term,
            SearchType = additionalQuery.SearchType,
            SearchSource = additionalQuery.SearchSource,
            Operator = additionalQuery.Operator,
            IsNegated = additionalQuery.IsNegated
        };
    }
}