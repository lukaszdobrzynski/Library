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
        public DateTime PublicationDate { get; set; }
        public string PublishingHouse { get; set; }
        public string AuthorQuery { get; set; }
        public string TitleQuery { get; set; }
        public string PublishingHouseQuery { get; set; }
        public string AnyTermQuery { get; set; }
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
                book.PublicationDate,
                book.PublishingHouse,
                AuthorQuery = book.Author,
                TitleQuery = book.Title,
                PublishingHouseQuery = book.PublishingHouse,
                AnyTermQuery = queryData,
                ExactQuery = queryData 
            };
        
        Index(x => x.AnyTermQuery, FieldIndexing.Search);
        Index(x => x.AuthorQuery, FieldIndexing.Search);
        Index(x => x.TitleQuery, FieldIndexing.Search);
        Index(x => x.PublishingHouseQuery, FieldIndexing.Search);
    }
}