using Dapper;
using Domain.Buyers;
using Domain.Orders;
using Infra.DAL.Contracts;
using System.Data.SqlClient;

namespace Infra.DAL.Repository
{
    public class OrderDAL : IOrderDAL, IConnection
    {
        private readonly SqlConnection _conn;
        public OrderDAL(SqlConnection conn)
        {
            _conn = conn;
            _conn.ConnectionString = GetConnectionString();
        }
        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            var query = @"Select [ORDER].*, [Buyer].*, [Product].*, [Order_Product].ProductQuantity
                                    FROM [Order]
                            INNER JOIN [Buyer] ON [Order].[BuyerId] = [Buyer].Id
                            LEFT JOIN [Order_Product] on [Order].Id = [Order_Product].OrderId
                            INNER JOIN [Product] ON [Product].[Id] = [Order_Product].[ProductId]";
            var list = new List<Order>();
            var exec = await _conn.QueryAsync<Order, Buyer, ProductOrder, Order>(query, (order, buyer, product) =>
            {
                var ordr = list.FirstOrDefault(p => p.Id == order.Id);
                if (ordr == null)
                {
                    ordr = order;
                    ordr.Buyer = buyer;
                    ordr.Products.Add(product);
                    list.Add(ordr);
                }
                else
                {
                    ordr.Buyer = buyer;
                    ordr.Products.Add(product);
                }
                return order;
            }, splitOn: "Id");
            return exec.Where(p => p.Buyer != null).ToList();
        }
        public string GetConnectionString() => Settings.ConnectionString;
        public Task<Order?> GetOrderByOrderId(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<Order>> GetOrdersByBuyerId(int buyerId)
        {
            var query = @"Select [ORDER].*, [Buyer].*, [Product].*, [Order_Product].ProductQuantity
                                    FROM [Order]
                            INNER JOIN [Buyer] ON [Order].[BuyerId] = [Buyer].Id
                            LEFT JOIN [Order_Product] on [Order].Id = [Order_Product].OrderId
                            INNER JOIN [Product] ON [Product].[Id] = [Order_Product].[ProductId]
                                            where [Buyer].[Id] = @buyerId";
            var param = new DynamicParameters();
            param.Add("buyerId", buyerId);
            var list = new List<Order>();
            var exec = await _conn.QueryAsync<Order, Buyer, ProductOrder, Order>(query, (order, buyer, product) =>
            {
                var ordr = list.FirstOrDefault(p => p.Id == order.Id);
                if (ordr == null)
                {
                    ordr = order;
                    ordr.Buyer = buyer;
                    ordr.Products.Add(product);
                    list.Add(ordr);
                }
                else
                {
                    ordr.Buyer = buyer;
                    ordr.Products.Add(product);
                }
                return order;
            }, splitOn: "Id", param: param);
            return exec.Where(p => p.Buyer != null).ToList();
        }
        public async Task<bool> PlaceOrder(PlaceOrder order)
        {
            var orderId = 0;
            await _conn.OpenAsync();
            var transaction = await _conn.BeginTransactionAsync();

            var query = @"Insert into [Order](BuyerId, TotalValue, IsCompleted) 
                            OUTPUT Inserted.Id
                       VALUES 
                    (@buyerId, @TotalValue, @IsCompleted)";
            var param = new DynamicParameters();
            param.Add("buyerId", order.BuyerId);
            param.Add("TotalValue", order.TotalValue);
            param.Add("IsCompleted", order.IsCompleted);

            try
            {
                orderId = await _conn.ExecuteScalarAsync<int>(query, param: param, transaction: transaction);
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
            if (await PlaceProductsOrder(order.Products, orderId, _conn, transaction))
            {
                transaction.Commit();
                return true;
            }
            await _conn.CloseAsync();
            return true;
        }
        private async Task<bool> PlaceProductsOrder(IEnumerable<Place_Order_Product> products, int orderId, SqlConnection _conn, System.Data.Common.DbTransaction _transaction)
        {
            try
            {
                var query_02 = @"Insert into [Order_Product]([OrderId], [ProductId], [ProductQuantity])
                            VALUES 
                        (@OrderId, @ProductId, @Quantity)";
                var param_02 = new DynamicParameters();
                param_02.Add("OrderId", orderId);
                foreach (var item in products)
                {
                    param_02.Add("ProductId", item.Id);
                    param_02.Add("Quantity", item.Product_Quantity);
                    await _conn.ExecuteAsync(query_02, param: param_02, transaction: _transaction);
                }
            }
            catch (Exception)
            {
                _transaction.Rollback();
                return false;
            }
            return true;
        }
        public Task<bool> UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
