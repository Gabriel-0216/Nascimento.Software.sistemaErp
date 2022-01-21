using Dapper;
using Dapper.Contrib.Extensions;
using Domain.Users;
using Infra.DAL.Contracts;
using System.Data.SqlClient;

namespace Infra.DAL.Repository
{
    public class UserDAL : IUserDAL, IConnection
    {
        private readonly SqlConnection _conn;
        public UserDAL(SqlConnection conn)
        {
            _conn = conn;
            _conn.ConnectionString = Settings.ConnectionString;
        }
        public string GetConnectionString() => Settings.ConnectionString;
        public async Task<User?> Login(User user)
        {
            var query = @"Select [User].* FROM [User] WHERE Email = @email and PasswordHash = @PasswordHash";
            var exec = await _conn.QueryFirstOrDefaultAsync<User>(query, new { Email = user.Email, PasswordHash = user.PasswordHash });
            await _conn.CloseAsync();
            return exec;
        }
        public async Task<User?> Register(User user)
        {
            user.Id = 0;
            var inserted = await _conn.InsertAsync(user);
            var query = @"select * From [User] where Email = @Email";
            var exec = await _conn.QueryFirstOrDefaultAsync<User>(query, new { Email = user.Email });
            await _conn.CloseAsync();
            return exec;
        }
    }
}