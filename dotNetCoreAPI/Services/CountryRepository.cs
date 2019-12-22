using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNetCoreAPI.Model;

namespace dotNetCoreAPI.Services
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _countryContext;

        public CountryRepository(AppDbContext countryContext)
        {
            _countryContext = countryContext;
        }

        public bool CountryExists(int countryId)
        {
            return _countryContext.Countries.Any(c=>c.Id ==countryId);
        }
        public ICollection<Author> GetAuthorsFromACountry(int countryId)
        {
            return _countryContext.Authors.Where(c=>c.Country.Id ==countryId).ToList();
        }

        public ICollection<Country> GetCountries()
        {
            return _countryContext.Countries.OrderBy(c => c.Name).ToList();
        }

        public Country GetCountry(int countryId)
        {
            return _countryContext.Countries.Where(c => c.Id ==countryId).FirstOrDefault();
        }

        public Country GetCountryOfAnAuthor(int authorId)
        {
            return _countryContext.Authors.Where(a => a.Id == authorId).Select(c => c.Country).FirstOrDefault();
        }

        public bool isDuplicateCountryName(int countryId, string countryName)
        {
            var country = _countryContext.Countries.Where(c => c.Name.Trim().ToUpper() == countryName.Trim().ToUpper() && c.Id != countryId).FirstOrDefault();

            return country == null ? false : true;
        }
    }
}
