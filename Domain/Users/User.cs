using Dapper.Contrib.Extensions;

namespace Domain.Users
{
    [Table("[User]")]
    public class User : Entity
    {
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CellPhone { get; set; } = string.Empty;
    }
}
