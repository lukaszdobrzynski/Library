using Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;
using Library.Modules.Catalogue.Infrastructure.Subscriptions;
using Raven.Client.Documents.Subscriptions;

namespace Library.Modules.Catalogue.Infrastructure.Inbox;

public class InboxSubscriptionProcessor : ISubscriptionProcessor
{
    private const string SubscriptionName = InboxSubscription.SubscriptionName;
    private const int BatchSize = 5;
    
    private readonly IDocumentStoreHolder _documentStoreHolder;
    private readonly InboxMessageHandlingStrategy _inboxMessageHandlingStrategy;
    
    public InboxSubscriptionProcessor(IDocumentStoreHolder documentStoreHolder, InboxMessageHandlingStrategy inboxMessageHandlingStrategy)
    {
        _documentStoreHolder = documentStoreHolder;
        _inboxMessageHandlingStrategy = inboxMessageHandlingStrategy;
    }
    
    public async Task Run()
    {
        var subscriptionWorkerOptions = new SubscriptionWorkerOptions(SubscriptionName)
        {
            MaxDocsPerBatch = BatchSize,
            Strategy = SubscriptionOpeningStrategy.TakeOver
        };
        
        var subscription = _documentStoreHolder.GetSubscriptionWorker<InboxMessage>(
            subscriptionWorkerOptions);
        
        var subscriptionTask = subscription.Run(ProcessInboxMessageBatch);
        await subscriptionTask;
    }

    private async Task ProcessInboxMessageBatch(SubscriptionBatch<InboxMessage> batch)
    {
        foreach (var item in batch.Items)
        {
           await _inboxMessageHandlingStrategy.Handle(item.Result);
        }
    }
}