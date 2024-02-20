namespace SimpleECommerce.Core.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; } // Foreign key to Order
        public int ProductId { get; set; } // Foreign key to Product
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Order Order { get; set; } // Navigation property to Order
        public Product Product { get; set; } // Navigation property to Product
    }
}
