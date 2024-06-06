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

			if (preference == null)
			{
				//TODO: Return default currency for eshop from configuration
				return _unitOfWork.CurrencyRepository.GetAll().First();
			}

			return _unitOfWork.CurrencyRepository.GetPrefferedCurrency(preference);
		}

		public bool StorePreferedCurrency(int userId, int currencyId)
		{
			_unitOfWork.CurrencyPreferenceRepository.StorePreferedCurrency(userId, currencyId);
			return true;
		}
	}
}
