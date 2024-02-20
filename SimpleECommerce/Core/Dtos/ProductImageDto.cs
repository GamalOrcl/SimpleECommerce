namespace SimpleECommerce.Core.Dtos
{
    public class ProductImageDto
    {
        public int Id { get; set; }
        public string Url { get; set; } // Assuming you store image URL
        public string Description { get; set; } // Optional: Description for the image
    }
}
