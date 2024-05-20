using Eshop.Api.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.ViewModels.Orders
{
	public class PaymentVM
	{
		public int UserId { get; set; }
		public string Token { get; set; }

		public Payment Payment { get; set; }
	}
}
