using Library.Modules.Catalogue.Application.Contracts;
using Library.Modules.Catalogue.Models;
using Raven.Client.Documents.Subscriptions;

namespace Library.Modules.Catalogue.Infrastructure.Inbox;

public class InboxMessagesSubscriptionProcessor : ISubscriptionProcessor
{
    private const string SubscriptionName = InboxSubscription.SubscriptionName;
    private const int BatchSize = 5;
    
    private readonly IDocumentStoreHolder _documentStoreHolder;
    private readonly InboxMessageHandlingStrategy _inboxMessageHandlingStrategy;
    
    public InboxMessagesSubscriptionProcessor(IDocumentStoreHolder documentStoreHolder, InboxMessageHandlingStrategy inboxMessageHandlingStrategy)
    {
        _documentStoreHolder = documentStoreHolder;
        _inboxMessageHandlingStrategy = inboxMessageHandlingStrategy;
    }
    
    public async Task Run()
    {
        var subscriptionWorkerOptions = new SubscriptionWorkerOptions(SubscriptionName)
        {
            MaxDocsPerBatch = BatchSize,
            Strategy = SubscriptionOpeningStrategy.Concurrent
        };
        
        var subscriptionOne = _documentStoreHolder.GetSubscriptionWorker<InboxMessage>(
            subscriptionWorkerOptions);
        var subscriptionTwo = _documentStoreHolder.GetSubscriptionWorker<InboxMessage>(
            subscriptionWorkerOptions);
        
        var subscriptionTaskOne = subscriptionOne.Run(ProcessInboxMessageBatch);
        var subscriptionTaskTwo = subscriptionTwo.Run(ProcessInboxMessageBatch);
        
        await Task.WhenAll(subscriptionTaskOne, subscriptionTaskTwo);
    }

    private async Task ProcessInboxMessageBatch(SubscriptionBatch<InboxMessage> batch)
    {
        foreach (var item in batch.Items)
        {
           await _inboxMessageHandlingStrategy.Handle(item.Result);
        }
    }
}