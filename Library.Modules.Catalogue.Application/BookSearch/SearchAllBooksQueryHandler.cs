using Library.Modules.Catalogue.Application.Contracts;
using MediatR;

namespace Library.Modules.Catalogue.Application.BookSearch;

public class SearchAllBooksQueryHandler : IRequestHandler<SearchAllBooksQuery, SearchAllBooksResultDto>
{
    private readonly IBookQueries _bookQueries;
    
    public SearchAllBooksQueryHandler(IBookQueries bookQueries)
    {
        _bookQueries = bookQueries;
    }

    public async Task<SearchAllBooksResultDto> Handle(SearchAllBooksQuery query, CancellationToken cancellationToken)
    {
        var queryResult = await _bookQueries.GetSearchResults(new BookSearchQueryParameters
        {
            Term = query.Term,
            SearchType = query.SearchType,
            PageSize = query.PageSize,
            PageNumber = query.PageNumber
        });
        
        var books = queryResult.Books.Select(BookDto.From).ToList();

        return new SearchAllBooksResultDto
        {
            Books = books,
            TotalResults = queryResult.TotalResults
        };
    }
}