﻿using System.Data.Entity;
using PharmaNet.Fulfillment.Domain;
using PharmaNet.Infrastructure.Repository;

namespace PharmaNet.Fulfillment.SQL
{
    public class FulfillmentDB : DbContext
    {
        public static void Initialize()
        {
            Database.SetInitializer<FulfillmentDB>(
                new DropCreateDatabaseIfModelChanges<FulfillmentDB>());
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Product> Products { get; set; }

        public IRepository<Customer> GetCustomerRepository()
        {
            return new SQLRepository<Customer>(this, Customers);
        }

        public IRepository<Warehouse> GetWarehouseRepository()
        {
            return new SQLRepository<Warehouse>(this, Warehouses);
        }

        public IRepository<Product> GetProductRepository()
        {
            return new SQLRepository<Product>(this, Products);
        }
    }
}
