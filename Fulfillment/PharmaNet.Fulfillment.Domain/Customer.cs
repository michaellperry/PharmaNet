
namespace PharmaNet.Fulfillment.Domain
{
    public class Customer
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string ShippingAddress { get; set; }
    }
}
