using Library.Modules.Catalogue.Models;
using Raven.Client.Documents.Indexes;

namespace Library.Modules.Catalogue.Infrastructure.Indexes;

public class BookMultiSearch : AbstractIndexCreationTask<Book, BookMultiSearch.Result>
{
    public class Result
    {
        public string Query { get; set; }

        public string QueryExactPhrase { get; set; }
    }

    public BookMultiSearch()
    {
        Map = books =>
            from book in books
            select new
            {
                Query = new object[]
                {
                    book.Isbn,
                    book.Author,
                    book.Genre,
                    book.Title,
                    book.PublishingHouse,
                    book.Tags.Select(x => new object[]{ x }),
                },
                QueryExactPhrase = new object[]
                {
                    book.Isbn,
                    book.Author.ToLower(),
                    book.Genre.ToLower(),
                    book.Title.ToLower(),
                    book.PublishingHouse.ToLower(),
                    book.Tags.Select(x => new object[]{ x.ToLower() })
                }
            };
        
        Index(x => x.Query, FieldIndexing.Search);
        Analyzers.Add(x => x.QueryExactPhrase, "KeywordAnalyzer");
    }
}