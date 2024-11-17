using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Library.Modules.Catalogue.Application.BookSearch;

namespace Library.API.Modules.Catalogue;

public class BookSearchRequest
{
    [Required]
    public BookSearchMainQueryRequest MainQuery { get; set; }

    [Required]
    public List<BookSearchAdditionalQueryRequest> AdditionalQueries { get; set; }
    
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public BookSearchQuery ToSearchBooksQuery()
    {
        var mainQuery = MainQuery.ToSearchBooksMainQuery();
        var additionalQueries = AdditionalQueries
            .Select(MapAdditionalQueryRequestToAdditionalQuery)
            .ToList();
        
        return new BookSearchQuery
        {
            MainQuery = mainQuery,
            AdditionalQueries = additionalQueries,
            PageSize = PageSize,
            PageNumber = PageNumber
        };
    }

    private static BookSearchAdditionalQuery MapAdditionalQueryRequestToAdditionalQuery(
        BookSearchAdditionalQueryRequest request)
    {
        BookSearchAdditionalQuery additionalQuery = request switch
        {
            BookSearchAdditionalTextQueryRequest additionalTextQueryRequest => additionalTextQueryRequest
                .ToSearchBooksAdditionalTextQuery(),
            BookSearchAdditionalDateRangeQueryRequest additionalDateQueryRequest =>
                additionalDateQueryRequest.ToSearchBooksAdditionalDateRangeQuery(),
            BookSearchAdditionalDateSequenceQueryRequest additionalDateSequenceQueryRequest =>
                additionalDateSequenceQueryRequest.ToSearchBooksAdditionalDateSequenceQuery(),
            _ => throw new ArgumentException($"Unsupported query type: {request.GetType().Name}")
        };

        return additionalQuery;
    }
}

public class BookSearchMainQueryRequest
{
    [Required]
    public string Term { get; set; }
    
    [Required]
    public BookTextSearchType? SearchType { get; set; }

    [Required]
    public BookTextSearchSource? SearchSource { get; set; }

    public bool IsNegated { get; set; }

    public BookSearchMainQuery ToSearchBooksMainQuery()
    {
        return new BookSearchMainQuery
        {
            Term = Term,
            SearchSource = SearchSource.Value,
            SearchType = SearchType.Value,
            IsNegated = IsNegated
        };
    }
}

public class BookSearchAdditionalTextQueryRequest : BookSearchAdditionalQueryRequest
{
    [Required]
    public string Term { get; set; }
    
    [Required]
    public BookTextSearchType? SearchType { get; set; }

    [Required]
    public BookTextSearchSource? SearchSource { get; set; }

    public BookSearchAdditionalTextQuery ToSearchBooksAdditionalTextQuery()
    {
        return new BookSearchAdditionalTextQuery
        {
            Term = Term,
            SearchSource = SearchSource.Value,
            SearchType = SearchType.Value,
            ConsecutiveQueryOperator = ConsecutiveQueryOperator.Value,
        };
    }
}

public class BookSearchAdditionalDateRangeQueryRequest : BookSearchAdditionalQueryRequest
{
    [Required]
    public DateTime? FromDate { get; set; }

    [Required]
    public DateTime? ToDate { get; set; }
    
    [Required]
    public BookDateSearchSource? SearchSource { get; set; }
   
    public BookSearchAdditionalDateRangeQuery ToSearchBooksAdditionalDateRangeQuery()
    {
        return new BookSearchAdditionalDateRangeQuery
        {
            FromDate = FromDate.Value,
            ToDate = ToDate.Value,
            SearchSource = SearchSource.Value,
            ConsecutiveQueryOperator = ConsecutiveQueryOperator.Value,
        };
    }
}

public class BookSearchAdditionalDateSequenceQueryRequest : BookSearchAdditionalQueryRequest
{
    [Required]
    public DateTime? DateToCompare { get; set; }
    
    [Required]
    public DateSequenceSearchOperator? DateSequenceOperator { get; set; }
    
    [Required]
    public BookDateSearchSource? SearchSource { get; set; }

    public BookSearchAdditionalDateSequenceQuery ToSearchBooksAdditionalDateSequenceQuery()
    {
        return new BookSearchAdditionalDateSequenceQuery
        {
            DateToCompare = DateToCompare.Value,
            SearchSource = SearchSource.Value,
            ConsecutiveQueryOperator = ConsecutiveQueryOperator.Value,
            DateSequenceOperator = DateSequenceOperator.Value,
        };
    }
}

public class BookSearchAdditionalQueryRequest
{
    [Required]
    public BookSearchAdditionalQueryType Type { get; set; }
    
    [Required]
    public BookSearchConsecutiveQueryOperator? ConsecutiveQueryOperator { get; set; }
}