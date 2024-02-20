namespace SimpleECommerce.Core.Dtos
{
    // Dtos/ProductCreateDto.cs
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public IFormFile MainImage { get; set; } // For uploading the main product image
        public ICollection<IFormFile>? AdditionalImages { get; set; } // For uploading additional images
    }
}
