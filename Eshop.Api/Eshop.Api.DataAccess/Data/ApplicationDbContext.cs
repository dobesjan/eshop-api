using Eshop.Api.Models.Contacts;
using Eshop.Api.Models.Orders;
using Eshop.Api.Models.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Eshop.Api.DataAccess.Data
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductCategory> ProductCategories { get; set; }
		public DbSet<ProductPriceList> ProductPrices { get; set; }

		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderShipping> OrderShippings { get; set; }
		public DbSet<OrderProduct> OrderProducts { get; set; }
		public DbSet<OrderCustomer> CustomerOrders { get; set; }
		public DbSet<OrderStatus> OrderStatus { get; set; }
		public DbSet<Payment> Payments { get; set; }
		public DbSet<PaymentStatus> PaymentStatuses { get; set; }
		public DbSet<PaymentMethod> PaymentMethods { get; set; }
		public DbSet<Shipping> Shippings { get; set; }
		public DbSet<ShippingPaymentMethod> ShippingPaymentMethods { get; set; }

		public DbSet<Address> Addresses { get; set; }
		public DbSet<AddressType> AddressTypes { get; set; }
		public DbSet<Contact> Contacts { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Person> Persons { get; set; }

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

			modelBuilder.Entity<Order>()
				.HasOne(o => o.DeliveryAddress)
				.WithMany()
				.HasForeignKey(o => o.AddressId)
				.OnDelete(DeleteBehavior.NoAction);

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

			modelBuilder.Entity<OrderCustomer>()
				.HasKey(pc => new { pc.CustomerId, pc.OrderId });

			modelBuilder.Entity<OrderCustomer>()
				.HasOne(pc => pc.Customer)
				.WithMany(p => p.CustomerContacts)
				.HasForeignKey(pc => pc.CustomerId);

			modelBuilder.Entity<OrderCustomer>()
				.HasOne(pc => pc.Order)
				.WithMany(c => c.CustomerContacts)
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

			modelBuilder.Entity<AddressType>().HasData(
				new AddressType { Id = 1, Name = "Billing" },
				new AddressType { Id = 2, Name = "Delivery" }
			);

			modelBuilder.Entity<PaymentMethod>().HasData(
				new PaymentMethod { Id = 1, Name = "Cash on delivery" },
				new PaymentMethod { Id = 2, Name = "Card" }
			);

			modelBuilder.Entity<Shipping>().HasData(
				new Shipping { Id = 1, Name = "DPD" },
				new Shipping { Id = 2, Name = "PPL" }
			);

			modelBuilder.Entity<ShippingPaymentMethod>().HasData(
				new ShippingPaymentMethod { Id = 1, ShippingId = 1, PaymentMethodId = 1 },
				new ShippingPaymentMethod { Id = 2, ShippingId = 1, PaymentMethodId = 2 },
				new ShippingPaymentMethod { Id = 3, ShippingId = 2, PaymentMethodId = 1 }
			);

			modelBuilder.Entity<OrderStatus>().HasData(
				new OrderStatus { Id = 1, Name = "Address filled" },
				new OrderStatus { Id = 2, Name = "Shipping filled" },
				new OrderStatus { Id = 3, Name = "Payment method filled" },
				new OrderStatus { Id = 4, Name = "Order sent" },
				new OrderStatus { Id = 5, Name = "Order in delivery service" },
				new OrderStatus { Id = 6, Name = "Order processed" }
			);
		}
	}
}
