using Eshop.Api.Models.Orders;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eshop.UI.Models
{
	public class ShippingVM
	{
		public int ShippingId { get; set; }

		[ValidateNever]
		public IEnumerable<SelectListItem> ShippingOptions { get; set; }
	}
}
