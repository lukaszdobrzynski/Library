using Library.Modules.Catalogue.Application.Contracts;
using Raven.Client.Documents.Subscriptions;

namespace Library.Modules.Catalogue.Infrastructure.Configuration.Processing;

public class InternalCommandsSubscriptionProcessor : ISubscriptionProcessor
{
    private const string SubscriptionName = InternalCommandsSubscription.SubscriptionName;
    private const int BatchSize = 5;

    private readonly IDocumentStoreHolder _documentStoreHolder;
    private readonly InternalCommandHandlingStrategy _internalCommandHandlingStrategy;

    public InternalCommandsSubscriptionProcessor(IDocumentStoreHolder documentStoreHolder, 
        InternalCommandHandlingStrategy internalCommandHandlingStrategy)
    {
        _documentStoreHolder = documentStoreHolder;
        _internalCommandHandlingStrategy = internalCommandHandlingStrategy;
    }
    
    public async Task Run()
    {
        var subscriptionWorkerOptions = new SubscriptionWorkerOptions(SubscriptionName)
        {
            MaxDocsPerBatch = BatchSize,
            Strategy = SubscriptionOpeningStrategy.TakeOver
        };
        
        var subscription = _documentStoreHolder.GetSubscriptionWorker<InternalCommandBase>(
            subscriptionWorkerOptions);
        
        var subscriptionTask = subscription.Run(ProcessInboxMessageBatch);
        await subscriptionTask;
    }
    
    private async Task ProcessInboxMessageBatch(SubscriptionBatch<InternalCommandBase> batch)
    {
        foreach (var item in batch.Items)
        {
            await _internalCommandHandlingStrategy.Handle(item.Result);
        }
    }
}