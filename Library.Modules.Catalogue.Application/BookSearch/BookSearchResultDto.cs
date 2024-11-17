namespace Library.Modules.Catalogue.Application.BookSearch;

public class BookSearchResultDto
{
    public List<BookDto> Books { get; set; }
    public int TotalResults { get; set; }    
}