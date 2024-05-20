using Eshop.Api.DataAccess.Data;
using Eshop.Api.Models.Currencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Currencies
{
    public class CurrencyRepository : Repository<Currency>, ICurrencyRepository
    {
        private const int DefaultCurrencyId = 1;

        public CurrencyRepository(ApplicationDbContext db) : base(db)
        {
        }

        public Currency GetPrefferedCurrency(CurrencyPreference preference)
        {
            if (preference != null)
            {
                var currency = Get(c => c.Id == preference.CurrencyId);
                if (currency == null) throw new ArgumentNullException("Currency not found");
                return currency;
            }

            return Get(c => c.Id == DefaultCurrencyId);
        }
    }
}
