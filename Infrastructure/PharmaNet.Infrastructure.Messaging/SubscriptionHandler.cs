using System;

namespace PharmaNet.Infrastructure.Messaging
{
    public class SubscriptionHandler<T> : IMessageHandler<Subscription>
    {
        private readonly SubscriberRegistry<T> _subscriberRegistry;

        public SubscriptionHandler(SubscriberRegistry<T> subscriberRegistry)
        {
            _subscriberRegistry = subscriberRegistry;
        }

        public void HandleMessage(Subscription subscription)
        {
            Console.WriteLine("Adding subscription {0}: {1}.",
                subscription.Target,
                subscription.QueueName);
            _subscriberRegistry.Add(subscription);
        }

        public void Dispose()
        {
        }
    }
}
