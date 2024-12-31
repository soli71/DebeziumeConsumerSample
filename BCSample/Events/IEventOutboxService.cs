
using BCSample.Partials;

namespace BCSample.Events
{
    public interface IEventOutboxService
    {
        Task SaveEventToOutboxAsync(LoginActionEvent eventData, string topic);
    }
}