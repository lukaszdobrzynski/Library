using Library.Modules.Catalogue.Application.Contracts;

namespace Library.Modules.Catalogue.Application.BookSearch;

public class SearchBooksQuery : IQuery<SearchBooksResultDto>
{
    public SearchBooksMainQuery MainQuery { get; set; }
    public List<SearchBooksAdditionalQuery> AdditionalQueries { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class SearchBooksMainQuery
{
    public string Term { get; set; }
    public BookSearchType SearchType { get; set; }
    public BookSearchSource SearchSource { get; set; }
    public bool IsNegated { get; set; }
}

public class SearchBooksAdditionalQuery
{
    public string Term { get; set; }
    public BookSearchType SearchType { get; set; }
    public BookSearchSource SearchSource { get; set; }
    public BookSearchQueryOperator Operator { get; set; }
    public bool IsNegated { get; set; }
}