using Eshop.Api.BusinessLayer.Services.Contacts;
using Eshop.Api.BusinessLayer.Services.Currencies;
using Eshop.Api.BusinessLayer.Services.Images;
using Eshop.Api.BusinessLayer.Services.Orders;
using Eshop.Api.BusinessLayer.Services.Products;
using Eshop.Api.BusinessLayer.Services.Tokens;
using Eshop.Api.BusinessLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer
{
	public static class BusinessLayerServiceCollectionExtensions
	{
		public static IServiceCollection AddEshopServices(this IServiceCollection services)
		{
			ArgumentNullException.ThrowIfNull(services);

			services.AddScoped<IEshopService, EshopService>();
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IImageService, ImageService>();

			services.AddScoped<IOrderService, OrderService>();
			services.AddScoped<ICurrencyService, CurrencyService>();
			services.AddScoped<ICustomerService, CustomerService>();

			services.AddScoped<ITokenService, TokenService>();

			return services;
		}
	}
}
