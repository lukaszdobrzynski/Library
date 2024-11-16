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
    public List<BookSearchAdditionalTextQueryRequest> AdditionalTextQueries { get; set; }

    [Required]
    public List<BookSearchAdditionalDateRangeQueryRequest> AdditionalDateRangeQueries { get; set; }

    [Required]
    public List<BookSearchAdditionalDateSequenceQueryRequest> AdditionalDateSequenceQueries { get; set; }
    
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public SearchBooksQuery ToSearchBooksQuery()
    {
        var mainQuery = MainQuery.ToSearchBooksMainQuery();
        var additionalTextQueries = AdditionalTextQueries
            .Select(x => x.ToSearchBooksAdditionalTextQuery())
            .ToList();
        var additionalDateRangeQueries = AdditionalDateRangeQueries
            .Select(x => x.ToSearchBooksAdditionalDateRangeQuery())
            .ToList();
        var additionalDateSequenceQueries = AdditionalDateSequenceQueries
            .Select(x => x.ToSearchBooksAdditionalDateSequenceQuery())
            .ToList();

        return new SearchBooksQuery
        {
            MainQuery = mainQuery,
            AdditionalTextQueries = additionalTextQueries,
            AdditionalDateRangeQueries = additionalDateRangeQueries,
            AdditionalDateSequenceQueries = additionalDateSequenceQueries,
            PageSize = PageSize,
            PageNumber = PageNumber
        };
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

    public SearchBooksMainQuery ToSearchBooksMainQuery()
    {
        return new SearchBooksMainQuery
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

    public SearchBooksAdditionalTextQuery ToSearchBooksAdditionalTextQuery()
    {
        return new SearchBooksAdditionalTextQuery
        {
            Term = Term,
            SearchSource = SearchSource.Value,
            SearchType = SearchType.Value,
            ConsecutiveQueryOperator = ConsecutiveQueryOperator.Value,
            IsNegated = IsNegated,
            Order = Order
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
   
    public SearchBooksAdditionalDateRangeQuery ToSearchBooksAdditionalDateRangeQuery()
    {
        return new SearchBooksAdditionalDateRangeQuery
        {
            FromDate = FromDate.Value,
            ToDate = ToDate.Value,
            SearchSource = SearchSource.Value,
            ConsecutiveQueryOperator = ConsecutiveQueryOperator.Value,
            IsNegated = IsNegated,
            Order = Order
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

    public SearchBooksAdditionalDateSequenceQuery ToSearchBooksAdditionalDateSequenceQuery()
    {
        return new SearchBooksAdditionalDateSequenceQuery
        {
            DateToCompare = DateToCompare.Value,
            SearchSource = SearchSource.Value,
            ConsecutiveQueryOperator = ConsecutiveQueryOperator.Value,
            DateSequenceOperator = DateSequenceOperator.Value,
            IsNegated = IsNegated,
            Order = Order
        };
    }
}

public abstract class BookSearchAdditionalQueryRequest
{
    public bool IsNegated { get; set; }
    public int Order { get; set; }
    
    [Required]
    public BookSearchConsecutiveQueryOperator? ConsecutiveQueryOperator { get; set; }
}