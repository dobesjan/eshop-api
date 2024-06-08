using Eshop.Api.DataAccess.Data;
using Eshop.Api.Models.Orders;
using Eshop.Api.Models.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Orders
{
	public class PaymentMethodRepository : Repository<PaymentMethod>, IPaymentMethodRepository
	{
		private readonly string _properties = "ShippingPaymentMethod";

		public PaymentMethodRepository(ApplicationDbContext db) : base(db)
		{
		}

		public bool IsPaymentMethodEnabled(int id)
		{
			return Get(p => p.Enabled == true && p.Id == id, includeProperties: _properties) != null;
		}

		public bool IsPaymentMethodSupported(int id, int shippingId)
		{
            return Get(p => p.Enabled == true && p.Id == id && p.ShippingPaymentMethod.Any(s => s.ShippingId == shippingId), includeProperties: _properties) != null;
        }

        public IEnumerable<PaymentMethod> GetPaymentMethodsForRepository(int shippingId)
		{
			return GetAll(pm => pm.ShippingPaymentMethod != null && pm.ShippingPaymentMethod.Any(s => s.ShippingId == shippingId), includeProperties: _properties);
        }

    }
}
