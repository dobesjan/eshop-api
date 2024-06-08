using Eshop.Api.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Orders
{
	public interface IPaymentMethodRepository : IRepository<PaymentMethod>
	{
		bool IsPaymentMethodEnabled(int id);
        bool IsPaymentMethodSupported(int id, int shippingId);
        IEnumerable<PaymentMethod> GetPaymentMethodsForRepository(int shippingId);
	}
}
