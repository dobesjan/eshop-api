using Eshop.Api.DataAccess.Repository.Interfaces;
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
		IRepository<Currency> _currencyRepository;
		IRepository<CurrencyPreference> _currencyPreferenceRepository;

		public CurrencyService(IRepository<Currency> currencyRepository, IRepository<CurrencyPreference> currencyPreferenceRepository)
		{
			_currencyRepository = currencyRepository;
			_currencyPreferenceRepository = currencyPreferenceRepository;
		}

		private Currency GetPrefferedCurrencyInternal(CurrencyPreference preference)
		{
			if (preference != null)
			{
				var currency = _currencyRepository.Get(c => c.Id == preference.CurrencyId);
				if (currency == null) throw new ArgumentNullException("Currency not found");
				return currency;
			}

			return _currencyRepository.Get(c => c.Id == 1);
		}

		//TODO: Move to repository
		private CurrencyPreference GetCurrencyPreference(string token)
		{
			return _currencyPreferenceRepository.Get(p => p.Token == token);
		}

		private CurrencyPreference GetCurrencyPreference(int userId)
		{
			return _currencyPreferenceRepository.Get(p => p.UserId == userId);
		}

		public Currency GetPreferedCurrency(int userId)
		{
			var preference = GetCurrencyPreference(userId);
			return GetPrefferedCurrencyInternal(preference);
		}

		public Currency GetPreferedCurrency(string token)
		{
			var preference = GetCurrencyPreference(token);
			return GetPrefferedCurrencyInternal(preference);
		}

		public bool StorePreferedCurrency(int userId, int currencyId)
		{
			var preference = GetCurrencyPreference(userId);
			if (preference != null)
			{
				preference.CurrencyId = currencyId;
				_currencyPreferenceRepository.Update(preference);
				_currencyPreferenceRepository.Save();
				return true;
			}

			preference = new CurrencyPreference
			{
				CurrencyId = currencyId,
				UserId = userId,
			};

			_currencyPreferenceRepository.Add(preference);
			_currencyPreferenceRepository.Save();
			return true;
		}

		public bool StorePreferedCurrency(string token, int currencyId)
		{
			var preference = GetCurrencyPreference(token);
			if (preference != null)
			{
				preference.CurrencyId = currencyId;
				_currencyPreferenceRepository.Update(preference);
				_currencyPreferenceRepository.Save();
				return true;
			}

			preference = new CurrencyPreference
			{
				CurrencyId = currencyId,
				Token = token,
			};

			_currencyPreferenceRepository.Add(preference);
			_currencyPreferenceRepository.Save();
			return true;
		}
	}
}
