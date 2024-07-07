using Library.Modules.Catalogue.Application.Contracts;
using Library.Modules.Catalogue.Infrastructure.Indexes;
using Library.Modules.Catalogue.Models;
using Raven.Client.Documents;

namespace Library.Modules.Catalogue.Infrastructure.Queries;

public class BookQueries : IBookQueries
{
    private readonly IDocumentStoreHolder _documentStoreHolder;
    
    public BookQueries(IDocumentStoreHolder documentStoreHolder)
    {
        _documentStoreHolder = documentStoreHolder;
    }

    public async Task<BookMultiSearchQueryResult> GetMultiSearchResults(string term, int skip, int take)
    {
        using (var session = _documentStoreHolder.OpenAsyncSession())
        {
            var result = await session.Query<BookMultiSearch.Result, BookMultiSearch>()
                .Search(x => x.Query, term)
                .Statistics(out var stats)
                .OfType<Book>()
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return new BookMultiSearchQueryResult
            {
                Books = result,
                TotalResults = (int)stats.TotalResults
            };
        }
    }
}