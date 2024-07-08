namespace Library.Modules.Catalogue.Models;

public class BookSearchQueryResult
{
    public List<Book> Books { get; set; }
    public int TotalResults { get; set; }
}