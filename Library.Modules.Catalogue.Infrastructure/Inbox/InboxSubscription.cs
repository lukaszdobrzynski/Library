using Raven.Client.Documents;
using Raven.Client.Documents.Subscriptions;
using Raven.Client.Exceptions.Documents.Subscriptions;

namespace Library.Modules.Catalogue.Infrastructure.Inbox;

public class InboxSubscription
{
    public const string SubscriptionName = $"{nameof(InboxSubscription)}";
    
    private const string SubscriptionQuery = "from InboxMessages where Status == 'Submitted'";
    
    public static void Create(IDocumentStore store)
    {
        try
        {
            var subscriptionState = store.Subscriptions.GetSubscriptionState(SubscriptionName);
            if (subscriptionState.Query == SubscriptionQuery) 
                return;
            
            store.Subscriptions.Delete(SubscriptionName); 
            store.Subscriptions.Create(new SubscriptionCreationOptions
            {
                Name = SubscriptionName,
                Query = SubscriptionQuery,
                ChangeVector = subscriptionState.ChangeVectorForNextBatchStartingPoint,
                MentorNode = subscriptionState.MentorNode
            });
        }
        catch (SubscriptionDoesNotExistException)
        {
            store.Subscriptions.Create(new SubscriptionCreationOptions
            {
                Name = SubscriptionName,
                Query = SubscriptionQuery
            });
        }
    }
}