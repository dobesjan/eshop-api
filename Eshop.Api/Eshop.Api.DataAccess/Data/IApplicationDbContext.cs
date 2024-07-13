using Eshop.Api.Models.Contacts;
using Eshop.Api.Models.Currencies;
using Eshop.Api.Models.Orders;
using Eshop.Api.Models.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Data
{
	public interface IApplicationDbContext
	{
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductCategory> ProductCategories { get; set; }
		public DbSet<ProductPriceList> ProductPrices { get; set; }

		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderProduct> OrderProducts { get; set; }
		public DbSet<OrderStatus> OrderStatus { get; set; }
		public DbSet<Payment> Payments { get; set; }
		public DbSet<PaymentStatus> PaymentStatuses { get; set; }
		public DbSet<PaymentMethod> PaymentMethods { get; set; }
		public DbSet<Shipping> Shippings { get; set; }
		public DbSet<ShippingPaymentMethod> ShippingPaymentMethods { get; set; }

		public DbSet<Address> Addresses { get; set; }
		public DbSet<Contact> Contacts { get; set; }
		public DbSet<CustomerContact> CustomerContacts { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Person> Persons { get; set; }

		public DbSet<Currency> Currencies { get; set; }
		public DbSet<CurrencyPreference> CurrencyPreferences { get; set; }
	}
}
