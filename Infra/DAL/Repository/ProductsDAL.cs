using Dapper;
using Dapper.Contrib.Extensions;
using Domain.Products;
using Infra.DAL.Contracts;
using System.Data.SqlClient;

namespace Infra.DAL.Repository
{
    public class ProductsDAL : IProductsDAL, IConnection
    {
        private readonly SqlConnection _conn;
        public ProductsDAL(SqlConnection conn)
        {
            _conn = conn;
            _conn.ConnectionString = GetConnectionString();
        }
        public async Task<bool> Create(Product entity)
        {
            var insert = await _conn.InsertAsync(entity);
            return insert.GetType() == typeof(int);
        }
        public async Task<bool> Delete(Product entity) => await _conn.DeleteAsync(entity);
        public async Task<Product?> Get(int Id) => await _conn.GetAsync<Product>(Id);
        public async Task<Product?> GetProductById(int id)
        {
            var query = @"SELECT [Product].*, [Category].* FROM [Product] 
                            LEFT JOIN [Category] ON [Product].[CategoryId] = [Category].[Id]
                            where [Product].[Id] = @id";
            var param = new DynamicParameters();
            param.Add("id", id);

            var exec = await _conn.QueryAsync<Product, Category, Product>(query, (product, category) =>
            {
                if (category != null) product.Category = category;

                return product;
            }, splitOn: "Id", param: param);

            return exec.FirstOrDefault(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var query = @"SELECT [Product].*, [Category].* FROM [Product]
                        LEFT JOIN [Category] ON [Product].[CategoryId] = [Category].[Id]";

            var list = new List<Product>();
            var exec = await _conn.QueryAsync<Product, Category, Product>(query, (product, category) =>
            {
                var prod = list.FirstOrDefault(p => p.Id == product.Id);
                if (prod == null)
                {
                    prod = product;
                    prod.Category = category;
                    list.Add(prod);
                }
                else
                {
                    prod.Category = category;
                }
                return product;
            }, splitOn: "Id");
            return list;
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryId(int id)
        {
            var query = @"SELECT [Product].* FROM [Product] WHERE [CategoryId] = @Id";
            var param = new DynamicParameters();
            param.Add("Id", id);
            var exec = await _conn.QueryAsync<Product>(query, param: param);
            return exec;
        }
        public Task<IEnumerable<Product>> GetProductsWithOrders()
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<Product>> GetTop50Products()
        {
            var query = @"SELECT TOP 50 [Product].*, [Category].* FROM [Product]
                        LEFT JOIN [Category] ON [Product].[CategoryId] = [Category].[Id]";

            var list = new List<Product>();
            var exec = await _conn.QueryAsync<Product, Category, Product>(query, (product, category) =>
            {
                var prod = list.FirstOrDefault(p => p.Id == product.Id);
                if (prod == null)
                {
                    prod = product;
                    prod.Category = category;
                    list.Add(prod);

                }
                else
                {
                    prod.Category = category;
                }
                return product;
            }, splitOn: "Id");
            return list;
        }
        public async Task<bool> Update(Product entity) => await _conn.UpdateAsync<Product>(entity);
        public string GetConnectionString() => Settings.ConnectionString;

    }
}
