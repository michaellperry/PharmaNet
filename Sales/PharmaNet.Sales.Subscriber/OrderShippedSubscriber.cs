using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PharmaNet.Infrastructure.Messaging;
using PharmaNet.Infrastructure.Repository;
using PharmaNet.Sales.Domain;
using PharmaNet.Sales.Application;
using PharmaNet.Sales.SQL;
using PharmaNet.Fulfillment.Messages;

namespace PharmaNet.Sales.Subscriber
{
    public class OrderShippedSubscriber :
        IMessageHandler<OrderShipped>
    {
        private SalesDB _context;
        private SalesHistoryService _salesHistoryService;

        public OrderShippedSubscriber()
        {
            _context = new SalesDB();
            
            _salesHistoryService = new SalesHistoryService(
                _context.GetMemberRepository(),
                _context.GetMeasurementPeriodRepository(),
                _context.GetProductRepository(),
                _context.GetSalesHistoryRepository()
                );
        }

        public void HandleMessage(OrderShipped message)
        {
            Console.WriteLine("Order shipped.");

            foreach (var shipment in message.Shipments)
            {
                _salesHistoryService.RecordSale(
                    message.CustomerName,
                    message.OrderDate,
                    shipment.ProductNumber,
                    shipment.Quantity);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
