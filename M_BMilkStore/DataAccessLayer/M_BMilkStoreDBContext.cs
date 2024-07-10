using System.IO;
using BussinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer
{
    public class M_BMilkStoreDBContext : DbContext
    {
        public M_BMilkStoreDBContext() { }

        public M_BMilkStoreDBContext(DbContextOptions<M_BMilkStoreDBContext> options)
            : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductBrand> ProductBrands { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }
        public virtual DbSet<ProductLine> ProductLines { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserVoucher> UserVouchers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var strConn = config.GetConnectionString("DB");

            return strConn;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>(e =>
            {
                e.ToTable("UserRole");
                e.HasKey(x => x.UserRoleId);
                e.Property(x => x.UserRoleId).ValueGeneratedNever();
                e.Property(x => x.UserRoleName);
            });

            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("User");
                e.HasKey(x => x.UserId);
                e.Property(x => x.Name);
                e.Property(x => x.Email);
                e.Property(x => x.Password);
                e.Property(x => x.Status);
                e.Property(x => x.IsDeleted);
                e.HasOne(x => x.UserRole)
                    .WithMany(x => x.ListUser)
                    .HasForeignKey(x => x.RoleId)
                    .HasConstraintName("FK_User_UserRole");
            });

            modelBuilder.Entity<Voucher>(e =>
            {
                e.ToTable("Voucher");
                e.HasKey(x => x.VoucherId);
                e.Property(x => x.VoucherName);
                e.Property(x => x.VoucherValue).HasColumnType("decimal(18, 2)");
                e.Property(x => x.MinimumPrice).HasColumnType("decimal(18, 2)");
                e.Property(x => x.CreationDate);
                e.Property(x => x.ExpiryDate);
            });

            modelBuilder.Entity<Order>(e =>
            {
                e.ToTable("Order");
                e.HasKey(x => x.OrderId);
                e.Property(x => x.OrderDate).IsRequired();
                e.Property(x => x.Status);
                e.Property(x => x.OrderTotalAmount).IsRequired();
                e.Property(x => x.isDeleted).IsRequired();
                e.Property(x => x.RefundStatus)
                    .HasConversion(
                        v => v.ToString(),
                        v => (RefundStatus)Enum.Parse(typeof(RefundStatus), v)
                    )
                    .HasMaxLength(20);
                e.Property(x => x.RefundRequestDate);
                e.HasOne(x => x.User)
                    .WithMany(x => x.ListOrder)
                    .HasForeignKey(x => x.UserId)
                    .HasConstraintName("FK_Order_User");
                e.HasOne(x => x.Voucher)
                    .WithMany(x => x.ListOrders)
                    .HasForeignKey(x => x.VoucherId)
                    .HasConstraintName("FK_Order_Voucher")
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<OrderDetail>(e =>
            {
                e.ToTable("OrderDetail");
                e.HasKey(x => x.OrderDetailId);
                e.Property(x => x.ProductQuantity);
                e.Property(x => x.ProductPrice);
                e.HasOne(x => x.Order)
                    .WithMany(x => x.ListOrderDetail)
                    .HasForeignKey(x => x.OrderId)
                    .HasConstraintName("FK_OrderDetail_Order");
                e.HasOne<Product>(x => x.Product)
                    .WithMany(x => x.ListOrderDetail)
                    .HasForeignKey(x => x.ProductId)
                    .HasConstraintName("FK_OrderDetail_Product");
            });

            modelBuilder.Entity<ProductLine>(e =>
            {
                e.ToTable("ProductLine");
                e.HasKey(x => x.ProductLineId);
                e.Property(x => x.QuantityIn);
                e.Property(x => x.QuantityOut);
                e.Property(x => x.ExpiredDate);
                e.HasOne(x => x.Product)
                    .WithMany(x => x.ListProductLine)
                    .HasForeignKey(x => x.ProductId)
                    .HasConstraintName("FK_ProductLine_Product");
            });

            modelBuilder.Entity<Product>(e =>
            {
                e.ToTable("Product");
                e.HasKey(x => x.ProductId);
                e.Property(x => x.Name);
                e.Property(x => x.Description);
                e.Property(x => x.Price);
                e.Property(x => x.Image);
                e.Property(x => x.Status);
                e.Property(x => x.IsDeleted);
                e.HasOne(x => x.ProductBrand)
                    .WithMany(x => x.ListProduct)
                    .HasForeignKey(x => x.ProductBrandId)
                    .HasConstraintName("FK_Product_ProductBrand");
                e.HasOne(x => x.ProductCategory)
                    .WithMany(x => x.ListProduct)
                    .HasForeignKey(x => x.ProductCategoryId)
                    .HasConstraintName("FK_Product_ProductCategory");
            });

            modelBuilder.Entity<ProductBrand>(e =>
            {
                e.ToTable("ProductBrand");
                e.HasKey(x => x.ProductBrandId);
                e.Property(x => x.Name);
            });

            modelBuilder.Entity<ProductCategory>(e =>
            {
                e.ToTable("ProductCategory");
                e.HasKey(x => x.ProductCategoryId);
                e.Property(x => x.Name);
            });

            modelBuilder.Entity<UserVoucher>(e =>
            {
                e.ToTable("UserVoucher");
                e.HasKey(x => x.UserVoucherId);
                e.Property(x => x.RedemptionDate);
                e.HasOne(x => x.User)
                    .WithMany(x => x.UserVouchers)
                    .HasForeignKey(x => x.UserId)
                    .HasConstraintName("FK_UserVoucher_User");
                e.HasOne(x => x.Voucher)
                    .WithMany(x => x.UserVouchers)
                    .HasForeignKey(x => x.VoucherId)
                    .HasConstraintName("FK_UserVoucher_Voucher");
            });
        }
    }
}
