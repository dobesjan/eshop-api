using Eshop.Api.BusinessLayer.Services.Tokens;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Cryptography;

namespace Eshop.UI.ActionFilters
{
	public class SessionTokenActionFilter : IAsyncActionFilter
	{
		private readonly ITokenService _tokenService;
		private readonly string _tokenVariableName = "SessionToken";

		public SessionTokenActionFilter(ITokenService tokenService) 
		{
			_tokenService = tokenService;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var session = context.HttpContext.Session;
			var sessionToken = session.GetString(_tokenVariableName);

			if (string.IsNullOrEmpty(sessionToken))
			{
				sessionToken = _tokenService.GenerateSessionToken();

				session.SetString(_tokenVariableName, sessionToken);
			}

			context.HttpContext.Items[_tokenVariableName] = sessionToken;

			await next();
		}
	}
}
