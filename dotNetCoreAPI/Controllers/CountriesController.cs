﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNetCoreAPI.Dtos;
using dotNetCoreAPI.Model;
using dotNetCoreAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace dotNetCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : Controller
    {
        private  ICountryRepository _countryRepository;
        private IAuthorRepository _authorRepository;
        public CountriesController(ICountryRepository countryRepository, IAuthorRepository authorRepository)
        {
            _countryRepository = countryRepository;
            _authorRepository = authorRepository;
        }

        //api/countries
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200,Type =typeof(IEnumerable<CountryDto>))]
        public IActionResult GetCountries()
        {
            var countries = _countryRepository.GetCountries().ToList();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countriesDto = new List<CountryDto>();
            foreach (var country in countries)
            {
                countriesDto.Add(new CountryDto
                {
                    Id = country.Id,
                    Name = country.Name
                });
            }
            return Ok(countriesDto);
        }

        //api/countries/countryId
        [HttpGet("{countryId}", Name ="GetCountry")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        public IActionResult GetCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            var country = _countryRepository.GetCountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryDto = new CountryDto()
       
                {
                    Id = country.Id,
                    Name = country.Name
                };
            
            return Ok(countryDto);
        }

        //api/countries/authors/authorId
        [HttpGet("authors/{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        public IActionResult GetCountryOfAnAuthor(int authorId)
        {
          
            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();


            var country = _countryRepository.GetCountryOfAnAuthor(authorId);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryDto = new CountryDto()

            {
                Id = country.Id,
                Name = country.Name
            };

            return Ok(countryDto);
        }


        //api/countries/countryId/authors
        [HttpGet("{countryId}/authors")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(AuthorDto))]
        public IActionResult GetAuthorsFromACountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            var authors = _countryRepository.GetAuthorsFromACountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorsDto = new List<AuthorDto>();

            foreach (var author in authors)
            {
                authorsDto.Add(new AuthorDto
                { 
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                });

            }

            return Ok(authorsDto);

        }

        //api/countries
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Country))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateCountry([FromBody]Country countryToCreate)
        {
            if (countryToCreate == null)
                return BadRequest(ModelState);

            var country = _countryRepository.GetCountries()
                .Where(c => c.Name.Trim().ToUpper() == countryToCreate.Name
                .Trim().ToUpper()).FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", $"Country { countryToCreate.Name } already exists");
                return StatusCode(422, $"Country { countryToCreate.Name } already exists");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepository.CreateCountry(countryToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving {countryToCreate.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCountry", new { countryId = countryToCreate.Id}, countryToCreate);
        }

        //api/countries/countryId
        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]//No Content
        public IActionResult UpdateCountry(int countryId,[FromBody]Country updatedCountryInfo)
        {
            if (updatedCountryInfo == null)
                return BadRequest(ModelState);

            if (countryId != updatedCountryInfo.Id)
                return BadRequest(ModelState);

            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            if(_countryRepository.isDuplicateCountryName(countryId, updatedCountryInfo.Name))
            {
                ModelState.AddModelError("",$"Country {updatedCountryInfo.Name} already exists");
                return StatusCode(422, $"Country {updatedCountryInfo.Name} already exists");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepository.UpdateCountry(updatedCountryInfo))
            {
                ModelState.AddModelError("", $"Something went wrong saving {updatedCountryInfo.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


    }
}
 