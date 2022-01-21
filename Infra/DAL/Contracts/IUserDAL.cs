using Domain.Users;
namespace Infra.DAL.Contracts
{
    public interface IUserDAL
    {
        Task<User?> Login(User user);
        Task<User?> Register(User user);
    }
}
