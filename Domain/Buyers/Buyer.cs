using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Domain.Buyers
{
    [Table("[Buyer]")]

    public class Buyer : Entity
    {
        public Buyer()
        {
            Orders = new List<Orders.Order>();
        }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]

        public string LastName { get; set; } = string.Empty;
        [Required]

        public string Email { get; set; } = string.Empty;
        [Required]

        public string Cellphone { get; set; } = string.Empty;
        [Required]
        public bool IsActive { get; set; }

        [Write(false)]
        public List<Orders.Order> Orders { get; set; }
    }
}
