using System.ComponentModel.DataAnnotations;
namespace Nascimento.Software.Erp.Api.Dto.User
{
    public class UserLoginDto
    {
        [DataType(DataType.Text)]
        public string Email { get; set; } = string.Empty;
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}