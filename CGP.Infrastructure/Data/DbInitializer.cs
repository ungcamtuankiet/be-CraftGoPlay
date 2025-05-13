using CGP.Domain.Entities;
using CGP.Domain.Enums;

namespace Server.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.User.Any())
            {
                return;
            }

            // Create admin users
            var admin = new ApplicationUser
            {
                UserName = "admin1",
                Email = "admin1@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("AdminPassword123"),
                IsVerified = true,
                RoleId = context.Role.Single(r => r.RoleName == "Admin").Id
            };
            context.User.Add(admin);

            // Create shop user
            var shop = new ApplicationUser
            {
                UserName = "staff1",
                Email = "kietuctse161952@fpt.edu.vn",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Kiet@0793213702"),
                Status = StatusEnum.Active,
                IsVerified = true,
                RoleId = context.Role.Single(r => r.RoleName == "User").Id
            };
            context.User.Add(shop);
        }
    }
}
