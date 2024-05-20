using Eshop.Api.BusinessLayer.Services.Interfaces;
using Eshop.Api.Models.Currencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Currencies
{
	public interface ICurrencyService : IEshopService
	{
		Currency GetPreferedCurrency(int userId);
		Currency GetPreferedCurrency(string token);
		bool StorePreferedCurrency(int userId, int currencyId);
		bool StorePreferedCurrency(string token, int currencyId);
	}
}
