using Library.Modules.Catalogue.Application.Contracts;

namespace Library.Modules.Catalogue.Application.BookSearch;

public class BookSearchQuery : IQuery<BookSearchResultDto>
{
    public BookSearchMainQuery MainQuery { get; set; }
    
    public List<BookSearchAdditionalQuery> AdditionalQueries { get; set; } = new();

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class BookSearchMainQuery
{
    public string Term { get; set; }
    public BookTextSearchType SearchType { get; set; }
    public BookTextSearchSource SearchSource { get; set; }
    public bool IsNegated { get; set; }
}

public abstract class BookSearchAdditionalQuery
{
    public BookSearchConsecutiveQueryOperator ConsecutiveQueryOperator { get; set; }
}

public class BookSearchAdditionalTextQuery : BookSearchAdditionalQuery
{
    public string Term { get; set; }
    public BookTextSearchType SearchType { get; set; }
    public BookTextSearchSource SearchSource { get; set; }
}

public class BookSearchAdditionalDateRangeQuery : BookSearchAdditionalQuery
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public BookDateSearchSource SearchSource { get; set; }
}

public class BookSearchAdditionalDateSequenceQuery : BookSearchAdditionalQuery
{
    public DateTime DateToCompare { get; set; }
    public DateSequenceSearchOperator DateSequenceOperator { get; set; }
    public BookDateSearchSource SearchSource { get; set; }
}