using Eshop.Api.DataAccess.Repository.Currencies;
using Eshop.Api.Models.Currencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Currencies
{
    public class CurrencyService : EshopService, ICurrencyService
	{
		ICurrencyRepository _currencyRepository;
		ICurrencyPreferenceRepository _currencyPreferenceRepository;

		public CurrencyService(ICurrencyRepository currencyRepository, ICurrencyPreferenceRepository currencyPreferenceRepository)
		{
			_currencyRepository = currencyRepository;
			_currencyPreferenceRepository = currencyPreferenceRepository;
		}

		public Currency GetPreferedCurrency(int userId)
		{
			var preference = _currencyPreferenceRepository.GetCurrencyPreference(userId);
			return _currencyRepository.GetPrefferedCurrency(preference);
		}

		public Currency GetPreferedCurrency(string token)
		{
			var preference = _currencyPreferenceRepository.GetCurrencyPreference(token);
			return _currencyRepository.GetPrefferedCurrency(preference);
		}

		public bool StorePreferedCurrency(int userId, int currencyId)
		{
			_currencyPreferenceRepository.StorePreferedCurrency(userId, currencyId);
			return true;
		}

		public bool StorePreferedCurrency(string token, int currencyId)
		{
			_currencyPreferenceRepository.StorePreferedCurrency(token, currencyId);
			return true;
		}
	}
}
