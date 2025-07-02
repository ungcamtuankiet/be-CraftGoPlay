using CGP.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CGP.Domain.Entities
{
    public class ApplicationUser : BaseEntity
    {
        public string UserName { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Thumbnail { get; set; }
        public string Email { get; set; }
        public string? RefreshToken { get; set; }
        public StatusEnum? Status { get; set; }
        public string? Otp { get; set; }
        public bool IsVerified { get; set; } = false;
        public DateTime? OtpExpiryTime { get; set; }
        public string? VerificationToken { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }
        public string? Provider { get; set; }
        public string? ProviderKey { get; set; }


        //Relationships
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        public Guid? CraftVillage_Id { get; set; }
        [ForeignKey("CraftVillage_Id")]
        public CraftVillage CraftVillage { get; set; }
        public Wallet Wallet { get; set; }
        public ArtisanRequest ArtisanRequest { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();
        public ICollection<Order> Orders { get; set; }

    }
}
