namespace SimpleECommerce.Core.Dtos
{
    // Dtos/ProductDto.cs
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MainImageUrl { get; set; }
        public IEnumerable<string> AdditionalImageUrls { get; set; } // URLs for additional images
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } // Optional: Category name for display

        // Navigation property (mapped if needed):
        public IEnumerable<ProductImageDto> Images { get; set; } // Optional: Detailed image information
    }
}
