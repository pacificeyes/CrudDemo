using System.ComponentModel.DataAnnotations;

namespace BusinessEntities
{
    public class CustomerDto : CreateCustomerDto
    {
        [Required]
        public int Id { get; set; }
    }
}
