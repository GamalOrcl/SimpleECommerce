namespace SimpleECommerce.Core.Dtos
{
    // Dtos/CategoryCreateDto.cs
    public class CategoryCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; } // For uploading the category image
    }
}
