namespace SimpleECommerce.Core.Dtos
{
    // Dtos/ProductUpdateDto.cs
    public class ProductUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? MainImage { get; set; } // Optional for updating the main image
        public IEnumerable<string> AdditionalImagesToRemove { get; set; } // For removing existing images
        public ICollection<IFormFile>? AdditionalImages { get; set; } // For adding new images
    }
}
