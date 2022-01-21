using System.ComponentModel.DataAnnotations;

namespace Nascimento.Software.Erp.Api.Dto.Buyer
{
    public class BuyerDto
    {
        [DataType(DataType.Text)]
        public string FirstName { get; set; } = string.Empty;

        [DataType(DataType.Text)]
        public string LastName { get; set; } = string.Empty;

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.PhoneNumber)]
        public string Cellphone { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
