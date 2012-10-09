using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PharmaNet.Fulfillment.Contract;
using System.ServiceModel.MsmqIntegration;

namespace PharmaNet.Fulfillment.Presentation
{
    public class FulfillmentCommandService : IFulfillmentCommandService
    {
        public FulfillmentCommandService()
        {
            OrderHandler.Instance.Start();
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public void PlaceOrder(MsmqMessage<Order> message)
        {
            MessageQueue<Order>.Instance.Send(message.Body);
        }
    }
}
