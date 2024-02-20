

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimpleECommerce.DataAccess.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {

        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "711111-111–77777-1111-111117",
                    Name = "Admin",
                    NormalizedName = "Admin",
                    ConcurrencyStamp = "711111-111–77777-1111-111117"
                },

                new IdentityRole
                {
                    Id = "341743f0-9999–42de-FFFD-59mkkmk72cf6",
                    Name = "Student",
                    NormalizedName = "STUDENT",
                    ConcurrencyStamp = "341743f0-9999–42de-FFFD-59mkkmk72cf6"
                },

                new IdentityRole
                {
                    Id = "341743f0-9999–6666-FFFD-59mkkmk72cf6",
                    Name = "Parent",
                    NormalizedName = "PARENT",
                    ConcurrencyStamp = "341743f0-9999–6666-FFFD-59mkkmk72cf6"
                },

                new IdentityRole
                {
                    Id = "341743f0-9999–3366-8888-5944mk77777",
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                    ConcurrencyStamp = "341743f0-9999–6666-FFFD-59mkkmk72cf6"
                },

                new IdentityRole
                {
                    Id = "1111f0-111222–3339999-FF2424mk9999",
                    Name = "DataEntry",
                    NormalizedName = "DATAENTRY",
                    ConcurrencyStamp = "341743f0-9999–6666-FdFFD-59mqqmk72cf6"
                },

                new IdentityRole
                {
                    Id = "1111f0-11192–33366-FF2424mk72cf6",
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE",
                    ConcurrencyStamp = "341743f0-9999–6666-FFFD-59mkkmk72cf6"
                },

                new IdentityRole
                {
                    Id = "909088-909090–33966-FF242999999",
                    Name = "FinancialManager",
                    NormalizedName = "FINANCIALMANAGER",
                    ConcurrencyStamp = "341743f0-9999–6666-FFFD-59mkkmk72cf6"
                }
            );
        }
    }
}

