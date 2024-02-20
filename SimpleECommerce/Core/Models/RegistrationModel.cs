using System.ComponentModel.DataAnnotations;

namespace SimpleECommerce.Core.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Role is required")]
        public string RoleId { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "FirstName Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName Name is required")]
        public string LastName { get; set; }


        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

 

        //[Required(ErrorMessage = "Mobile is required")]
        public string? Mobile { get; set; }
    }
}
