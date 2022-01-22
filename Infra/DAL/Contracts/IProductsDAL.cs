using Domain.Products;
namespace Infra.DAL.Contracts
{
    public interface IProductsDAL : IGenericDAL<Product>
    {
        Task<IEnumerable<Product>> GetProductsWithOrders();
        Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<Product>> GetTop50Products();
        Task<IEnumerable<Product>> GetProductsByCategoryId(int id);
        Task<Product?> GetProductById(int id);
        Task<bool> ProductExists(int id);

    }
}
