﻿// <auto-generated />
using System;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(M_BMilkStoreDBContext))]
    partial class M_BMilkStoreDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BussinessObject.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("OrderTotalAmount")
                        .HasColumnType("real");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("VoucherId")
                        .HasColumnType("int");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("OrderId");

                    b.HasIndex("UserId");

                    b.HasIndex("VoucherId");

                    b.ToTable("Order", (string)null);
                });

            modelBuilder.Entity("BussinessObject.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderDetailId"));

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<double>("ProductPrice")
                        .HasColumnType("float");

                    b.Property<int>("ProductQuantity")
                        .HasColumnType("int");

                    b.HasKey("OrderDetailId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetail", (string)null);
                });

            modelBuilder.Entity("BussinessObject.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("ProductBrandId")
                        .HasColumnType("int");

                    b.Property<int>("ProductCategoryId")
                        .HasColumnType("int");

                    b.Property<bool?>("Status")
                        .HasColumnType("bit");

                    b.HasKey("ProductId");

                    b.HasIndex("ProductBrandId");

                    b.HasIndex("ProductCategoryId");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("BussinessObject.ProductBrand", b =>
                {
                    b.Property<int>("ProductBrandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductBrandId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductBrandId");

                    b.ToTable("ProductBrand", (string)null);
                });

            modelBuilder.Entity("BussinessObject.ProductCategory", b =>
                {
                    b.Property<int>("ProductCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductCategoryId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductCategoryId");

                    b.ToTable("ProductCategory", (string)null);
                });

            modelBuilder.Entity("BussinessObject.ProductLine", b =>
                {
                    b.Property<int>("ProductLineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductLineId"));

                    b.Property<DateTime>("ExpiredDate")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<bool?>("Status")
                        .HasColumnType("bit");

                    b.HasKey("ProductLineId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductLine", (string)null);
                });

            modelBuilder.Entity("BussinessObject.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("BussinessObject.UserRole", b =>
                {
                    b.Property<int>("UserRoleId")
                        .HasColumnType("int");

                    b.Property<string>("UserRoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserRoleId");

                    b.ToTable("UserRole", (string)null);
                });

            modelBuilder.Entity("BussinessObject.UserVoucher", b =>
                {
                    b.Property<int>("UserVoucherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserVoucherId"));

                    b.Property<DateTime>("RedemptionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("VoucherId")
                        .HasColumnType("int");

                    b.HasKey("UserVoucherId");

                    b.HasIndex("UserId");

                    b.HasIndex("VoucherId");

                    b.ToTable("UserVoucher", (string)null);
                });

            modelBuilder.Entity("BussinessObject.Voucher", b =>
                {
                    b.Property<int>("VoucherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VoucherId"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<decimal>("MinimumPrice")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("VoucherName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("VoucherValue")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("VoucherId");

                    b.ToTable("Voucher", (string)null);
                });

            modelBuilder.Entity("BussinessObject.Order", b =>
                {
                    b.HasOne("BussinessObject.User", "User")
                        .WithMany("ListOrder")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Order_User");

                    b.HasOne("BussinessObject.Voucher", "Voucher")
                        .WithMany("ListOrders")
                        .HasForeignKey("VoucherId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_Order_Voucher");

                    b.Navigation("User");

                    b.Navigation("Voucher");
                });

            modelBuilder.Entity("BussinessObject.OrderDetail", b =>
                {
                    b.HasOne("BussinessObject.Order", "Order")
                        .WithMany("ListOrderDetail")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_OrderDetail_Order");

                    b.HasOne("BussinessObject.Product", "Product")
                        .WithMany("ListOrderDetail")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_OrderDetail_Product");

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("BussinessObject.Product", b =>
                {
                    b.HasOne("BussinessObject.ProductBrand", "ProductBrand")
                        .WithMany("ListProduct")
                        .HasForeignKey("ProductBrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Product_ProductBrand");

                    b.HasOne("BussinessObject.ProductCategory", "ProductCategory")
                        .WithMany("ListProduct")
                        .HasForeignKey("ProductCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Product_ProductCategory");

                    b.Navigation("ProductBrand");

                    b.Navigation("ProductCategory");
                });

            modelBuilder.Entity("BussinessObject.ProductLine", b =>
                {
                    b.HasOne("BussinessObject.Product", "Product")
                        .WithMany("ListProductLine")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ProductLine_Product");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("BussinessObject.User", b =>
                {
                    b.HasOne("BussinessObject.UserRole", "UserRole")
                        .WithMany("ListUser")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_User_UserRole");

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("BussinessObject.UserVoucher", b =>
                {
                    b.HasOne("BussinessObject.User", "User")
                        .WithMany("UserVouchers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_UserVoucher_User");

                    b.HasOne("BussinessObject.Voucher", "Voucher")
                        .WithMany("UserVouchers")
                        .HasForeignKey("VoucherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_UserVoucher_Voucher");

                    b.Navigation("User");

                    b.Navigation("Voucher");
                });

            modelBuilder.Entity("BussinessObject.Order", b =>
                {
                    b.Navigation("ListOrderDetail");
                });

            modelBuilder.Entity("BussinessObject.Product", b =>
                {
                    b.Navigation("ListOrderDetail");

                    b.Navigation("ListProductLine");
                });

            modelBuilder.Entity("BussinessObject.ProductBrand", b =>
                {
                    b.Navigation("ListProduct");
                });

            modelBuilder.Entity("BussinessObject.ProductCategory", b =>
                {
                    b.Navigation("ListProduct");
                });

            modelBuilder.Entity("BussinessObject.User", b =>
                {
                    b.Navigation("ListOrder");

                    b.Navigation("UserVouchers");
                });

            modelBuilder.Entity("BussinessObject.UserRole", b =>
                {
                    b.Navigation("ListUser");
                });

            modelBuilder.Entity("BussinessObject.Voucher", b =>
                {
                    b.Navigation("ListOrders");

                    b.Navigation("UserVouchers");
                });
#pragma warning restore 612, 618
        }
    }
}
