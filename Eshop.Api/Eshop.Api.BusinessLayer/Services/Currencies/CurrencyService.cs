using Eshop.Api.DataAccess.Repository.Currencies;
using Eshop.Api.DataAccess.UnitOfWork;
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
		private IUnitOfWork _unitOfWork;

		public CurrencyService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Currency GetPreferedCurrency(int userId)
		{
			var preference = _unitOfWork.CurrencyPreferenceRepository.GetCurrencyPreference(userId);
			return _unitOfWork.CurrencyRepository.GetPrefferedCurrency(preference);
		}

		public Currency GetPreferedCurrency(string token)
		{
			var preference = _unitOfWork.CurrencyPreferenceRepository.GetCurrencyPreference(token);
			return _unitOfWork.CurrencyRepository.GetPrefferedCurrency(preference);
		}

		public bool StorePreferedCurrency(int userId, int currencyId)
		{
			_unitOfWork.CurrencyPreferenceRepository.StorePreferedCurrency(userId, currencyId);
			return true;
		}

		public bool StorePreferedCurrency(string token, int currencyId)
		{
			_unitOfWork.CurrencyPreferenceRepository.StorePreferedCurrency(token, currencyId);
			return true;
		}
	}
}
