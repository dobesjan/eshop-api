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
		public PaymentMethodRepository(ApplicationDbContext db) : base(db)
		{
		}

		public bool IsPaymentMethodEnabled(int id)
		{
			return Get(p => p.Enabled && p.Id == id) != null;
		}
	}
}
