namespace SimpleECommerce.Core.Dtos
{
    // Dtos/CategoryUpdateDto.cs
    public class CategoryUpdateDto
    {
        public int Id { get; set; } // Important for validation and mapping
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; } // Optional for updating the image
        public bool IsActive { get; set; } = true;
    }
}
