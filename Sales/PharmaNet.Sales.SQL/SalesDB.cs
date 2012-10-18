using System.Data.Entity;
using PharmaNet.Infrastructure.Repository;
using PharmaNet.Infrastructure.SQL;
using PharmaNet.Sales.Domain;

namespace PharmaNet.Sales.SQL
{
    public class SalesDB : DbContext
    {
        public static void Initialize()
        {
            Database.SetInitializer<SalesDB>(
                new DropCreateDatabaseIfModelChanges<
                    SalesDB>());
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<MeasurementPeriod> MeasurementPeriods { get; set; }
        public DbSet<Rebate> Rebates { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SalesHistory> SalesHistories { get; set; }

        public IRepository<Member> GetMemberRepository()
        {
            return new SQLRepository<Member>(this, Members);
        }

        public IRepository<MeasurementPeriod> GetMeasurementPeriodRepository()
        {
            return new SQLRepository<MeasurementPeriod>(this, MeasurementPeriods);
        }

        public IRepository<Rebate> GetRebateRepository()
        {
            return new SQLRepository<Rebate>(this, Rebates);
        }

        public IRepository<Product> GetProductRepository()
        {
            return new SQLRepository<Product>(this, Products);
        }

        public IRepository<SalesHistory> GetSalesHistoryRepository()
        {
            return new SQLRepository<SalesHistory>(this, SalesHistories);
        }
    }
}
