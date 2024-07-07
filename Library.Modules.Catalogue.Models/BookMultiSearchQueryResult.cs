namespace Library.Modules.Catalogue.Models;

public class BookMultiSearchQueryResult
{
    public List<Book> Books { get; set; }
    public int TotalResults { get; set; }
}