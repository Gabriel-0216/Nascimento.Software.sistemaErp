using System.Text;

namespace Services.Password
{
    public class PasswordService : IPasswordService
    {
        public string EncryptPassword(string password)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(password));
        }
    }
}
