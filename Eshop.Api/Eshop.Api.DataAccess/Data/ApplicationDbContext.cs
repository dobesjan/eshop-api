using Eshop.Api.Models.Order;
using Eshop.Api.Models.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Data
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductCategory> ProductCategories { get; set; }

		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderShipping> OrderShippings { get; set; }
		public DbSet<OrderProduct> OrderProducts { get; set; }
		public DbSet<OrderStatus> OrderStatus { get; set; }
		public DbSet<Payment> Payments { get; set; }
		public DbSet<PaymentStatus> PaymentStatuses { get; set; }
		public DbSet<Shipping> Shippings { get; set; }
		public DbSet<ShippingPaymentMethod> ShippingPaymentMethods { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ProductCategory>()
				.HasKey(pc => new { pc.ProductId, pc.CategoryId });

			modelBuilder.Entity<ProductCategory>()
				.HasOne(pc => pc.Product)
				.WithMany(p => p.ProductCategories)
				.HasForeignKey(pc => pc.ProductId);

			modelBuilder.Entity<ProductCategory>()
				.HasOne(pc => pc.Category)
				.WithMany(c => c.ProductCategories)
				.HasForeignKey(pc => pc.CategoryId);

			modelBuilder.Entity<OrderProduct>()
				.HasKey(pc => new { pc.ProductId, pc.OrderId });

			modelBuilder.Entity<OrderProduct>()
				.HasOne(pc => pc.Product)
				.WithMany(p => p.OrderProducts)
				.HasForeignKey(pc => pc.ProductId);

			modelBuilder.Entity<OrderProduct>()
				.HasOne(pc => pc.Order)
				.WithMany(c => c.OrderProducts)
				.HasForeignKey(pc => pc.OrderId);

			modelBuilder.Entity<OrderShipping>()
				.HasKey(pc => new { pc.ShippingId, pc.OrderId });

			modelBuilder.Entity<OrderShipping>()
				.HasOne(pc => pc.Shipping)
				.WithMany(p => p.OrderShipping)
				.HasForeignKey(pc => pc.ShippingId);

			modelBuilder.Entity<OrderShipping>()
				.HasOne(pc => pc.Order)
				.WithMany(c => c.OrderShipping)
				.HasForeignKey(pc => pc.OrderId);

			modelBuilder.Entity<ShippingPaymentMethod>()
				.HasKey(pc => new { pc.PaymentMethodId, pc.ShippingId });

			modelBuilder.Entity<ShippingPaymentMethod>()
				.HasOne(pc => pc.PaymentMethod)
				.WithMany(p => p.ShippingPaymentMethod)
				.HasForeignKey(pc => pc.PaymentMethodId);

			modelBuilder.Entity<ShippingPaymentMethod>()
				.HasOne(pc => pc.Shipping)
				.WithMany(c => c.ShippingPaymentMethod)
				.HasForeignKey(pc => pc.ShippingId);

			modelBuilder.Entity<Category>()
				.HasOne(c => c.ParentCategory)
				.WithMany()
				.HasForeignKey(c => c.ParentCategoryId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
