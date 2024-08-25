using System.Threading.Tasks;
using Library.Modules.Catalogue.Application.PlaceBookOnHold;
using Library.Modules.Catalogue.Infrastructure.Configuration;
using Library.Modules.Catalogue.Models;
using NUnit.Framework;

namespace Library.Modules.Catalogue.IntegrationTests.RequestHold;

[TestFixture]
public class RequestHoldTests : TestBase
{
    [Test]
    public async Task Succeeds_WhenBookIsAvailable()
    {
        var book = new Book { Id = SomeBookId.ToString(), Status = BookStatus.Available };

        await StoreAsync(book);

        var command = RequestHoldCommand.CreateSubmitted(
            SomeBookId,
            SomeLibraryBranchId,
            SomePatronId,
            HoldTillWeekFromNow,
            SomeExternalHoldRequestId);

        await Act(command);

        var notification = await GetSingleOutboxMessage<BookHoldGrantedNotification>();
        Assert.That(notification.BookId, Is.EqualTo(SomeBookId));

        var bookOnHold = await LoadAsync<Book>(book.Id);
        Assert.That(bookOnHold.Status, Is.EqualTo(BookStatus.OnHold));
    }

    [Test]
    public async Task Succeeds_WhenBookIsOnHold()
    {
        var book = new Book { Id = SomeBookId.ToString(), Status = BookStatus.OnHold };

        await StoreAsync(book);

        var command = RequestHoldCommand.CreateSubmitted(
            SomeBookId,
            SomeLibraryBranchId,
            SomePatronId,
            HoldTillWeekFromNow,
            SomeExternalHoldRequestId);

        await Act(command);

        var notification = await GetSingleOutboxMessage<BookHoldRejectedNotification>();
        Assert.That(notification.BookId, Is.EqualTo(SomeBookId));

        var bookOnHold = await LoadAsync<Book>(book.Id);
        Assert.That(bookOnHold.Status, Is.EqualTo(BookStatus.OnHold));
    }
    
    [Test]
    public async Task Succeeds_WhenBookIsCheckedOut()
    {
        var book = new Book { Id = SomeBookId.ToString(), Status = BookStatus.CheckedOut };

        await StoreAsync(book);

        var command = RequestHoldCommand.CreateSubmitted(
            SomeBookId,
            SomeLibraryBranchId,
            SomePatronId,
            HoldTillWeekFromNow,
            SomeExternalHoldRequestId);

        await Act(command);

        var notification = await GetSingleOutboxMessage<BookHoldRejectedNotification>();
        Assert.That(notification.BookId, Is.EqualTo(SomeBookId));

        var bookOnHold = await LoadAsync<Book>(book.Id);
        Assert.That(bookOnHold.Status, Is.EqualTo(BookStatus.CheckedOut));
        WaitForUserToContinueTheTest();
    }
    
    private Task Act(RequestHoldCommand command)
    {
        var commandHandler = new RequestHoldCommandHandler(DocumentStoreHolder, new DomainNotificationsRegistry());
        return commandHandler.Handle(command);
    }
}