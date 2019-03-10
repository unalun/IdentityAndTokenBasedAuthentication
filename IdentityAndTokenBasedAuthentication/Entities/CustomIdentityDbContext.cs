using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAndTokenBasedAuthentication.Entities
{
    public class CustomIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public CustomIdentityDbContext(DbContextOptions<CustomIdentityDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>(p =>
            {
                p.Property(b => b.Price).HasColumnType("decimal(18, 2)");
                p.Property(b => b.DiscountPrice).HasColumnType("decimal(18, 2)");
            });

            builder.Entity<Product>().HasData(

                new Product() { Id = 1, Name = "Adana Dürüm", Desc = "Çok Lezzetli Adana Dürüm", Price = 50, DiscountPrice = 25, ProductType = Models.Enums.ProductType.Eat },
                new Product() { Id = 2, Name = "Urfa Dürüm", Desc = "Çok Lezzetli Urfa Dürüm", Price = 50, DiscountPrice = 25, ProductType = Models.Enums.ProductType.Eat },
                new Product() { Id = 3, Name = "Tavuk Döner", Desc = "Çok Lezzetli Tavuk Döner", Price = 40, DiscountPrice = 20, ProductType = Models.Enums.ProductType.Eat },
                new Product() { Id = 4, Name = "Et Döner", Desc = "Çok Lezzetli Tavuk Döner", Price = 65, DiscountPrice = 55, ProductType = Models.Enums.ProductType.Eat },
                new Product() { Id = 5, Name = "Kebab", Desc = "Çok Lezzetli Kebab", Price = 75, DiscountPrice = 25, ProductType = Models.Enums.ProductType.Eat },
                new Product() { Id = 6, Name = "ÇigKöfte", Desc = "Çok Lezzetli ÇigKöfte", Price = 35, DiscountPrice = 25, ProductType = Models.Enums.ProductType.Eat },
                new Product() { Id = 7, Name = "Kutu Kola", Desc = "Çok Lezzetli Kutu Kola", Price = 25, DiscountPrice = 20, ProductType = Models.Enums.ProductType.Drink },
                new Product() { Id = 8, Name = "Büyük Kola", Desc = "Büyük Kola", Price = 40, DiscountPrice = 35, ProductType = Models.Enums.ProductType.Drink },
                new Product() { Id = 9, Name = "Soda", Desc = "Soda", Price = 18, DiscountPrice = 15, ProductType = Models.Enums.ProductType.Drink }
            );

            base.OnModelCreating(builder);
        }

        public DbSet<Product> Product { get; set; }
    }
}
