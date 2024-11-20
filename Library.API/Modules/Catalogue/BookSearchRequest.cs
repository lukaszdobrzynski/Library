using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
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
            BookSearchTextAdditionalQueryRequest additionalTextQueryRequest => additionalTextQueryRequest
                .ToSearchBooksAdditionalTextQuery(),
            BookSearchDateRangeAdditionalQueryRequest additionalDateQueryRequest =>
                additionalDateQueryRequest.ToSearchBooksAdditionalDateRangeQuery(),
            BookSearchDateSequenceAdditionalQueryRequest additionalDateSequenceQueryRequest =>
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

public class BookSearchTextAdditionalQueryRequest : BookSearchAdditionalQueryRequest
{
    [Required]
    public string Term { get; set; }
    
    [Required]
    public BookTextSearchType? SearchType { get; set; }

    [Required]
    public BookTextSearchSource? SearchSource { get; set; }

    public BookSearchTextAdditionalQuery ToSearchBooksAdditionalTextQuery()
    {
        return new BookSearchTextAdditionalQuery
        {
            Term = Term,
            SearchSource = SearchSource.Value,
            SearchType = SearchType.Value,
            QueryOperator = QueryOperator.Value,
        };
    }
}

public class BookSearchDateRangeAdditionalQueryRequest : BookSearchAdditionalQueryRequest
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }
    
    [Required]
    public BookDateSearchSource? SearchSource { get; set; }
   
    public BookSearchDateRangeAdditionalQuery ToSearchBooksAdditionalDateRangeQuery()
    {
        return new BookSearchDateRangeAdditionalQuery
        {
            FromDate = FromDate,
            ToDate = ToDate,
            SearchSource = SearchSource.Value,
            QueryOperator = QueryOperator.Value,
        };
    }
}

public class BookSearchDateSequenceAdditionalQueryRequest : BookSearchAdditionalQueryRequest
{
    public DateTime Date { get; set; }
    
    [Required]
    public BookSearchDateSequenceOperator? SequenceOperator { get; set; }
    
    [Required]
    public BookDateSearchSource? SearchSource { get; set; }

    public BookSearchDateSequenceAdditionalQuery ToSearchBooksAdditionalDateSequenceQuery()
    {
        return new BookSearchDateSequenceAdditionalQuery
        {
            Date = Date,
            SearchSource = SearchSource.Value,
            QueryOperator = QueryOperator.Value,
            SequenceOperator = SequenceOperator.Value,
        };
    }
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
[JsonDerivedType(typeof(BookSearchTextAdditionalQueryRequest), nameof(BookSearchAdditionalQueryType.Text))]
[JsonDerivedType(typeof(BookSearchDateRangeAdditionalQueryRequest), nameof(BookSearchAdditionalQueryType.DateRange))]
[JsonDerivedType(typeof(BookSearchDateSequenceAdditionalQueryRequest), nameof(BookSearchAdditionalQueryType.DateSequence))]
public class BookSearchAdditionalQueryRequest
{
    [Required]
    public BookSearchConsecutiveQueryOperator? QueryOperator { get; set; }
}