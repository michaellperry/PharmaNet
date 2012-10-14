using PharmaNet.Fulfillment.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace PharmaNet.Fulfillment.Presentation
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            FulfillmentDB.Initialize();

            FulfillmentDB context = new FulfillmentDB();
            var product = EnsureProduct(context);
            EnsureWarehouse(context, product);

            OrderHandler.Instance.Start();
        }

        private static Domain.Product EnsureProduct(FulfillmentDB context)
        {
            var products = context.GetProductRepository();
            var product = products.GetAll()
                .FirstOrDefault(p => p.ProductNumber == 11190);

            if (product == null)
            {
                product = new Domain.Product { ProductNumber = 11190 };
                products.Add(product);
                products.SaveChanges();
            }

            return product;
        }

        private static void EnsureWarehouse(FulfillmentDB context, Domain.Product product)
        {
            var warehouses = context.GetWarehouseRepository();
            var warehouse = warehouses.GetAll()
                .FirstOrDefault();

            if (warehouse == null)
            {
                warehouse = new Domain.Warehouse { Name = "Hangar 18" };
                warehouse.Inventory = new List<Domain.Inventory>
                {
                    new Domain.Inventory { Product = product, QuantityOnHand = 1000000 }
                };
                warehouses.Add(warehouse);
                warehouses.SaveChanges();
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}