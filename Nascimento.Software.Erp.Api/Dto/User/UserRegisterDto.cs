using System.ComponentModel.DataAnnotations;
namespace Nascimento.Software.Erp.Api.Dto.User
{
    public class UserRegisterDto
    {
        [DataType(DataType.Text)]
        public string UserName { get; set; } = string.Empty;
        [DataType(DataType.Text)]
        public string FirstName { get; set; } = string.Empty;
        [DataType(DataType.Text)]
        public string LastName { get; set; } = string.Empty;
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [DataType(DataType.PhoneNumber)]
        public string CellPhone { get; set; } = string.Empty;
    }
}