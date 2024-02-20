namespace SimpleECommerce.Core.Entities
{
    // Models/ProductImage.cs
    public class ProductImage
    {
        public int Id { get; set; }
        public string Url { get; set; } // Image URL
        public int ProductId { get; set; }
        public Product Product { get; set; } // Navigation property
    }
}
