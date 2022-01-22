namespace Domain.Orders
{
    public class PlaceOrder
    {
        public PlaceOrder() => Products = new List<Place_Order_Product>();
        public int BuyerId { get; set; }
        public decimal TotalValue { get; set; }
        public bool IsCompleted { get; set; } = false;
        public List<Place_Order_Product> Products { get; set; }
    }
}
