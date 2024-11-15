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
    public List<BookSearchAdditionalQueryRequest> AdditionalQueries { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public SearchBooksQuery ToSearchBooksQuery()
    {
        var mainQuery = MainQuery.ToSearchBooksMainQuery();
        var additionalQueries = AdditionalQueries
            .Select(x => x.ToSearchBooksAdditionalQuery())
            .ToList();

        return new SearchBooksQuery
        {
            MainQuery = mainQuery,
            AdditionalQueries = additionalQueries,
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
    public BookSearchType? SearchType { get; set; }

    [Required]
    public BookSearchSource? SearchSource { get; set; }

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

public class BookSearchAdditionalQueryRequest
{
    [Required]
    public string Term { get; set; }
    
    [Required]
    public BookSearchType? SearchType { get; set; }

    [Required]
    public BookSearchSource? SearchSource { get; set; }

    [Required]
    public BookSearchQueryOperator? Operator { get; set; }

    public bool IsNegated { get; set; }

    public SearchBooksAdditionalQuery ToSearchBooksAdditionalQuery()
    {
        return new SearchBooksAdditionalQuery
        {
            Term = Term,
            SearchSource = SearchSource.Value,
            SearchType = SearchType.Value,
            Operator = Operator.Value,
            IsNegated = IsNegated
        };
    }
}