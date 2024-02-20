namespace SimpleECommerce.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; } // Placeholder for future payment integration
        public string Status { get; set; } // e.g., Pending, Shipped, Completed
        public int CustomerId { get; set; } // Foreign key to Customer
        public ApplicationUser Customer { get; set; } // Navigation property to Customer
        public ICollection<OrderDetail> OrderDetails { get; set; } // Navigation property to OrderDetails
    }
}
