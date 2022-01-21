using Dapper.Contrib.Extensions;
namespace Domain.Products
{
    [Table("[Category]")]

    public class Category : Entity
    {
        public Category()
        {
            Products = new List<Product>();
        }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        [Write(false)]
        public List<Product> Products { get; set; }
    }
}