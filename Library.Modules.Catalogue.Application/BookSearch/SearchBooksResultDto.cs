namespace Library.Modules.Catalogue.Application.BookSearch;

public class SearchBooksResultDto
{
    public List<BookDto> Books { get; set; }
    public int TotalResults { get; set; }    
}