using Library.BuildingBlocks.Domain;
using Library.Modules.Lending.Domain.Book;
using Library.Modules.Lending.Domain.LibraryBranch;
using Library.Modules.Lending.Domain.Patron;

namespace Library.Modules.Lending.Domain.Events;

public class BookHoldCanceledDomainEvent : DomainEventBase
{
    public BookId BookId { get; set; }
    public PatronId PatronId { get; set; }
    public LibraryBranchId LibraryBranchId { get; set; }

    public BookHoldCanceledDomainEvent(BookId bookId, PatronId patronId, LibraryBranchId libraryBranchId)
    {
        BookId = bookId;
        PatronId = patronId;
        LibraryBranchId = libraryBranchId;
    }
}