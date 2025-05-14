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
    }  
}
