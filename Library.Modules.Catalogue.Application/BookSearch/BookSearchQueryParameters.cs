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
                .Select(MapSearchAdditionalQueryToSearchAdditionalQueryParameters)
                .ToList();

        return new BookSearchQueryParameters
        {
            MainQuery = mainQuery,
            AdditionalQueries = additionalQueries,
            PageSize = query.PageSize,
            PageNumber = query.PageNumber
        };
    }

    private static BookSearchAdditionalQueryParameters MapSearchAdditionalQueryToSearchAdditionalQueryParameters(BookSearchAdditionalQuery query)
    {
        BookSearchAdditionalQueryParameters parameters = query switch
        {
            BookSearchTextAdditionalQuery additionalTextQuery => BookSearchTextAdditionalQueryParameters.From(additionalTextQuery),
            BookSearchDateRangeAdditionalQuery additionalDateRangeQuery => BookSearchDateRangeAdditionalQueryParameters.From(additionalDateRangeQuery),
            BookSearchDateSequenceAdditionalQuery additionalDateSequenceQuery => BookSearchDateSequenceAdditionalQueryParameters.From(additionalDateSequenceQuery),
            _ => throw new ArgumentException($"Unsupported query type: {query.GetType().Name}")
        };

        return parameters;
    }
}

public class BookSearchMainQueryParameters
{
    public bool IsNegated { get; set; }

    public static BookSearchMainQueryParameters From(BookSearchMainQuery query)
    {
        BookSearchMainQueryParameters parameters = query switch
        {
            BookSearchTextMainQuery textQuery => BookSearchTextMainQueryParameters.From(textQuery),
            BookSearchDateRangeMainQuery dateRangeQuery => BookSearchDateRangeMainQueryParameters.From(dateRangeQuery),
            BookSearchDateSequenceMainQuery dateSequenceQuery => BookSearchDateSequenceMainQueryParameters.From(
                dateSequenceQuery),
            _ => throw new ArgumentException($"Unsupported query type: {query.GetType().Name}")
        };
        
        return parameters;
    }
}

public class BookSearchTextMainQueryParameters : BookSearchMainQueryParameters
{
    public string Term { get; set; }
    public BookTextSearchType SearchType { get; set; }
    public BookTextSearchSource SearchSource { get; set; }

    public static BookSearchTextMainQueryParameters From(BookSearchTextMainQuery query)
    {
        return new BookSearchTextMainQueryParameters
        {
            Term = query.Term,
            SearchSource = query.SearchSource,
            SearchType = query.SearchType,
            IsNegated = query.IsNegated
        };
    }
}

public class BookSearchDateRangeMainQueryParameters : BookSearchMainQueryParameters
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public BookDateSearchSource SearchSource { get; set; }
    
    public static BookSearchDateRangeMainQueryParameters From(BookSearchDateRangeMainQuery query)
    {
        return new BookSearchDateRangeMainQueryParameters
        {
            FromDate = query.FromDate,
            ToDate = query.ToDate,
            SearchSource = query.SearchSource,
            IsNegated = query.IsNegated
        };
    }
}

public class BookSearchDateSequenceMainQueryParameters : BookSearchMainQueryParameters
{
    public DateTime Date { get; set; }
    public BookSearchDateSequenceOperator SequenceOperator { get; set; }
    public BookDateSearchSource SearchSource { get; set; }

    public static BookSearchDateSequenceMainQueryParameters From(BookSearchDateSequenceMainQuery query)
    {
        return new BookSearchDateSequenceMainQueryParameters
        {
            Date = query.Date,
            SequenceOperator = query.SequenceOperator,
            SearchSource = query.SearchSource,
            IsNegated = query.IsNegated
        };
    }
}

public abstract class BookSearchAdditionalQueryParameters
{
    public BookSearchConsecutiveQueryOperator QueryOperator { get; set; }
}

public class BookSearchTextAdditionalQueryParameters : BookSearchAdditionalQueryParameters
{
    public string Term { get; set; }
    public BookTextSearchType SearchType { get; set; }
    public BookTextSearchSource SearchSource { get; set; }

    public static BookSearchTextAdditionalQueryParameters From(BookSearchTextAdditionalQuery query)
    {
        return new BookSearchTextAdditionalQueryParameters
        {
            Term = query.Term,
            SearchType = query.SearchType,
            SearchSource = query.SearchSource,
            QueryOperator = query.QueryOperator,
        };
    }
}

public class BookSearchDateSequenceAdditionalQueryParameters : BookSearchAdditionalQueryParameters
{
    public DateTime Date { get; set; }
    public BookSearchDateSequenceOperator SequenceOperator { get; set; }
    public BookDateSearchSource SearchSource { get; set; }

    public static BookSearchDateSequenceAdditionalQueryParameters From(BookSearchDateSequenceAdditionalQuery query)
    {
        return new BookSearchDateSequenceAdditionalQueryParameters
        {
            Date = query.Date,
            SearchSource = query.SearchSource,
            SequenceOperator = query.SequenceOperator,
            QueryOperator = query.QueryOperator,
        };
    }
}

public class BookSearchDateRangeAdditionalQueryParameters : BookSearchAdditionalQueryParameters
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public BookDateSearchSource SearchSource { get; set; }

    public static BookSearchDateRangeAdditionalQueryParameters From(BookSearchDateRangeAdditionalQuery query)
    {
        return new BookSearchDateRangeAdditionalQueryParameters
        {
            FromDate = query.FromDate,
            ToDate = query.ToDate,
            SearchSource = query.SearchSource,
            QueryOperator = query.QueryOperator,
        };
    }
}