using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PharmaNet.Infrastructure.Messaging;
using PharmaNet.Fulfillment.Messages;
using PharmaNet.Sales.SQL;
using PharmaNet.Sales.Domain;
using PharmaNet.Infrastructure.Repository;

namespace PharmaNet.Sales.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SalesDB context = new SalesDB())
            {
                EnsureMember(context.GetMemberRepository());
                Rebate rebate = EnsureRebate(context.GetRebateRepository());
                EnsureProduct(rebate, context.GetProductRepository());
            }

            Console.WriteLine("Subscribing to events...");

            var queue = new MsmqMessageQueueOutbound<Subscription>(
                ".",
                typeof(OrderShipped).FullName);
            queue.Send(new Subscription
            {
                Target = Environment.MachineName,
                QueueName = typeof(OrderShippedSubscriber).FullName
            });

            Console.WriteLine("Starting order shipped subscriber...");

            MessageProcessor<OrderShipped> processor =
                new MessageProcessor<OrderShipped>(
                    typeof(OrderShippedSubscriber).FullName,
                    () => new OrderShippedSubscriber());
            processor.Start();

            Console.ReadKey();

            processor.Stop();
        }

        private static void EnsureMember(IRepository<Member> memberRepository)
        {
            if (!memberRepository.GetAll().Any(m => m.Name == "Sherlock Holmes"))
            {
                memberRepository.Add(new Member
                {
                    Name = "Sherlock Holmes"
                });
                memberRepository.SaveChanges();
            }
        }

        private static Rebate EnsureRebate(IRepository<Rebate> rebateRepository)
        {
            Rebate rebate = rebateRepository.GetAll().FirstOrDefault(r => r.Name == "Procrit Rebate");
            if (rebate == null)
            {
                rebate = rebateRepository.Add(new Rebate
                {
                    Name = "Procrit Rebate"
                });
                rebateRepository.SaveChanges();
            }
            return rebate;
        }

        private static void EnsureProduct(Rebate rebate, IRepository<Product> productRepository)
        {
            if (!productRepository.GetAll().Any(p => p.ProductNumber == 11190))
            {
                productRepository.Add(new Product
                {
                    ProductNumber = 11190,
                    Name = "Procrit",
                    Rebate = rebate
                });
                productRepository.SaveChanges();
            }
        }
    }
}
