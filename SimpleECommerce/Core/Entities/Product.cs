namespace SimpleECommerce.Core.Entities
{
    // Models/Product.cs
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MainImageUrl { get; set; } // Assuming you store image URL
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } // Navigation property
        public ICollection<ProductImage> Images { get; set; } // Navigation property (one-to-many)
    }
}
