using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Tokens
{
	public class TokenService : EshopService, ITokenService
	{
		public string GenerateSessionToken()
		{
			var guid = Guid.NewGuid().ToString();

			using (var rng = new RNGCryptoServiceProvider())
			{
				var randomNumber = new byte[32];
				rng.GetBytes(randomNumber);
				var secureRandom = Convert.ToBase64String(randomNumber);

				return $"{guid}-{secureRandom}";
			}
		}
	}
}
