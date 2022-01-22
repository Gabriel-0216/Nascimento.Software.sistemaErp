using Dapper.Contrib.Extensions;
using Domain.Products;
namespace Domain.Orders
{
    public class ProductOrder
    {
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public int CategoryId { get; set; }
        [Write(false)]
        public Category? Category { get; set; }
        public bool IsActive { get; set; }
        public int ProductQuantity { get; set; }

    }
}
