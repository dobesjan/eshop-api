using Eshop.Api.DataAccess.Data;
using Eshop.Api.DataAccess.Repository.Contacts;
using Eshop.Api.DataAccess.Repository.Currencies;
using Eshop.Api.DataAccess.Repository.Orders;
using Eshop.Api.DataAccess.Repository;
using Eshop.Api.Models.Contacts;
using Eshop.Api.Models.Images;
using Eshop.Api.Models.Orders;
using Eshop.Api.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eshop.Api.DataAccess.Repository.Products;

namespace Eshop.Api.DataAccess.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private ApplicationDbContext _context;

		public ICategoryRepository CategoryRepository { get; private set; }
		public IProductRepository ProductRepository { get; private set; }
		public IProductCategoryRepository ProductCategoryRepository { get; private set; }
		public IProductImageRepository ProductImageRepository { get; private set; }
		public IRepository<ProductPriceList> ProductPriceListRepository { get; private set; }

		public IRepository<ImageGroup> ImageGroupRepository { get; private set; }
		public IRepository<Image> ImageRepository { get; private set; }

		public ICurrencyPreferenceRepository CurrencyPreferenceRepository { get; private set; }
		public ICurrencyRepository CurrencyRepository { get; private set; }

		public IOrderRepository OrderRepository { get; private set; }
		public IRepository<OrderStatus> OrderStatusRepository { get; private set; }
		public IOrderProductRepository OrderProductRepository { get; private set; }

		public IShippingRepository ShippingRepository { get; private set; }

		public IRepository<Payment> PaymentRepository { get; private set; }
		public IPaymentMethodRepository PaymentMethodRepository { get; private set; }
		public IRepository<PaymentStatus> PaymentStatusRepository { get; private set; }

		public IAddressRepository AddressRepository { get; private set; }
		public IRepository<Person> PersonRepository { get; private set; }
		public ICustomerRepository CustomerRepository { get; private set; }

		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
			CategoryRepository = new CategoryRepository(context);
			ProductRepository = new ProductRepository(context);
			ProductCategoryRepository = new ProductCategoryRepository(context);
			ProductImageRepository = new ProductImageRepository(context);
			ProductPriceListRepository = new Repository<ProductPriceList>(context);

			ImageGroupRepository = new Repository<ImageGroup>(context);
			ImageRepository = new Repository<Image>(context);

			CurrencyPreferenceRepository = new CurrencyPreferenceRepository(context);
			CurrencyRepository = new CurrencyRepository(context);

			OrderRepository = new OrderRepository(context);
			OrderStatusRepository = new Repository<OrderStatus>(context);
			OrderProductRepository = new OrderProductRepository(context);

			ShippingRepository = new ShippingRepository(context);

			PaymentRepository = new Repository<Payment>(context);
			PaymentMethodRepository = new PaymentMethodRepository(context);
			PaymentStatusRepository = new Repository<PaymentStatus>(context);

			AddressRepository = new AddressRepository(context);
			PersonRepository = new Repository<Person>(context);
			CustomerRepository = new CustomerRepository(context);
		}
	}
}
