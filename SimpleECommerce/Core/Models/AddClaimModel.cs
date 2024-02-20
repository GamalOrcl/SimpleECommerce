namespace SimpleECommerce.Core.Models
{
    public class AddClaimModel
    {
        public int UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
