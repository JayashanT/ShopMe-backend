﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using webapi.Entities;

namespace backend_webapi.Migrations
{
    [DbContext(typeof(ShopmeDbContext))]
    [Migration("20190812051456_update1")]
    partial class update1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("backend_webapi.Entities.Login", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("Password");

                    b.Property<string>("Role");

                    b.HasKey("Id");

                    b.ToTable("Login");
                });

            modelBuilder.Entity("webapi.Entities.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<int>("LoginId");

                    b.Property<string>("MobileNumber");

                    b.Property<string>("ProfileImage");

                    b.Property<string>("Qualifications");

                    b.Property<string>("Token");

                    b.HasKey("Id");

                    b.HasIndex("LoginId")
                        .IsUnique();

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("webapi.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName");

                    b.Property<string>("Image");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("webapi.Entities.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<int>("LoginId");

                    b.Property<string>("MobileNumber");

                    b.Property<string>("ProfileImage");

                    b.Property<string>("Token");

                    b.HasKey("Id");

                    b.HasIndex("LoginId")
                        .IsUnique();

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("webapi.Entities.Deliverer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DeliveryStatus");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<int>("LoginId");

                    b.Property<string>("MobileNumber");

                    b.Property<string>("NIC");

                    b.Property<string>("ProfileImage");

                    b.Property<string>("Token");

                    b.Property<string>("VehicleNo");

                    b.Property<string>("VehicleType");

                    b.HasKey("Id");

                    b.HasIndex("LoginId")
                        .IsUnique();

                    b.ToTable("Deliverers");
                });

            modelBuilder.Entity("webapi.Entities.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConnectionId");

                    b.Property<int>("DelivererId");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.HasKey("Id");

                    b.HasIndex("DelivererId")
                        .IsUnique();

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("webapi.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("CustomerId");

                    b.Property<int>("SellerId");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("SellerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("webapi.Entities.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("OrderId");

                    b.Property<int>("Quantity");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("webapi.Entities.OrderItemProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("OrderItemId");

                    b.Property<int>("ProductId");

                    b.HasKey("Id");

                    b.HasIndex("OrderItemId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItemProducts");
                });

            modelBuilder.Entity("webapi.Entities.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("OrderId");

                    b.Property<DateTime>("PaymentDate");

                    b.Property<double>("Price");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("webapi.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId");

                    b.Property<string>("Description");

                    b.Property<double>("Discount");

                    b.Property<string>("Image");

                    b.Property<int>("Like");

                    b.Property<string>("Name");

                    b.Property<int>("Quantity");

                    b.Property<double>("Rating");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("SellerId");

                    b.Property<string>("ShortDescription");

                    b.Property<int>("UnitPrice");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("SellerId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("webapi.Entities.Seller", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountNo");

                    b.Property<string>("ConnectionId");

                    b.Property<string>("FirstName");

                    b.Property<string>("Image");

                    b.Property<string>("LastName");

                    b.Property<int>("LoginId");

                    b.Property<string>("MobileNumber");

                    b.Property<string>("ProfileImage");

                    b.Property<string>("ShopAddress");

                    b.Property<double>("ShopLocationLatitude");

                    b.Property<double>("ShopLocationLongitude");

                    b.Property<string>("ShopName");

                    b.Property<string>("Token");

                    b.HasKey("Id");

                    b.HasIndex("LoginId")
                        .IsUnique();

                    b.ToTable("Sellers");
                });

            modelBuilder.Entity("webapi.Entities.Admin", b =>
                {
                    b.HasOne("backend_webapi.Entities.Login", "login")
                        .WithOne("Admin")
                        .HasForeignKey("webapi.Entities.Admin", "LoginId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("webapi.Entities.Customer", b =>
                {
                    b.HasOne("backend_webapi.Entities.Login", "login")
                        .WithOne("Customer")
                        .HasForeignKey("webapi.Entities.Customer", "LoginId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("webapi.Entities.Deliverer", b =>
                {
                    b.HasOne("backend_webapi.Entities.Login", "login")
                        .WithOne("Deliverer")
                        .HasForeignKey("webapi.Entities.Deliverer", "LoginId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("webapi.Entities.Location", b =>
                {
                    b.HasOne("webapi.Entities.Deliverer", "Deliverer")
                        .WithOne("Location")
                        .HasForeignKey("webapi.Entities.Location", "DelivererId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("webapi.Entities.Order", b =>
                {
                    b.HasOne("webapi.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("webapi.Entities.Seller", "Seller")
                        .WithMany()
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("webapi.Entities.OrderItem", b =>
                {
                    b.HasOne("webapi.Entities.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("webapi.Entities.OrderItemProduct", b =>
                {
                    b.HasOne("webapi.Entities.OrderItem", "OrderItem")
                        .WithMany("OrderItemProducts")
                        .HasForeignKey("OrderItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("webapi.Entities.Product", "Product")
                        .WithMany("OrderItemProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("webapi.Entities.Payment", b =>
                {
                    b.HasOne("webapi.Entities.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("webapi.Entities.Product", b =>
                {
                    b.HasOne("webapi.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("webapi.Entities.Seller", "Seller")
                        .WithMany("Products")
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("webapi.Entities.Seller", b =>
                {
                    b.HasOne("backend_webapi.Entities.Login", "login")
                        .WithOne("Seller")
                        .HasForeignKey("webapi.Entities.Seller", "LoginId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
