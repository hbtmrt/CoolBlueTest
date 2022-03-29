using System.ComponentModel.DataAnnotations;

namespace Insurance.Api.Dtos
{
    public class AddProductTypeRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public float Surcharge { get; set; }

        public bool HasInsurance { get; set; }
    }
}