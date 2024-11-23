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
    [ValidatePolymorphicMainQuery]
    public BookSearchMainQueryRequest MainQuery { get; set; }

    [Required]
    public List<BookSearchAdditionalQueryRequest> AdditionalQueries { get; set; }
    
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public BookSearchQuery ToSearchBooksQuery()
    {
        var mainQuery = MainQuery.MapMainQueryRequestToMainQuery();
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

[JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
[JsonDerivedType(typeof(BookSearchTextMainQueryRequest), nameof(BookSearchMainQueryType.Text))]
[JsonDerivedType(typeof(BookSearchDateRangeMainQueryRequest), nameof(BookSearchMainQueryType.DateRange))]
[JsonDerivedType(typeof(BookSearchDateSequenceMainQueryRequest), nameof(BookSearchMainQueryType.DateSequence))]
public class BookSearchMainQueryRequest
{
    public bool IsNegated { get; set; }
    
    public BookSearchMainQuery MapMainQueryRequestToMainQuery()
    {
        BookSearchMainQuery mainQuery = this switch
        {
            BookSearchTextMainQueryRequest textRequest =>
                textRequest.ToBookSearchTextMainQuery(),
            BookSearchDateRangeMainQueryRequest dateRangeRequest =>
                dateRangeRequest.ToBookSearchDateRangeMainQuery(),
            BookSearchDateSequenceMainQueryRequest dateSequenceRequest =>
                dateSequenceRequest.ToBookSearchDateSequenceMainQuery(),
            _ => throw new ArgumentException($"Unsupported query type: {GetType().Name}")
        };
        
        return mainQuery;
    }
}

public class BookSearchTextMainQueryRequest : BookSearchMainQueryRequest
{
    public string Term { get; set; }
    
    public BookTextSearchType? SearchType { get; set; }

    public BookTextSearchSource? SearchSource { get; set; }

    public BookSearchTextMainQuery ToBookSearchTextMainQuery()
    {
        return new BookSearchTextMainQuery
        {
            Term = Term,
            SearchType = SearchType.Value,
            SearchSource = SearchSource.Value,
            IsNegated = IsNegated
        };
    }
}

public class BookSearchDateRangeMainQueryRequest : BookSearchMainQueryRequest
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }
    
    public BookDateSearchSource? SearchSource { get; set; }

    public BookSearchDateRangeMainQuery ToBookSearchDateRangeMainQuery()
    {
        return new BookSearchDateRangeMainQuery
        {
            FromDate = FromDate,
            ToDate = ToDate,
            SearchSource = SearchSource.Value,
            IsNegated = IsNegated
        };
    }
}

public class BookSearchDateSequenceMainQueryRequest : BookSearchMainQueryRequest
{
    public DateTime Date { get; set; }
    
    public BookSearchDateSequenceOperator? SequenceOperator { get; set; }
    
    public BookDateSearchSource? SearchSource { get; set; }

    public BookSearchDateSequenceMainQuery ToBookSearchDateSequenceMainQuery()
    {
        return new BookSearchDateSequenceMainQuery
        {
            Date = Date,
            SequenceOperator = SequenceOperator.Value,
            SearchSource = SearchSource.Value,
            IsNegated = IsNegated
        };
    }
}

public class BookSearchTextAdditionalQueryRequest : BookSearchAdditionalQueryRequest
{
    public string Term { get; set; }
    
    public BookTextSearchType? SearchType { get; set; }

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
    
    public BookSearchDateSequenceOperator? SequenceOperator { get; set; }
    
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
[ValidatePolymorphicAdditionalQuery]
public class BookSearchAdditionalQueryRequest
{
    [Required]
    public BookSearchConsecutiveQueryOperator? QueryOperator { get; set; }
}