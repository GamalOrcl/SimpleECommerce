using System.ComponentModel.DataAnnotations;

namespace SimpleECommerce.Core.Models
{
    public class ChangePasswordModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string OriginalPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
