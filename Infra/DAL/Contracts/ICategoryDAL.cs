using Domain.Products;
namespace Infra.DAL.Contracts
{
    public interface ICategoryDAL : IGenericDAL<Category>
    {
        Task<Category?> GetCategoryWithProducts(int id);
        Task<IEnumerable<Category>> GetCategories();
    }
}