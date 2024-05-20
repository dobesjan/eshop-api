using Eshop.Api.Models.Currencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Currencies
{
    public interface ICurrencyRepository : IRepository<Currency>
    {
        Currency GetPrefferedCurrency(CurrencyPreference preference);
    }
}
