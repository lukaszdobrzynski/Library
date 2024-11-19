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
    public BookSearchConsecutiveQueryOperator QueryOperator { get; set; }
}

public class BookSearchTextAdditionalQuery : BookSearchAdditionalQuery
{
    public string Term { get; set; }
    public BookTextSearchType SearchType { get; set; }
    public BookTextSearchSource SearchSource { get; set; }
}

public class BookSearchDateRangeAdditionalQuery : BookSearchAdditionalQuery
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public BookDateSearchSource SearchSource { get; set; }
}

public class BookSearchDateSequenceAdditionalQuery : BookSearchAdditionalQuery
{
    public DateTime Date { get; set; }
    public BookSearchDateSequenceOperator SequenceOperator { get; set; }
    public BookDateSearchSource SearchSource { get; set; }
}