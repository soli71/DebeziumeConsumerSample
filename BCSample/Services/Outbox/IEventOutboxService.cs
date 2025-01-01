using BCSample.Events;

namespace BCSample.Services.Outbox
{
    public interface IEventOutboxService
    {
        Task SaveEventToOutboxAsync(IEvent eventData);
    }
}