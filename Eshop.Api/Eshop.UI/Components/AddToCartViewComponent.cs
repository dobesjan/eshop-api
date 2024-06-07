using Eshop.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.UI.Components
{
	public class AddToCartViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke(AddToCartVM model)
		{
			return View(model);
		}
	}
}
