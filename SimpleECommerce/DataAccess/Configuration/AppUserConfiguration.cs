 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleECommerce.Core.Entities;

namespace SimpleECommerce.DataAccess.Configuration
{
    public class AppUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("AspNetUsers");
             
            builder
                .HasIndex(p => p.Mobile)
                .IsUnique();

            builder
              .HasIndex(p => p.Email)
              .IsUnique();

        


        }
    }
}
