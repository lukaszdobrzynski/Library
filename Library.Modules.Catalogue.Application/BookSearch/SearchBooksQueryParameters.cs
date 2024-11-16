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
        
        var additionalTextQueries =
            query.AdditionalTextQueries
                .Select(SearchBooksAdditionalTextQueryParameters.From)
                .ToList();
        var additionalDateRangeQueries = query.AdditionalDateRangeQueries
            .Select(SearchBooksAdditionalDateRangeQueryParameters.From)
            .ToList();
        var additionalDateComparisonQueries = query.AdditionalDateSequenceQueries
            .Select(SearchBooksAdditionalDateSequenceQueryParameters.From)
            .ToList();

        var additionalQueries = new List<SearchBooksAdditionalQueryParameters>();
        additionalQueries.AddRange(additionalTextQueries);
        additionalQueries.AddRange(additionalDateRangeQueries);
        additionalQueries.AddRange(additionalDateComparisonQueries);

        return new SearchBooksQueryParameters
        {
            MainQuery = mainQuery,
            AdditionalQueries = additionalQueries,
            PageSize = query.PageSize,
            PageNumber = query.PageNumber
        };
    }
}

public class SearchBooksMainQueryParameters
{
    public string Term { get; set; }
    public BookTextSearchType SearchType { get; set; }
    public BookTextSearchSource SearchSource { get; set; }
    public bool IsNegated { get; set; }

    public static SearchBooksMainQueryParameters From(SearchBooksMainQuery query)
    {
        return new SearchBooksMainQueryParameters
        {
            Term = query.Term,
            SearchSource = query.SearchSource,
            SearchType = query.SearchType,
            IsNegated = query.IsNegated
        };
    }
}

public abstract class SearchBooksAdditionalQueryParameters
{
    public BookSearchConsecutiveQueryOperator ConsecutiveQueryOperator { get; set; }
    public int Order { get; set; }
    public bool IsNegated { get; set; }
}

public class SearchBooksAdditionalTextQueryParameters : SearchBooksAdditionalQueryParameters
{
    public string Term { get; set; }
    public BookTextSearchType SearchType { get; set; }
    public BookTextSearchSource SearchSource { get; set; }

    public static SearchBooksAdditionalTextQueryParameters From(SearchBooksAdditionalTextQuery query)
    {
        return new SearchBooksAdditionalTextQueryParameters
        {
            Term = query.Term,
            SearchType = query.SearchType,
            SearchSource = query.SearchSource,
            ConsecutiveQueryOperator = query.ConsecutiveQueryOperator,
            IsNegated = query.IsNegated,
            Order = query.Order
        };
    }
}

public class SearchBooksAdditionalDateSequenceQueryParameters : SearchBooksAdditionalQueryParameters
{
    public DateTime DateToCompare { get; set; }
    public DateSequenceSearchOperator DateSequenceOperator { get; set; }
    public BookDateSearchSource SearchSource { get; set; }

    public static SearchBooksAdditionalDateSequenceQueryParameters From(SearchBooksAdditionalDateSequenceQuery query)
    {
        return new SearchBooksAdditionalDateSequenceQueryParameters
        {
            DateToCompare = query.DateToCompare,
            SearchSource = query.SearchSource,
            DateSequenceOperator = query.DateSequenceOperator,
            ConsecutiveQueryOperator = query.ConsecutiveQueryOperator,
            Order = query.Order,
            IsNegated = query.IsNegated
        };
    }
}

public class SearchBooksAdditionalDateRangeQueryParameters : SearchBooksAdditionalQueryParameters
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public BookDateSearchSource SearchSource { get; set; }

    public static SearchBooksAdditionalDateRangeQueryParameters From(SearchBooksAdditionalDateRangeQuery query)
    {
        return new SearchBooksAdditionalDateRangeQueryParameters
        {
            FromDate = query.FromDate,
            ToDate = query.ToDate,
            SearchSource = query.SearchSource,
            ConsecutiveQueryOperator = query.ConsecutiveQueryOperator,
            IsNegated = query.IsNegated,
            Order = query.Order
        };
    }
}