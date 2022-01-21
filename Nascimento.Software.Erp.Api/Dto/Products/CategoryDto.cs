using System.ComponentModel.DataAnnotations;

namespace Nascimento.Software.Erp.Api.Dto.Products
{
    public class CategoryDto
    {
        [DataType(DataType.Text)]
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}