using Library.BuildingBlocks.Domain;
using Library.Modules.Lending.Domain.Book;
using Library.Modules.Lending.Domain.LibraryBranch;
using Library.Modules.Lending.Domain.Patron;

namespace Library.Modules.Lending.Domain.Events;

public class BookPlacedOnHoldDomainEvent(BookId bookId, PatronId patronId, LibraryBranchId libraryBranchId)
    : DomainEventBase
{
    public BookId BookId { get; set; } = bookId;
    public PatronId PatronId { get; set; } = patronId;
    public LibraryBranchId LibraryBranchId { get; set; } = libraryBranchId;
}