namespace Nascimento.Software.Erp.Api.Dto.Orders
{
    public class OrderDto
    {
        public OrderDto() => Products = new List<ProductOrderDto>();
        public int BuyerId { get; set; }
        public List<ProductOrderDto> Products { get; set; }
    }
}
