using Library.Modules.Catalogue.Application.Contracts;
using MediatR;

namespace Library.Modules.Catalogue.Application.BookSearch;

public class BookSearchQueryHandler : IRequestHandler<BookSearchQuery, BookSearchResultDto>
{
    private readonly IBookQueries _bookQueries;
    
    public BookSearchQueryHandler(IBookQueries bookQueries)
    {
        _bookQueries = bookQueries;
    }

    public async Task<BookSearchResultDto> Handle(BookSearchQuery query, CancellationToken cancellationToken)
    {
        var parameters = BookSearchQueryParameters.From(query);
        
        var queryResult = await _bookQueries.GetMultiSearchResult(parameters);
        
        var books = queryResult.Books.Select(BookDto.From).ToList();

        return new BookSearchResultDto
        {
            Books = books,
            TotalResults = queryResult.TotalResults
        };
    }
}