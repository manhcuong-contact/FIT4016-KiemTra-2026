using Microsoft.EntityFrameworkCore;
using OrderManagementApp.Models;

namespace OrderManagementApp.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(p => p.Sku).IsUnique();

            modelBuilder.Entity<Order>().HasIndex(o => o.CustomerEmail).IsUnique();

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed 15 sản phẩm
            var products = Enumerable.Range(1, 15).Select(i => new Product
            {
                Id = i,
                Name = $"Product {i}",
                Sku = $"SKU{i:D3}",
                Price = 100 + i * 10,
                StockQuantity = 50 + i * 5,
                Category = i % 2 == 0 ? "Electronics" : "Accessories",
                Description = $"Sample product {i}",
                CreatedAt = new DateTime(2026, 01, 01)
            }).ToArray();
            modelBuilder.Entity<Product>().HasData(products);

            // Seed 30 đơn hàng
            var orders = Enumerable.Range(1, 30).Select(i => new Order
            {
                Id = i,
                ProductId = (i % 15) + 1,
                CustomerName = $"Customer {i}",
                CustomerEmail = $"customer{i}@example.com",
                Quantity = (i % 5) + 1,
                OrderDate = new DateTime(2026, 01, (i % 28) + 1),
                CreatedAt = new DateTime(2026, 01, (i % 28) + 1)
            }).ToArray();
            modelBuilder.Entity<Order>().HasData(orders);
        }
    }
}
