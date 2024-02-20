using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleECommerce.Core.Entities;
using SimpleECommerce.DataAccess.Configuration;
using System.Reflection.Emit;

namespace SimpleECommerce.DataAccess
{
    public class SimpleECommerceDbContext : IdentityDbContext<ApplicationUser>
    {
        public SimpleECommerceDbContext() { }
         
        public SimpleECommerceDbContext(DbContextOptions<SimpleECommerceDbContext> options) : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
             
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=SimpleECommerceDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
            }
            optionsBuilder.EnableSensitiveDataLogging();

            base.OnConfiguring(optionsBuilder);
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ApplicationUser> Customers { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());

            string ADMIN_ID = "02174cf0–9412–4cfe-afbD-59f706d72cf6";
            
            //create user
            var ApplicationUser = new ApplicationUser
            {
                Id = ADMIN_ID,
                FirstName = "جمال صلاح الدين",
                LastName = "عابدين",
                Mobile = "01067543471",
                Email = "gamalorcl@gmail.com",
                NormalizedEmail = "GAMALORCL@GMAIL.COM",
                EmailConfirmed = true,
                UserName = "gamalorcl",
                NormalizedUserName = "GAMALORCL"  
            };

            //set user password
            PasswordHasher<ApplicationUser> ph2 = new PasswordHasher<ApplicationUser>();
            ApplicationUser.PasswordHash = ph2.HashPassword(ApplicationUser, "Admin_123");

            //seed user
            modelBuilder.Entity<ApplicationUser>().HasData(ApplicationUser);

            //set user role to admin
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "711111-111–77777-1111-111117",
                UserId = ADMIN_ID
            });


            //////===============================================================



            base.OnModelCreating(modelBuilder);


        }

    }
}
