using Eshop.Api.DataAccess.Data;
using Eshop.Api.Models.Currencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Currencies
{
    public class CurrencyPreferenceRepository : Repository<CurrencyPreference>, ICurrencyPreferenceRepository
    {
        public CurrencyPreferenceRepository(ApplicationDbContext db) : base(db)
        {
        }

        public CurrencyPreference GetCurrencyPreference(string token)
        {
            return Get(p => p.Token == token);
        }

        public CurrencyPreference GetCurrencyPreference(int userId)
        {
            return Get(p => p.UserId == userId);
        }

        public CurrencyPreference StorePreferedCurrency(string token, int currencyId)
        {
            var preference = GetCurrencyPreference(token);
            if (preference != null)
            {
                preference.CurrencyId = currencyId;
                return Update(preference, true);
            }

            preference = new CurrencyPreference
            {
                CurrencyId = currencyId,
                Token = token,
            };

            return Add(preference, true);
        }

        public CurrencyPreference StorePreferedCurrency(int userId, int currencyId)
        {
            var preference = GetCurrencyPreference(userId);
            if (preference != null)
            {
                preference.CurrencyId = currencyId;
                return Update(preference, true);
            }

            preference = new CurrencyPreference
            {
                CurrencyId = currencyId,
                UserId = userId,
            };

            return Add(preference, true);
        }
    }
}
