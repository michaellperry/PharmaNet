using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using PharmaNet.Fulfillment.Domain;
using PharmaNet.Fulfillment.Application;
using PharmaNet.Fulfillment.SQL;
using PharmaNet.Fulfillment.Contract;
using System.Transactions;

namespace PharmaNet.Fulfillment.Presentation
{
    public class OrderHandler
    {
        private static OrderHandler _instance =
            new OrderHandler();

        public static OrderHandler Instance
        {
            get { return _instance; }
        }

        private CustomerService _customerService;
        private ProductService _productService;
        private InventoryAllocationService _inventoryAllocationService;
        private PickListService _pickListService;
        private IMessageQueue<Order> _messageQueue;

        private ManualResetEvent _stop =
            new ManualResetEvent(false);
        private Thread _thread;

        public OrderHandler()
        {
            // TODO: Inject these dependencies.
            FulfillmentDB.Initialize();

            FulfillmentDB context = new FulfillmentDB();

            _customerService = new CustomerService(context.GetCustomerRepository());
            _productService = new ProductService(context.GetProductRepository());
            _inventoryAllocationService = new InventoryAllocationService(context.GetWarehouseRepository());
            _pickListService = new PickListService(context.GetPickListRepository());

            _messageQueue = MemoryMessageQueue<Order>.Instance;

            _thread = new Thread(ThreadProc);
            _thread.Name = "OrderHandler";
        }

        public void Start()
        {
            if (_thread.ThreadState ==
                ThreadState.Unstarted)
            {
                _thread.Start();
            }
        }

        public void Stop()
        {
            _stop.Set();
            _thread.Join();
        }

        private void ThreadProc(object o)
        {
            Order order;
            while (!_stop.WaitOne(0))
            {
                if (_messageQueue
                    .TryReceive(out order))
                {
                    ProcessOrder(order);
                }
            }
        }

        private void ProcessOrder(Order order)
        {
            using (var scope = new TransactionScope())
            {
                Customer customer = _customerService
                    .GetCustomer(
                        order.CustomerName,
                        order.CustomerAddress);

                List<OrderLine> orderLines = order.Lines
                    .Select(line => new OrderLine
                    {
                        Customer = customer,
                        Product = _productService
                            .GetProduct(
                                line.ProductNumber),
                        Quantity = line.Quantity
                    })
                    .ToList();

                List<PickList> pickLists =
                    _inventoryAllocationService
                        .AllocateInventory(
                            order.OrderId,
                            orderLines);

                _pickListService.SavePickLists(pickLists);

                scope.Complete();
            }
        }
    }
}