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

    public async Task<SearchAllBooksResultDto> Handle(SearchAllBooksQuery request, CancellationToken cancellationToken)
    {
        var skip = (request.PageNumber * request.PageSize) - request.PageSize;
        var queryResult = await _bookQueries.GetMultiSearchResults(request.Term, skip, request.PageSize);
        var books = queryResult.Books.Select(BookDto.From).ToList();

        return new SearchAllBooksResultDto
        {
            Books = books,
            TotalResults = queryResult.TotalResults
        };
    }
}