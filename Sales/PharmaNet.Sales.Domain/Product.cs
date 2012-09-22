using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Sales.Domain
{
    public class Product
    {
        public Rebate Rebate { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
