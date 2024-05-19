using BussinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class M_BMilkStoreDBContext : DbContext
    {
        public M_BMilkStoreDBContext()
        {

        }
        public M_BMilkStoreDBContext(DbContextOptions<M_BMilkStoreDBContext> options) : base(options)
        {
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductBrand> ProductBrands { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
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
            var strConn = /*config["ConnectionStrings:DB"]*/ config.GetConnectionString("DB");

            return strConn;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("User");
                e.HasKey(x => x.Id);
                e.Property(x => x.Name);
                e.Property(x => x.Email);
                e.Property(x => x.Password);
                e.Property(x => x.Role);
            });
            modelBuilder.Entity<Order>(e =>
            {
                e.ToTable("Order");
                e.HasKey(x => x.Id);
                e.Property(x => x.OrderDate);
                e.Property(x => x.Status);
                e.Property(x => x.OrderTotalAmount);
                e.HasOne(x => x.User)
                    .WithMany(x => x.ListOrder)
                    .HasForeignKey(x => x.UserId)
                    .HasConstraintName("FK_Order_User");

            });
            modelBuilder.Entity<OrderDetail>(e =>
            {
                e.ToTable("OrderDetail");
                e.HasKey(x => x.Id);
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
            modelBuilder.Entity<Product>(e =>
            {
                e.ToTable("Product");
                e.HasKey(x => x.Id);
                e.Property(x => x.Name);
                e.Property(x => x.Description);
                e.Property(x => x.Price);
                e.Property(x => x.Image);
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
                e.HasKey(x => x.Id);
                e.Property(x => x.Name);
            });
            modelBuilder.Entity<ProductCategory>(e =>
            {
                e.ToTable("ProductCategory");
                e.HasKey(x => x.Id);
                e.Property(x => x.Name);
            });
        }
    }
}
