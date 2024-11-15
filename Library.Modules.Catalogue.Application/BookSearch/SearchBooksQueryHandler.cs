using Library.Modules.Catalogue.Application.Contracts;
using MediatR;

namespace Library.Modules.Catalogue.Application.BookSearch;

public class SearchBooksQueryHandler : IRequestHandler<SearchBooksQuery, SearchBooksResultDto>
{
    private readonly IBookQueries _bookQueries;
    
    public SearchBooksQueryHandler(IBookQueries bookQueries)
    {
        _bookQueries = bookQueries;
    }

    public async Task<SearchBooksResultDto> Handle(SearchBooksQuery query, CancellationToken cancellationToken)
    {
        var parameters = SearchBooksQueryParameters.From(query);
        
        var queryResult = await _bookQueries.GetMultiSearchResult(parameters);
        
        var books = queryResult.Books.Select(BookDto.From).ToList();

        return new SearchBooksResultDto
        {
            Books = books,
            TotalResults = queryResult.TotalResults
        };
    }
}