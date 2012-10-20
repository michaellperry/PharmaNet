using System.Collections.Generic;

namespace PharmaNet.Infrastructure.Messaging
{
    public class SubscriberRegistry<T>
    {
        private HashSet<Subscription> _subscriptions =
            new HashSet<Subscription>();

        public void Add(Subscription subscription)
        {
            _subscriptions.Add(subscription);
        }

        public void Publish(T @event)
        {
            foreach (var subscription in _subscriptions)
            {
                var queue = new MsmqMessageQueueOutbound<T>(
                    subscription.Target,
                    subscription.QueueName);
                queue.Send(@event);
            }
        }
    }
}
