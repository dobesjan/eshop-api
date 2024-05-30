using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Tokens
{
	public interface ITokenService : IEshopService
	{
		string GenerateSessionToken();
	}
}
