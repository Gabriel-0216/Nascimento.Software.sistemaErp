namespace Nascimento.Software.Erp.Api.Dto.Products
{
    public class ProductDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
    }
}