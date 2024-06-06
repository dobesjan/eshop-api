using Eshop.Api.DataAccess.Data;
using Eshop.Api.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Contacts
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(ApplicationDbContext db) : base(db)
        {
        }

        public IEnumerable<Country> GetCountries(bool enabled = true)
        {
            return GetAll(c => c.IsEnabled == enabled);
        }

        public Country GetCountry(int id, bool enabled = true)
        {
            return Get(c => c.Id == id && c.IsEnabled == enabled);
        }
    }
}
