namespace SimpleECommerce.Core.Models
{
    public class IdentityResponseModel
    {
        public bool IsSuccessful { get; set; }
        public List<IdentityErrorResponseModel> errors { get; set; }
    }
}
