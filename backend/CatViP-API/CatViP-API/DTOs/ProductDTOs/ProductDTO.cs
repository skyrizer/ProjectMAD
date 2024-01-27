
namespace CatViP_API.DTOs.ProductDTOs
{
    public class ProductDTO
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public string Description { get; set; } = null!;

        public string URL { get; set; } = null!;

        public long ProductTypeId { get; set; }

        public string ProductType { get; set; } = null!;

        public byte[] Image { get; set; } = null!;
    }
}
