using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Entities
{
    public class ShopmeDbContext :DbContext
    {
        public ShopmeDbContext(DbContextOptions<ShopmeDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Seller>().ToTable("Sellers");
            modelBuilder.Entity<Deliverer>().ToTable("Deliverers");
            modelBuilder.Entity<Admin>().ToTable("Admins");
        }

        public DbSet<Customer> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Deliverer> Deliverers { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderItemProduct> OrderItemProducts { get; set; }

        public IQueryable<Customer> User { get; internal set; }
        public IQueryable<Customer> Customer { get; internal set; }
        public IQueryable<Deliverer> Deliverer { get; internal set; }
        public IQueryable<Product> Product { get; internal set; }
        public IQueryable<Seller> Seller { get; internal set; }
        public IQueryable<Admin> Admin { get; internal set; }
        public IQueryable<Category> Category { get; internal set; }
        public IQueryable<Location> Location { get; internal set; }
        public IQueryable<Order> Order { get; internal set; }
        public IQueryable<OrderItem> OrderItem { get; internal set; }
        public IQueryable<Payment> Payment { get; internal set; }
        public IQueryable<OrderItemProduct> OrderItemProduct { get; set; }
    }
}
