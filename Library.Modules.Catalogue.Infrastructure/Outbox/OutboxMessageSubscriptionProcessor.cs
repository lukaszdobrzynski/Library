using Library.Modules.Catalogue.Application.Contracts;
using Library.Modules.Catalogue.Models;
using Raven.Client.Documents.Subscriptions;

namespace Library.Modules.Catalogue.Infrastructure.Outbox;

public class OutboxMessageSubscriptionProcessor : ISubscriptionProcessor
{
    private readonly IDocumentStoreHolder _documentStoreHolder;
    private readonly OutboxMessageHandlingStrategy _outboxMessageHandlingStrategy;

    private const string SubscriptionName = OutboxSubscription.SubscriptionName;
    private const int BatchSize = 5;

    public OutboxMessageSubscriptionProcessor(IDocumentStoreHolder documentStoreHolder, OutboxMessageHandlingStrategy outboxMessageHandlingStrategy)
    {
        _outboxMessageHandlingStrategy = outboxMessageHandlingStrategy;
        _documentStoreHolder = documentStoreHolder;
    }
    
    public async Task Run()
    {
        var subscriptionWorkerOption = new SubscriptionWorkerOptions(SubscriptionName)
        {
            MaxDocsPerBatch = BatchSize,
            Strategy = SubscriptionOpeningStrategy.TakeOver
        };

        var subscription = _documentStoreHolder.GetSubscriptionWorker<OutboxMessage>(subscriptionWorkerOption);

        var subscriptionTask = subscription.Run(ProcessOutboxMessageBatch);
        await subscriptionTask;
    }

    private async Task ProcessOutboxMessageBatch(SubscriptionBatch<OutboxMessage> batch)
    {
        foreach (var item in batch.Items)
        {
            await _outboxMessageHandlingStrategy.Handle(item.Result);
        }
    }
}