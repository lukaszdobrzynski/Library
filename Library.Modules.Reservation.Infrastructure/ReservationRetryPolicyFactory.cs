using Library.BuildingBlocks.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;

namespace Library.Modules.Reservation.Infrastructure;

internal class ReservationRetryPolicyFactory : RetryPolicyFactory
{
    public override AsyncRetryPolicy RetryOnDbUpdateConcurrencyException(int retryCount)
    {
        var policy = Policy
            .Handle<Exception>(e => e is DbUpdateConcurrencyException)
            .WaitAndRetryAsync(1, (_) => TimeSpan.FromMilliseconds(1500));
            
        return policy;
    }
}