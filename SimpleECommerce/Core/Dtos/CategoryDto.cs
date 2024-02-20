using SimpleECommerce.Core.Entities;

namespace SimpleECommerce.Core.Dtos
{
    // Dtos/CategoryDto.cs
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }

        // Navigation property (mapped if needed):
        public IEnumerable<ProductDto> Products { get; set; } // Optional: List of product IDs or simplified product information
    }
}
 