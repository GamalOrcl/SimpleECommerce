using Microsoft.AspNetCore.Identity;
using SimpleECommerce.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Reflection;

namespace SimpleECommerce.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }


         public string? Email { get; set; }


        public string? Mobile { get; set; }
        public string? Address { get; set; }

           

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

    }
}
 