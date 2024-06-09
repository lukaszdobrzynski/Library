using Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;
using Library.Modules.Catalogue.Infrastructure.Subscriptions;
using MediatR;
using Raven.Client.Documents.Subscriptions;

namespace Library.Modules.Catalogue.Infrastructure.Inbox;

public class InboxSubscriptionProcessor : ISubscriptionProcessor
{
    private const string SubscriptionName = InboxSubscription.SubscriptionName;
    private const int BatchSize = 5;
    
    private readonly IDocumentStoreHolder _documentStoreHolder;
    private readonly IMediator _mediator;
    
    public InboxSubscriptionProcessor(IDocumentStoreHolder documentStoreHolder, IMediator mediator)
    {
        _documentStoreHolder = documentStoreHolder;
        _mediator = mediator;
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
            var messageAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => item.Result.Type.Contains(x.GetName().Name));

            var messageType = messageAssembly.GetType(item.Result.Type);

            await _mediator.Publish((INotification)messageType); //TODO arch unit test to make sure all integration events inherit from INotification
        }
    }
}