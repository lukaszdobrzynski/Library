using Library.Modules.Catalogue.Models;

namespace Library.Modules.Catalogue.Application.BookSearch;

public class BookDto
{
    public string Id { get; set; }
    public string LibraryBranchId { get; set; }
    public string Isbn { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string PublicationDate { get; set; }
    public string PublishingHouse { get; set; }
    public string Edition { get; set; }
    public string Genre { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string DueDate { get; set; }
    public List<string> Tags { get; set; }

    public static BookDto From(Book book) => new()
    {
        Id = book.Id,
        LibraryBranchId = book.LibraryBranchId.ToString(),
        Isbn = book.Isbn,
        Title = book.Title,
        Author = book.Author,
        PublicationDate = book.PublicationDate.ToString("yyyy-MM-dd"),
        PublishingHouse = book.PublishingHouse,
        Description = book.Description,
        Edition = book.Edition,
        Genre = book.Edition,
        Status = book.Status.ToString(),
        Tags = book.Tags,
        DueDate = book.DueDate?.ToString("yyyy-MM-dd")
    };
}