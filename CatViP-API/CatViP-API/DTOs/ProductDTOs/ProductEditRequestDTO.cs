using System.ComponentModel.DataAnnotations;

namespace CatViP_API.DTOs.ProductDTOs
{
    public class ProductEditRequestDTO
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public string URL { get; set; } = null!;
        [Required]
        public long ProductTypeId { get; set; }
    }
}
