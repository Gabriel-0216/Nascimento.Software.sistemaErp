using Domain.Orders;
namespace Infra.DAL.Contracts
{
    public interface IOrderDAL
    {
        Task<Order?> GetOrderByOrderId(int id);
        Task<bool> PlaceOrder(PlaceOrder order);
        Task<bool> UpdateOrder(Order order);
        Task<IEnumerable<Order>> GetAllOrders();
        Task<IEnumerable<Order>> GetOrdersByBuyerId(int buyerId);

    }
}
