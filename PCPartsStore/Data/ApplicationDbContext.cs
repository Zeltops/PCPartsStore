using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PCPartsStore.Entities;

namespace PCPartsStore.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Product> Products { get; set; }

    public DbSet<ProductCategory> ProductsCategory { get; set; }

    public DbSet<ProductImage> ProductsImages { get; set; }

    public DbSet<Address> UserAddress { get; set; }

    public DbSet<Order> Orders { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
         SeedAspNetRolesTable(builder);
         SeedProductCategoryTable(builder);
    }

    private void SeedAspNetRolesTable(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole("Admin")
            {
                Id = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole("User")
            {
                Id = "User",
                NormalizedName = "USER"
            });
    }

    private void SeedProductCategoryTable(ModelBuilder builder)
    {
        builder.Entity<ProductCategory>().HasData(
            new ProductCategory
            {
                Id = 1,
                Name = "CPU"
            },
            new ProductCategory
            {
                Id = 2,
                Name = "GPU"
            },
            new ProductCategory
            {
                Id = 3,
                Name = "RAM"
            },
            new ProductCategory
            {
                Id = 4,
                Name = "Motherboard"
            },
            new ProductCategory
            {
                Id = 5,
                Name = "Other"
            });
    }
}