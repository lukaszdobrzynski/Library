using System;
using System.Reflection;
using System.Threading.Tasks;
using Library.BuildingBlocks.IntegrationTests;
using Library.Modules.Catalogue.Application.Contracts;
using Library.Modules.Catalogue.Infrastructure;
using Library.Modules.Catalogue.Infrastructure.Configuration;
using Library.Modules.Catalogue.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using Raven.Client.Documents;

namespace Library.Modules.Catalogue.IntegrationTests;

public class TestBase : RavenTestBase
{
    protected Guid SomeBookId => Guid.Parse("ae0236d1-badc-4cb1-a5e4-e4137f2b1942");
    protected Guid SomePatronId => Guid.Parse("60f21521-529f-4fae-8420-add4d067bf20");
    protected Guid SomeLibraryBranchId => Guid.Parse("a6b22b9c-52e1-438d-982d-668bcdb0821e");
    protected Guid SomeExternalHoldRequestId => Guid.Parse("8b2674a5-0bf6-4609-b539-0f415b1b6b9b");
    protected DateTime HoldTillWeekFromNow => DateTime.UtcNow.AddDays(7);
    
    [SetUp]
    protected override void BeforeEachTest()
    {
        base.BeforeEachTest();
        CatalogueStartup.Init(
            DocumentStoreHolder,
            new MockExecutionContextAccessor(),
            new MockLogger(),
            new MockEventBus());
    }

    protected CatalogueModule CatalogueModule => new();

    protected async Task StoreAsync<T>(T entity)
    {
        using (var session = OpenAsyncSession())
        {
            await session.StoreAsync(entity);
            await session.SaveChangesAsync();
        }
    }

    protected async Task<T> LoadSingleAsync<T>()
    {
        using (var session = OpenAsyncSession())
        {
             var document = await session.Query<T>().SingleAsync();
             return document;
        }
    }

    protected async Task<T> LoadAsync<T>(string id)
    {
        using (var session = OpenAsyncSession())
        {
            var document = await session.LoadAsync<T>(id);
            Assert.That(document, Is.Not.Null);
            return document;
        }
    }
    
    protected async Task AssertExistsInDb(string id)
    {
        using (var session = OpenAsyncSession())
        {
            var exists = await session.Advanced.ExistsAsync(id);
            Assert.That(exists, Is.True);
        }
    }

    protected async Task AssertDoesNotExistInDb(string id)
    {
        using (var session = OpenAsyncSession())
        {
            var exists = await session.Advanced.ExistsAsync(id);
            Assert.That(exists, Is.False);
        }
    }
    
    protected async Task AssertSingleDocumentInDb<T>()
    {
        using (var session = OpenAsyncSession())
        {
            var documents = await session.Query<T>().ToListAsync();
            Assert.That(documents, Has.Exactly(1).Items);
        }
    }
    
    protected async Task<T> GetSingleOutboxMessage<T>()
        where T : class, IDomainNotification
    {
        var message = await LoadSingleAsync<OutboxMessage>();
        var type = Assembly.GetAssembly(typeof(T)).GetType(typeof(T).FullName);
        return JsonConvert.DeserializeObject(message.Data, type) as T;
    }
}