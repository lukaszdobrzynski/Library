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
        var queryResult = await _bookQueries.GetMultiSearchResults(new SearchBooksQueryParameters
        {
            Term = query.Term,
            SearchType = query.SearchType,
            SearchSource = query.SearchSource,
            PageSize = query.PageSize,
            PageNumber = query.PageNumber
        });
        
        var books = queryResult.Books.Select(BookDto.From).ToList();

        return new SearchBooksResultDto
        {
            Books = books,
            TotalResults = queryResult.TotalResults
        };
    }
}