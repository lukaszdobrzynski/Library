using Library.Modules.Catalogue.Application.Contracts;
using Raven.Client.Documents.Subscriptions;

namespace Library.Modules.Catalogue.Infrastructure.Configuration.Processing;

internal class InternalCommandsSubscriptionProcessor : ISubscriptionProcessor
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
            Strategy = SubscriptionOpeningStrategy.Concurrent
        };
        
        var subscriptionWorkerOne = _documentStoreHolder.GetSubscriptionWorker<InternalCommandBase>(
            subscriptionWorkerOptions);
        var subscriptionWorkerTwo = _documentStoreHolder.GetSubscriptionWorker<InternalCommandBase>(
            subscriptionWorkerOptions);
        
        var subscriptionTaskOne = subscriptionWorkerOne.Run(ProcessInternalCommandBatch);
        var subscriptionTaskTwo = subscriptionWorkerTwo.Run(ProcessInternalCommandBatch);
        
        await Task.WhenAll(subscriptionTaskOne, subscriptionTaskTwo);
    }
    
    private async Task ProcessInternalCommandBatch(SubscriptionBatch<InternalCommandBase> batch)
    {
        foreach (var item in batch.Items)
        {
            await _internalCommandHandlingStrategy.Handle(item.Result);
        }
    }
}