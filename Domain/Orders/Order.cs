using Dapper.Contrib.Extensions;
using Domain.Buyers;

namespace Domain.Orders
{
    [Table("[Order]")]

    public class Order : Entity
    {
        public Order() => Products = new List<ProductOrder>();
        public int BuyerId { get; set; }
        [Write(false)]
        public Buyer? Buyer { get; set; }
        public decimal TotalValue { get; set; }
        public bool IsCompleted { get; set; }
        [Write(false)]
        public List<ProductOrder> Products { get; set; }
    }
}
