namespace Library.Modules.Catalogue.Application.BookSearch;

public class SearchAllBooksResultDto
{
    public List<BookDto> Books { get; set; }
    public int TotalResults { get; set; }    
}