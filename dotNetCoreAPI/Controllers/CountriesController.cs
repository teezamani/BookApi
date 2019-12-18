using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNetCoreAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace dotNetCoreAPI.Controllers
{
    [Route("api/[countroller]")]
    [ApiController]
    public class CountriesController : Controller
    {
        private ICountryRepository _countryRepository;
        public CountriesController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        //api/countries
        [HttpGet]
        public IActionResult GetCountries()
        {
            var countries = _countryRepository.GetCountries().ToList();
            return Ok(countries);
        }
    }
}