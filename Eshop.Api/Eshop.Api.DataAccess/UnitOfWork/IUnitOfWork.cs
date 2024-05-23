using Eshop.Api.DataAccess.Data;
using Eshop.Api.DataAccess.Repository;
using Eshop.Api.DataAccess.Repository.Contacts;
using Eshop.Api.DataAccess.Repository.Currencies;
using Eshop.Api.DataAccess.Repository.Orders;
using Eshop.Api.DataAccess.Repository.Products;
using Eshop.Api.Models.Contacts;
using Eshop.Api.Models.Images;
using Eshop.Api.Models.Orders;
using Eshop.Api.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.UnitOfWork
{
	public interface IUnitOfWork
	{
		IRepository<Category> CategoryRepository { get; }
		IProductRepository ProductRepository { get; }
		IRepository<ProductCategory> ProductCategoryRepository { get; }
		IProductImageRepository ProductImageRepository { get; }
		IRepository<ProductPriceList> ProductPriceListRepository { get; }

		IRepository<ImageGroup> ImageGroupRepository { get; }
		IRepository<Image> ImageRepository { get; }

		ICurrencyPreferenceRepository CurrencyPreferenceRepository { get; }
		ICurrencyRepository CurrencyRepository { get; }

		IOrderRepository OrderRepository { get; }
		IRepository<OrderStatus> OrderStatusRepository { get; }
		IOrderProductRepository OrderProductRepository { get; }

		IShippingRepository ShippingRepository { get; }

		IRepository<Payment> PaymentRepository { get; }
		IPaymentMethodRepository PaymentMethodRepository { get; }
		IRepository<PaymentStatus> PaymentStatusRepository { get; }

		IAddressRepository AddressRepository { get; }
		IRepository<Person> PersonRepository { get; }
		ICustomerRepository CustomerRepository { get; }
	}
}
