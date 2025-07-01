using CGP.Domain.Entities;
using CGP.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace CGP.Infrastructure.Data;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }


    #region DbSet
    public DbSet<Role> Role { get; set; }
    public DbSet<ApplicationUser> User { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<SubCategory> SubCategory { get; set; }
    public DbSet<CraftVillage> CraftVillage { get; set; }
    public DbSet<Wallet> Wallet { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Meterial> Meterial { get; set; }
    public DbSet<ProductImage> ProductImage { get; set; }
    public DbSet<UserAddress> UserAddress { get; set; }
    public DbSet<ArtisanRequest> ArtisanRequest { get; set; }
    public DbSet<Cart> Cart { get; set; }
    public DbSet<CartItem> CartItem { get; set; }
    public DbSet<Favourite> Favourite { get; set; }
    #endregion


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
           new Role { Id = 1, RoleName = "Admin" },
           new Role { Id = 2, RoleName = "Staff" },
           new Role { Id = 3, RoleName = "Artisan" },
           new Role { Id = 4, RoleName = "User" }
        );


        //User
        modelBuilder.Entity<ApplicationUser>()
        .Property(u => u.Status)
        .HasConversion(
            v => v.ToString(),
            v => (StatusEnum)Enum.Parse(typeof(StatusEnum), v)
        );
        modelBuilder.Entity<ApplicationUser>()
            .HasData(
                new ApplicationUser { Id = Guid.Parse("8B56687E-8377-4743-AAC9-08DCF5C4B471"), UserName = "Admin", Email = "admin@gmail.com", PasswordHash = "$2y$10$O1smXu1TdT1x.Z35v5jQauKcQIBn85VYRqiLggPD8HMF9rRyGnHXy", Status = StatusEnum.Active, RoleId = 1, IsVerified = true, PhoneNumber = "0123456789", CreationDate = DateTime.Now, IsDeleted = false },
                new ApplicationUser { Id = Guid.Parse("8B56687E-8377-4743-AAC9-08DCF5C4B47F"), UserName = "Staff", Email = "staff@gmail.com", PasswordHash = "$2y$10$O1smXu1TdT1x.Z35v5jQauKcQIBn85VYRqiLggPD8HMF9rRyGnHXy", Status = StatusEnum.Active, RoleId = 2, IsVerified = true, PhoneNumber = "0123456789", CreationDate = DateTime.Now, IsDeleted = false },
                new ApplicationUser { Id = Guid.Parse("8B56687E-8377-4743-AAC9-08DCF5C4B470"), UserName = "Artisan", Email = "artisan@gmail.com", PasswordHash = "$2y$10$O1smXu1TdT1x.Z35v5jQauKcQIBn85VYRqiLggPD8HMF9rRyGnHXy", Status = StatusEnum.Active, RoleId = 3, IsVerified = true, PhoneNumber = "0123456789", CreationDate = DateTime.Now, IsDeleted = false },
                new ApplicationUser { Id = Guid.Parse("8B56687E-8377-4743-AAC9-08DCF5C4B469"), UserName = "User", Email = "user@gmail.com", PasswordHash = "$2y$10$O1smXu1TdT1x.Z35v5jQauKcQIBn85VYRqiLggPD8HMF9rRyGnHXy", Status = StatusEnum.Active, RoleId = 4, IsVerified = true, PhoneNumber = "0123456789", CreationDate = DateTime.Now, IsDeleted = false }
           );

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(u => u.CraftVillage)
            .WithMany(u => u.Users)
            .HasForeignKey(u => u.CraftVillage_Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(r => r.Role)
            .WithMany(u => u.Users);

        //Wallet
        modelBuilder.Entity<Wallet>(e =>
        {
            e.ToTable("Wallet");
            e.HasKey(p => p.Id);
            e.Property(p => p.Id)
            .IsRequired();
            e.Property(p => p.Balance)
            .HasDefaultValue(0);
            e.HasOne(p => p.User)
            .WithOne(p => p.Wallet)
            .HasForeignKey<Wallet>(p => p.User_Id)
            .OnDelete(DeleteBehavior.Cascade);
        });

        //Category
        modelBuilder.Entity<Category>(e =>
        {
            e.ToTable("Category");
            e.HasKey(p => p.Id);
            e.Property(p => p.Id)
            .IsRequired()
            .HasMaxLength(50);
            e.Property(p => p.CategoryName)
            .IsRequired()
            .HasMaxLength(50);
            e.Property(p => p.CreationDate)
            .IsRequired()
            .HasDefaultValueSql("getutcdate()");
            e.Property(p => p.CategoryStatus)
            .IsRequired()
            .HasConversion(v => v.ToString(), v => (CategoryStatusEnum)Enum.Parse(typeof(CategoryStatusEnum), v));
        });

        //SubCategory
        modelBuilder.Entity<SubCategory>(e =>
        {
            e.ToTable("SubCategory");
            e.HasKey(p => p.Id);
            e.Property(p => p.Id)
            .IsRequired()
            .HasMaxLength(50);
            e.Property(p => p.SubName)
            .IsRequired()
            .HasMaxLength(50);
            e.Property(p => p.CreationDate)
            .IsRequired()
            .HasDefaultValueSql("getutcdate()");
            e.Property(p => p.CategoryId)
            .IsRequired();
            e.Property(p => p.Status)
            .IsRequired()
            .HasConversion(v => v.ToString(), v => (CategoryStatusEnum)Enum.Parse(typeof(CategoryStatusEnum), v));
            e.HasOne(p => p.Category)
            .WithMany(p => p.SubCategories)
            .HasForeignKey(p => p.CategoryId)
            .HasConstraintName("FK_SubCategory_Category");
        });

        modelBuilder.Entity<SubCategory>()
            .HasOne(sc => sc.Category)
            .WithMany(sc => sc.SubCategories);

        //CraftVillage
        modelBuilder.Entity<CraftVillage>(e =>
        {
            e.ToTable("CraftVillage");
            e.HasKey(p => p.Id);
            e.Property(p => p.Id)
            .IsRequired()
            .HasMaxLength(50);
            e.Property(p => p.Village_Name)
            .IsRequired()
            .HasMaxLength(50);
            e.Property(p => p.CreationDate)
            .IsRequired()
            .HasDefaultValueSql("getutcdate()");
        });

        //Product
        modelBuilder.Entity<Product>(e =>
        {
            e.ToTable("Product");
            e.HasKey(p => p.Id);
            e.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);
            e.Property(p => p.Description)
            .HasMaxLength(250);
            e.Property(p => p.Status)
            .IsRequired()
            .HasConversion(v => v.ToString(), v => (ProductStatusEnum)Enum.Parse(typeof(ProductStatusEnum), v));
        });

        modelBuilder.Entity<Product>()
            .HasOne(p => p.SubCategory)
            .WithMany(p => p.Products);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.User)
            .WithMany(p => p.Products);
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        //Meterial
        modelBuilder.Entity<Meterial>(e =>
        {
            e.ToTable("Meterial");
            e.HasKey(p => p.Id);
            e.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);
        });

        //ProductImage
        modelBuilder.Entity<ProductImage>(e =>
        {
            e.ToTable("ProductImage");
            e.HasKey(p => p.Id);
            e.Property(p => p.ImageUrl)
            .IsRequired()
            .HasMaxLength(250);
            e.HasOne(p => p.Product)
            .WithMany(p => p.ProductImages)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        });

        //UserAddress
        modelBuilder.Entity<UserAddress>(e =>
        {
            e.ToTable("UserAddress");
            e.HasKey(p => p.Id);
            e.HasOne(p => p.User)
            .WithMany(p => p.UserAddresses)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        });

        //ArtisanRequest
        modelBuilder.Entity<ArtisanRequest>(e =>
        {
            e.ToTable("ArtisanRequest");
            e.HasKey(p => p.Id);
            e.Property(p => p.Status)
            .IsRequired()
            .HasConversion(v => v.ToString(), v => (RequestArtisanStatus)Enum.Parse(typeof(RequestArtisanStatus), v));
            e.HasOne(p => p.User)
            .WithOne(p => p.ArtisanRequest)
            .HasForeignKey<ArtisanRequest>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(p => p.CraftVillages)
            .WithMany(p => p.ArtisanRequests)
            .HasForeignKey(p => p.CraftVillageId)
            .OnDelete(DeleteBehavior.Cascade);
        });

        //Cart
        modelBuilder.Entity<Cart>()
            .HasOne(c => c.User)
            .WithMany(u => u.Carts)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        //CartItem
        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Cart)
            .WithMany(c => c.Items)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CartItem>()
            .Property(ci => ci.UnitPrice)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Product)
            .WithMany(p => p.CartItems)
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        //Favourite

        modelBuilder.Entity<Favourite>()
            .HasOne(f => f.User)
            .WithMany(u => u.Favourites)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Favourite>()
            .HasOne(f => f.Product)
            .WithMany(p => p.Favourites)
            .HasForeignKey(f => f.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
