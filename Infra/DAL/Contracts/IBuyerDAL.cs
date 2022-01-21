using Domain.Buyers;
namespace Infra.DAL.Contracts
{
    public interface IBuyerDAL : IGenericDAL<Buyer>
    {
        Task<IEnumerable<Buyer>> GetAll();
        Task<Buyer?> GetBuyerWithOrder(int id);
        Task<Buyer?> GetBuyerWithEmail(string email);
        Task<Buyer?> GetBuyerWithCellphone(string cellphone);
        Task<Buyer?> GetBuyerWithUserName(string userName);
    }
}
