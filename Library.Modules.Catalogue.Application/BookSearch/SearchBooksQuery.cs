using Library.Modules.Catalogue.Application.Contracts;

namespace Library.Modules.Catalogue.Application.BookSearch;

public class SearchBooksQuery : IQuery<SearchBooksResultDto>
{
    public SearchBooksMainQuery MainQuery { get; set; }
    
    public List<SearchBooksAdditionalTextQuery> AdditionalTextQueries { get; set; } = new();

    public List<SearchBooksAdditionalDateRangeQuery> AdditionalDateRangeQueries { get; set; } = new();

    public List<SearchBooksAdditionalDateSequenceQuery> AdditionalDateSequenceQueries { get; set; } = new();
    
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class SearchBooksMainQuery
{
    public string Term { get; set; }
    public BookTextSearchType SearchType { get; set; }
    public BookTextSearchSource SearchSource { get; set; }
    public bool IsNegated { get; set; }
}

public abstract class SearchBooksAdditionalQuery
{
    public int Order { get; set; }
    public bool IsNegated { get; set; }
    
    public BookSearchConsecutiveQueryOperator ConsecutiveQueryOperator { get; set; }
}

public class SearchBooksAdditionalTextQuery : SearchBooksAdditionalQuery
{
    public string Term { get; set; }
    public BookTextSearchType SearchType { get; set; }
    public BookTextSearchSource SearchSource { get; set; }
}

public class SearchBooksAdditionalDateRangeQuery : SearchBooksAdditionalQuery
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public BookDateSearchSource SearchSource { get; set; }
}

public class SearchBooksAdditionalDateSequenceQuery : SearchBooksAdditionalQuery
{
    public DateTime DateToCompare { get; set; }
    public DateSequenceSearchOperator DateSequenceOperator { get; set; }
    public BookDateSearchSource SearchSource { get; set; }
}