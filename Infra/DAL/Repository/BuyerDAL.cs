using Dapper;
using Dapper.Contrib.Extensions;
using Domain.Buyers;
using Domain.Orders;
using Infra.DAL.Contracts;
using System.Data.SqlClient;

namespace Infra.DAL.Repository
{
    public class BuyerDAL : IBuyerDAL
    {
        private readonly SqlConnection _conn;
        public BuyerDAL(SqlConnection conn)
        {
            _conn = conn;
            _conn.ConnectionString = Settings.ConnectionString;
        }
        public async Task<bool> Create(Buyer entity)
        {
            var inserted = await _conn.InsertAsync<Buyer>(entity);
            return (inserted.GetType() == typeof(int));
        }
        public async Task<bool> Delete(Buyer entity) => await _conn.DeleteAsync<Buyer>(entity);
        public async Task<Buyer?> Get(int Id) => await _conn.GetAsync<Buyer>(Id);
        public async Task<IEnumerable<Buyer>> GetAll() => await _conn.GetAllAsync<Buyer>();
        public async Task<Buyer?> GetBuyerWithCellphone(string cellphone)
        {
            var query = @"SELECT [Buyer].* FROM [Buyer] WHERE CellPhone = @Cellphone";
            var buyer = await _conn.QueryFirstOrDefaultAsync<Buyer>(query, new { Cellphone = cellphone });
            await _conn.CloseAsync();
            return buyer;
        }
        public async Task<Buyer?> GetBuyerWithEmail(string email)
        {
            var query = @"SELECT [Buyer].* FROM [Buyer] WHERE Email = @Email";
            var buyer = await _conn.QueryFirstOrDefaultAsync<Buyer>(query, new { Email = email });
            await _conn.CloseAsync();
            return buyer;
        }
        public async Task<Buyer?> GetBuyerWithOrder(int id)
        {
            var query = @"SELECT [Buyer].*, [Order].* FROM [Buyer]
                        LEFT JOIN [Order] ON [Buyer].[Id] = [Order].[Id]
                            where [Buyer].[Id] = @id";
            var param = new DynamicParameters();
            param.Add("id", id);
            var buyer = await _conn.QueryAsync<Buyer, Order, Buyer>(query, (buyer, order) =>
            {
                if (order != null)
                {
                    buyer.Orders.Add(order);
                }
                return buyer;
            }, splitOn: "Id", param: param);
            return buyer.FirstOrDefault(p => p.Id == id);
        }
        public async Task<Buyer?> GetBuyerWithUserName(string userName)
        {
            var query = @"SELECT [Buyer].* FROM [Buyer] WHERE UserName = @UserName";
            var buyer = await _conn.QueryFirstOrDefaultAsync<Buyer>(query, new { UserName = userName });
            await _conn.CloseAsync();
            return buyer;
        }
        public Task<bool> Update(Buyer entity) => _conn.UpdateAsync<Buyer>(entity);
    }
}