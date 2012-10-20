using System;

namespace PharmaNet.Infrastructure.Messaging
{
    public class Subscription
    {
        public string Target { get; set; }
        public string QueueName { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            Subscription that = obj as Subscription;
            if (that == null)
                return false;
            return
                Object.Equals(this.Target, that.Target) &&
                Object.Equals(this.QueueName, that.QueueName);
        }

        public override int GetHashCode()
        {
            return
                Target.GetHashCode() * 37 +
                QueueName.GetHashCode();
        }
    }
}
