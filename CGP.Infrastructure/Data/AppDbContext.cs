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

    #endregion


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
           new Role { Id = 1, RoleName = "Admin" },
           new Role { Id = 2, RoleName = "User" }
        );

        //User
        modelBuilder.Entity<ApplicationUser>()
        .Property(u => u.Status)
        .HasConversion(
            v => v.ToString(),
            v => (StatusEnum)Enum.Parse(typeof(StatusEnum), v)
        );

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
            .IsRequired();
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
            e.HasOne(p => p.Category)
            .WithMany(p => p.SubCategories)
            .HasForeignKey(p => p.CategoryId)
            .HasConstraintName("FK_SubCategory_Category");
        });
    }  
}
