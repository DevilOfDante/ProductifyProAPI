using System.ComponentModel.DataAnnotations;

namespace ProductifyProAPI.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(150)]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; }

        public string Color { get; set; }
    }
}
