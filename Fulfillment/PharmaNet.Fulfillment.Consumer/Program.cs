using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PharmaNet.Fulfillment.Contract;
using PharmaNet.Infrastructure.Service;

namespace PharmaNet.Fulfillment.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceClient<IFulfillmentService> client = new ServiceClient<IFulfillmentService>();
            client.CallService(s => s.PlaceOrder(new Order
            {
                CustomerName = "Sherlock Holmes",
                CustomerAddress = "121B Baker Street",
                Lines = new List<Line>
                {
                    new Line { ProductId = 11190, Quantity = 2 }
                }
            }));
        }
    }
}
