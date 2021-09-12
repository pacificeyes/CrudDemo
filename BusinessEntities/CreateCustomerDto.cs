using System.ComponentModel.DataAnnotations;

namespace BusinessEntities
{
    public class CreateCustomerDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
