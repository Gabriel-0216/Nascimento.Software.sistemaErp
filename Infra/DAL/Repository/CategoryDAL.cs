using Dapper;
using Dapper.Contrib.Extensions;
using Domain.Products;
using Infra.DAL.Contracts;
using System.Data.SqlClient;

namespace Infra.DAL.Repository
{
    public class CategoryDAL : ICategoryDAL, IConnection
    {
        private readonly SqlConnection _conn;
        public CategoryDAL(SqlConnection conn)
        {
            _conn = conn;
            _conn.ConnectionString = GetConnectionString();
        }
        public async Task<bool> Create(Category entity)
        {
            var inserted = await _conn.InsertAsync(entity);
            return (inserted.GetType() == typeof(int));
        }
        public async Task<bool> Delete(Category entity) => await _conn.DeleteAsync(entity);

        public async Task<Category?> Get(int Id) => await _conn.GetAsync<Category>(Id);

        public async Task<IEnumerable<Category>> GetCategories() => await _conn.GetAllAsync<Category>();
        public async Task<Category?> GetCategoryWithProducts(int id)
        {
            var query = @"SELECT [Category].*, [Product].* FROM [Category]
                    LEFT JOIN [Product] ON [Category].[Id] = [Product].[CategoryId]
                                                        where [Category].[Id] = @id";
            var param = new DynamicParameters();
            param.Add("Id", id);
            var exec = await _conn.QueryAsync<Category, Product, Category>(query, (category, product) =>
            {
                if (product != null)
                {
                    category.Products.Add(product);
                }
                return category;
            }, splitOn: "Id", param: param);
            return exec.FirstOrDefault(p => p.Id == id);
        }

        public string GetConnectionString()
        {
            return Settings.ConnectionString;
        }

        public async Task<bool> Update(Category entity) => await _conn.UpdateAsync(entity);
    }
}
