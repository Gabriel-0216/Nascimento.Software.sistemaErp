using Dapper.Contrib.Extensions;

namespace Domain.Products
{
    [Table("Product")]
    public class Product : Entity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public int CategoryId { get; set; }
        [Write(false)]
        public Category? Category { get; set; }
        public bool IsActive { get; set; }
    }
}
