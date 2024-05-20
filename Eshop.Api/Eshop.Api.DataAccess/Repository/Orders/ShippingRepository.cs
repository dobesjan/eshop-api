using Eshop.Api.DataAccess.Data;
using Eshop.Api.Models.Orders;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Orders
{
	public class ShippingRepository : Repository<Shipping>, IShippingRepository
	{
		private readonly string _properties = "ShippingPaymentMethod.PaymentMethod";

		public ShippingRepository(ApplicationDbContext db) : base(db)
		{
		}

		public Shipping GetEnabledShipping(int id)
		{
			return Get(s => s.Id == id && s.Enabled == true && s.ShippingPaymentMethod != null, includeProperties: _properties);
		}
	}
}
