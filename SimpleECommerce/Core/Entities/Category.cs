 

namespace SimpleECommerce.Core.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; } // Assuming you store image URL
        public bool IsActive { get; set; }
        public ICollection<Product> Products { get; set; } // Navigation property (one-to-many)
    }
}


