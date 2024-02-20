using System.ComponentModel.DataAnnotations;

namespace SimpleECommerce.Core.Models
{
    public class UpdateUserInfoModel
    {
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string Role { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "FirstName Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName Name is required")]
        public string LastName { get; set; }




        [EmailAddress(ErrorMessage = "الإيميل غير صحيح")]
        public string? Email { get; set; }  

    }
}
