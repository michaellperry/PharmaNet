
namespace PharmaNet.Fulfillment.Domain
{
    public class OrderLine
    {
        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
        public virtual int Quantity { get; set; }
    }
}
