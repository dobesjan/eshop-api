using Eshop.Api.Models.Orders;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eshop.UI.Models
{
	public class PaymentMethodVM
	{
		public int PaymentMethodId { get; set; }
		[ValidateNever]
		public IEnumerable<SelectListItem> PaymentMethods { get; set; }
	}
}
