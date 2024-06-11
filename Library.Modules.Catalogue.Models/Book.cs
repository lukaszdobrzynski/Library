namespace Library.Modules.Catalogue.Models;

public class Book
{
    public string Id { get; set; }
    public Guid LibraryBranchId { get; set; }
    public string Isbn { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime PublicationDate { get; set; }
    public string PublishingHouse { get; set; }
    public string Edition { get; set; }
    public string Genre { get; set; }
    public string Description { get; set; }
    public BookStatus Status { get; set; }
    public DateTime? DueDate { get; set; }
    public List<string> Tags { get; set; }
}

