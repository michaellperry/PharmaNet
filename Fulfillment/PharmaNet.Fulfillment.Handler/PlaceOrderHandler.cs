using System;
using System.Collections.Generic;
using System.Linq;
using PharmaNet.Fulfillment.Application;
using PharmaNet.Fulfillment.Domain;
using PharmaNet.Fulfillment.Messages;
using PharmaNet.Fulfillment.SQL;

namespace PharmaNet.Fulfillment.Handler
{
    public class PlaceOrderHandler :
        IMessageHandler<PlaceOrder>
    {
        public void HandleMessage(PlaceOrder message)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
