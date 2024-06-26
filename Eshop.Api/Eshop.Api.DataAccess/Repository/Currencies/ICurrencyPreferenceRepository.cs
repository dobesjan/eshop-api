﻿using Eshop.Api.Models.Currencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Currencies
{
    public interface ICurrencyPreferenceRepository : IRepository<CurrencyPreference>
    {
        CurrencyPreference GetCurrencyPreference(int userId);
        CurrencyPreference StorePreferedCurrency(int userId, int currencyId);
    }
}
