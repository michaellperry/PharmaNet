using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaNet.Fulfillment.Handler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting order processor...");

            OrderProcessor orderProcessor =
                new OrderProcessor();
            orderProcessor.Start();

            Console.ReadKey();

            orderProcessor.Stop();
        }
    }
}
