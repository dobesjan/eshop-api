using Eshop.Api.BusinessLayer.Services.Tokens;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.UI.Controllers
{
	public class EshopBaseController : Controller
	{
		private readonly string _tokenVariableName = "SessionToken";
		public string SessionToken
		{
			get
			{
				return HttpContext.Items[_tokenVariableName] as string;
			}
		}

		public EshopBaseController()
		{
		}
	}
}
