namespace Library.Modules.Catalogue.Application.BookSearch;

public class BookSearchQueryParameters
{
    public BookSearchMainQueryParameters MainQuery { get; set; }
    public List<BookSearchAdditionalQueryParameters> AdditionalQueries { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }

    public static BookSearchQueryParameters From(BookSearchQuery query)
    {
        var mainQuery = BookSearchMainQueryParameters.From(query.MainQuery);

        var additionalQueries =
            query.AdditionalQueries
                .Select(MapAdditionalSearchQueryToAdditionalSearchQueryParameters)
                .ToList();

        return new BookSearchQueryParameters
        {
            MainQuery = mainQuery,
            AdditionalQueries = additionalQueries,
            PageSize = query.PageSize,
            PageNumber = query.PageNumber
        };
    }

    private static BookSearchAdditionalQueryParameters MapAdditionalSearchQueryToAdditionalSearchQueryParameters(BookSearchAdditionalQuery query)
    {
        BookSearchAdditionalQueryParameters parameters = query switch
        {
            BookSearchAdditionalTextQuery additionalTextQuery => BookSearchAdditionalTextQueryParameters.From(additionalTextQuery),
            BookSearchAdditionalDateRangeQuery additionalDateRangeQuery => BookSearchAdditionalDateRangeQueryParameters.From(additionalDateRangeQuery),
            BookSearchAdditionalDateSequenceQuery additionalDateSequenceQuery => BookSearchAdditionalDateSequenceQueryParameters.From(additionalDateSequenceQuery),
            _ => throw new ArgumentException($"Unsupported query type: {query.GetType().Name}")
        };

        return parameters;
    }
}

public class BookSearchMainQueryParameters
{
    public string Term { get; set; }
    public BookTextSearchType SearchType { get; set; }
    public BookTextSearchSource SearchSource { get; set; }
    public bool IsNegated { get; set; }

    public static BookSearchMainQueryParameters From(BookSearchMainQuery query)
    {
        return new BookSearchMainQueryParameters
        {
            Term = query.Term,
            SearchSource = query.SearchSource,
            SearchType = query.SearchType,
            IsNegated = query.IsNegated
        };
    }
}

public abstract class BookSearchAdditionalQueryParameters
{
    public BookSearchConsecutiveQueryOperator ConsecutiveQueryOperator { get; set; }
}

public class BookSearchAdditionalTextQueryParameters : BookSearchAdditionalQueryParameters
{
    public string Term { get; set; }
    public BookTextSearchType SearchType { get; set; }
    public BookTextSearchSource SearchSource { get; set; }

    public static BookSearchAdditionalTextQueryParameters From(BookSearchAdditionalTextQuery query)
    {
        return new BookSearchAdditionalTextQueryParameters
        {
            Term = query.Term,
            SearchType = query.SearchType,
            SearchSource = query.SearchSource,
            ConsecutiveQueryOperator = query.ConsecutiveQueryOperator,
        };
    }
}

public class BookSearchAdditionalDateSequenceQueryParameters : BookSearchAdditionalQueryParameters
{
    public DateTime DateToCompare { get; set; }
    public DateSequenceSearchOperator DateSequenceOperator { get; set; }
    public BookDateSearchSource SearchSource { get; set; }

    public static BookSearchAdditionalDateSequenceQueryParameters From(BookSearchAdditionalDateSequenceQuery query)
    {
        return new BookSearchAdditionalDateSequenceQueryParameters
        {
            DateToCompare = query.DateToCompare,
            SearchSource = query.SearchSource,
            DateSequenceOperator = query.DateSequenceOperator,
            ConsecutiveQueryOperator = query.ConsecutiveQueryOperator,
        };
    }
}

public class BookSearchAdditionalDateRangeQueryParameters : BookSearchAdditionalQueryParameters
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public BookDateSearchSource SearchSource { get; set; }

    public static BookSearchAdditionalDateRangeQueryParameters From(BookSearchAdditionalDateRangeQuery query)
    {
        return new BookSearchAdditionalDateRangeQueryParameters
        {
            FromDate = query.FromDate,
            ToDate = query.ToDate,
            SearchSource = query.SearchSource,
            ConsecutiveQueryOperator = query.ConsecutiveQueryOperator,
        };
    }
}