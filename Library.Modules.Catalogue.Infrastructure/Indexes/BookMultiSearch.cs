using Library.Modules.Catalogue.Models;
using Raven.Client.Documents.Indexes;

namespace Library.Modules.Catalogue.Infrastructure.Indexes;

public class BookMultiSearch : AbstractIndexCreationTask<Book, BookMultiSearch.Result>
{
    public class Result
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string Isbn { get; set; }
        public string Query { get; set; }
        public string ExactQuery { get; set; }
    }

    public BookMultiSearch()
    {
        Map = books =>
            from book in books
            let queryData = new object[]
            {
                book.Isbn,
                book.Author,
                book.Genre,
                book.Title,
                book.PublishingHouse,
                book.Tags.Select(x => new object[]{ x }),
            }
            select new
            {
                book.Author,
                book.Title,
                book.Isbn,
                Query = queryData,
                ExactQuery = queryData 
            };
        
        Index(x => x.Query, FieldIndexing.Search);
    }
}