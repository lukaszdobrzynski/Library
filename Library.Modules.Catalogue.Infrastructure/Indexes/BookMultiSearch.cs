using Library.Modules.Catalogue.Models;
using Raven.Client.Documents.Indexes;

namespace Library.Modules.Catalogue.Infrastructure.Indexes;

public class BookMultiSearch : AbstractIndexCreationTask<Book, BookMultiSearch.Result>
{
    public class Result
    {
        public string Query { get; set; }
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
                }
            };
        
        Index(x => x.Query, FieldIndexing.Search);
    }
}