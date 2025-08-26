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
    public DbSet<Order> Order { get; set; }
    public DbSet<OrderItem> OrderItem { get; set; }
    public DbSet<Payment> Payment { get; set; }
    public DbSet<CraftSkill> CraftSkill { get; set; }
    public DbSet<OrderVoucher> OrderVoucher { get; set; }
    public DbSet<Voucher> Voucher { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<Point> Point { get; set; }
    public DbSet<Rating> Rating { get; set; }
    public DbSet<ReturnRequest> ReturnRequest { get; set; }
    public DbSet<ActivityLog> ActivityLog { get; set; }
    public DbSet<WalletTransaction> WalletTransaction { get; set; }
    public DbSet<PointTransaction> PointTransactions { get; set; }
    public DbSet<Crop> Crop { get; set; }
    public DbSet<Inventory> Inventory { get; set; }
    public DbSet<UserQuest> UserQuest { get; set; }
    public DbSet<Quest> Quest { get; set; }
    public DbSet<DailyCheckIn> DailyCheckIn { get; set; }
    public DbSet<OrderAddress> OrderAddress { get; set; }
    public DbSet<FarmLand> FarmLand { get; set; }
    public DbSet<FarmlandCrop> FarmlandCrop { get; set; }
    public DbSet<SaleTransaction> SaleTransaction { get; set; }
    public DbSet<ShopPrice> ShopPrice { get; set; }
    public DbSet<Item> Items { get; set; }
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
                new ApplicationUser { Id = Guid.Parse("8B56687E-8377-4743-AAC9-08DCF5C4B47F"), UserName = "Staff", Email = "staff@gmail.com", PasswordHash = "$2y$10$O1smXu1TdT1x.Z35v5jQauKcQIBn85VYRqiLggPD8HMF9rRyGnHXy", Status = StatusEnum.Active, RoleId = 2, IsVerified = true, PhoneNumber = "0123456789", CreationDate = DateTime.Now, IsDeleted = false }
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
            e.Property(p => p.AvailableBalance)
            .HasDefaultValue(0);
            e.HasOne(p => p.User)
            .WithOne(p => p.Wallet)
            .HasForeignKey<Wallet>(p => p.User_Id)
            .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Wallet>()
            .HasData(
            new Wallet { Id = Guid.Parse("8B56687E-8377-4743-AAC9-08DCF5C4B400"), AvailableBalance = 0, User_Id = Guid.Parse("8B56687E-8377-4743-AAC9-08DCF5C4B471"), Type = WalletTypeEnum.System },
            new Wallet { Id = Guid.Parse("8B56687E-8377-4743-AAC9-08DCF5C4B401"), AvailableBalance = 0, User_Id = Guid.Parse("8B56687E-8377-4743-AAC9-08DCF5C4B47F"), Type = WalletTypeEnum.User }
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

            e.Property(p => p.AddressType)
            .IsRequired()
            .HasConversion(v => v.ToString(), v => (TypeAddressEnum)Enum.Parse(typeof(TypeAddressEnum), v));
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

        //Order
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(o => o.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        //OrderItem
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        //Payment
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Payment)
            .WithOne(p => p.Order)
            .HasForeignKey<Payment>(p => p.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
            .Property(p => p.PaymentMethod)
            .HasConversion(v => v.ToString(), v => (PaymentMethodEnum)Enum.Parse(typeof(PaymentMethodEnum), v));

        //Voucher
        modelBuilder.Entity<Voucher>(e =>
        {
            e.ToTable("Voucher");
            e.HasKey(v => v.Id);
            e.Property(v => v.Code)
            .IsRequired()
            .HasMaxLength(50);
        });

        modelBuilder.Entity<Voucher>()
            .Property(v => v.Type)
            .IsRequired()
            .HasConversion(v => v.ToString(), v => (VoucherTypeEnum)Enum.Parse(typeof(VoucherTypeEnum), v));

        modelBuilder.Entity<Voucher>()
            .Property(v => v.DiscountType)
            .IsRequired()
            .HasConversion(v => v.ToString(), v => (VoucherDiscountTypeEnum)Enum.Parse(typeof(VoucherDiscountTypeEnum), v));

        modelBuilder.Entity<Voucher>()
            .Property(v => v.PaymentMethod)
            .IsRequired()
            .HasConversion(v => v.ToString(), v => (PaymentMethodEnum)Enum.Parse(typeof(PaymentMethodEnum), v));

        //OrderVoucher
        modelBuilder.Entity<OrderVoucher>(e =>
        {
            e.ToTable("OrderVoucher");
            e.HasKey(ov => ov.Id);
            e.HasOne(ov => ov.Order)
            .WithMany(o => o.OrderVouchers)
            .HasForeignKey(ov => ov.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(ov => ov.Voucher)
            .WithMany(v => v.OrderVouchers)
            .HasForeignKey(ov => ov.VoucherId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        //Transaction
        modelBuilder.Entity<Transaction>(e =>
        {
            e.ToTable("Transaction");
            e.HasKey(t => t.Id);
            e.HasOne(t => t.Order)
            .WithMany(t => t.Transactions)
            .HasForeignKey(t => t.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Transaction>(e =>
        {
            e.HasOne(t => t.Wallet)
            .WithMany(t => t.Transactions)
            .HasForeignKey(t => t.WalletId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Transaction>(e =>
        {
            e.HasOne(t => t.Payment)
            .WithMany(t => t.Transactions)
            .HasForeignKey(t => t.PaymentId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Transaction>(e =>
        {
            e.HasOne(t => t.Voucher)
            .WithMany(t => t.Transactions)
            .HasForeignKey(t => t.VoucherId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Transaction>(e =>
        {
            e.HasOne(t => t.User)
            .WithMany(t => t.Transactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Transaction>()
            .Property(v => v.TransactionStatus)
            .IsRequired()
            .HasConversion(v => v.ToString(), v => (TransactionStatusEnum)Enum.Parse(typeof(TransactionStatusEnum), v));

        //Point
        modelBuilder.Entity<Point>(e =>
        {
            e.ToTable("Point");
            e.HasKey(p => p.Id);
            e.HasOne(p => p.User)
            .WithOne(u => u.Point)
            .HasForeignKey<Point>(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        //Rating
        modelBuilder.Entity<Rating>(entity =>
        {
            entity.ToTable("Ratings");

            entity.HasKey(r => r.Id);

            entity.HasOne(r => r.User)
                  .WithMany(u => u.Ratings)
                  .HasForeignKey(r => r.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Product)
                  .WithMany(p => p.Ratings)
                  .HasForeignKey(r => r.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.OrderItem)
                  .WithOne(oi => oi.Rating)
                  .HasForeignKey<Rating>(r => r.OrderItemId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(r => new { r.UserId, r.OrderItemId }).IsUnique();
        });

        //ReturnRequest
        modelBuilder.Entity<ReturnRequest>(e =>
        {
            e.ToTable("ReturnRequest");
            e.HasKey(r => r.Id);
            e.Property(r => r.Status)
            .IsRequired()
            .HasConversion(v => v.ToString(), v => (ReturnStatusEnum)Enum.Parse(typeof(ReturnStatusEnum), v));
            e.Property(r => r.Reason)
            .HasConversion(v => v.ToString(), v => (ReturnReasonEnum)Enum.Parse(typeof(ReturnReasonEnum), v));
            e.HasOne(r => r.OrderItem)
            .WithOne(o => o.ReturnRequest)
            .HasForeignKey<ReturnRequest>(r => r.OrderItemId)
            .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(r => r.User)
            .WithMany(o => o.ReturnRequests)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        //WalletTransaction
        modelBuilder.Entity<WalletTransaction>(e =>
        {
            e.ToTable("WalletTransaction");
            e.HasKey(wt => wt.Id);
            e.HasOne(wt => wt.Wallet)
            .WithMany(w => w.WalletTransactions)
            .HasForeignKey(wt => wt.Wallet_Id)
            .OnDelete(DeleteBehavior.Restrict);
        });

        //PointTransaction
        modelBuilder.Entity<PointTransaction>(e =>
        {
            e.ToTable("PointTransaction");
            e.HasKey(wt => wt.Id);
            e.HasOne(wt => wt.Point)
            .WithMany(w => w.PointTransactions)
            .HasForeignKey(wt => wt.Point_Id)
            .OnDelete(DeleteBehavior.Restrict);
        });

        //OrderAddress
        modelBuilder.Entity<OrderAddress>(e =>
        {
            e.ToTable("OrderAddress");
            e.HasKey(oa => oa.Id);
            e.HasOne(oa => oa.Order)
            .WithOne(o => o.OrderAddress)
            .HasForeignKey<OrderAddress>(oa => oa.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(oa => oa.Order)
            .WithOne(ua => ua.OrderAddress)
            .HasForeignKey<OrderAddress>(oa => oa.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        //DailyCheckIn
        modelBuilder.Entity<DailyCheckIn>(e =>
        {
            e.ToTable("DailyCheckIn");
            e.HasKey(d => d.Id);
            e.HasOne(d => d.User)
            .WithMany(u => u.DailyCheckIns)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        //FarmLand
        modelBuilder.Entity<FarmLand>(e =>
        {
            e.ToTable("FarmLand");
            e.HasKey(f => f.Id);
            e.HasOne(f => f.User)
            .WithMany(u => u.FarmLands)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        //FarmlandCrop
        modelBuilder.Entity<FarmlandCrop>(e =>
        {
            e.ToTable("FarmlandCrop");
            e.HasKey(fc => fc.Id);
            e.HasOne(fc => fc.Farmland)
            .WithMany(f => f.FarmlandCrops)
            .HasForeignKey(fc => fc.FarmlandId)
            .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(fc => fc.Crop)
            .WithMany(c => c.FarmlandCrops)
            .HasForeignKey(fc => fc.CropId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        //Crop
        modelBuilder.Entity<Inventory>(e =>
        {
            e.ToTable("Crop");
            e.HasKey(i => i.Id);
        });

        //Inventory
        modelBuilder.Entity<Inventory>(e =>
        {
            e.ToTable("Inventory");
            e.HasKey(i => i.Id);
            e.HasOne(i => i.User)
            .WithMany(u => u.Inventories)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Inventory>(e =>
        {
            e.HasOne(i => i.Item)
            .WithMany(i => i.Inventories)
            .HasForeignKey(i => i.ItemId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        //UserQuest
        modelBuilder.Entity<UserQuest>(e =>
        {
            e.ToTable("UserQuest");
            e.HasKey(uq => uq.Id);
            e.HasOne(uq => uq.User)
            .WithMany(u => u.UserQuests)
            .HasForeignKey(uq => uq.UserId)
            .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(uq => uq.Quest)
            .WithMany(q => q.UserQuests)
            .HasForeignKey(uq => uq.QuestId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        //Item
        modelBuilder.Entity<Item>(e =>
        {
            e.ToTable("Item");
            e.HasKey(i => i.Id);
            e.Property(i => i.NameItem)
            .IsRequired()
            .HasMaxLength(50);
            e.Property(i => i.ItemType)
            .HasMaxLength(50).HasConversion(v => v.ToString(), v => (ItemTypeEnum)Enum.Parse(typeof(ItemTypeEnum), v));
        });

        //ShopPPrice
        modelBuilder.Entity<ShopPrice>(e =>
        {
            e.ToTable("ShopPrice");
            e.HasKey(sp => sp.Id);
            e.HasOne(sp => sp.Item)
            .WithOne(i => i.ShopPrice)
            .HasForeignKey<ShopPrice>(sp => sp.ItemId)
            .OnDelete(DeleteBehavior.Restrict);
        });


        //SaleTransaction
        modelBuilder.Entity<SaleTransaction>(e =>
        {
            e.ToTable("SaleTransaction");
            e.HasKey(st => st.Id);
            e.HasOne(st => st.User)
            .WithMany(u => u.SaleTransactions)
            .HasForeignKey(st => st.UserId)
            .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(st => st.Item)
            .WithMany(i => i.SaleTransactions)
            .HasForeignKey(st => st.ItemId)
            .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
