﻿using dotNetCoreAPI.Dtos;
using dotNetCoreAPI.Model;
using dotNetCoreAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : Controller
    {
        private IAuthorRepository _authorRepository;
        private IBookRepository _bookRepository;
        private ICountryRepository _countryRepository;
        public AuthorsController(IAuthorRepository authorRepository, IBookRepository bookRepository, ICountryRepository countryRepository)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _countryRepository = countryRepository;
        }

        //api/authors
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDto>))]
        public IActionResult GetAuthors()
        {
            var authors = _authorRepository.GetAuthors();

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

        //api/authors/authorId
        [HttpGet("{authorId}", Name = "GetAuthor")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(AuthorDto))]
        public IActionResult GetAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();

            var author = _authorRepository.GetAuthor(authorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorDto = new AuthorDto()
          
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                };

            return Ok(authorDto);
        }

        //api/authors/authorId/books
        [HttpGet("{authorId}/books")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        public IActionResult GetAllBooksOfAAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();

            var books = _authorRepository.GetAllBooksOfAAuthor(authorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var booksDto = new List<BookDto>();

            foreach (var book in books)
            {
                booksDto.Add(new BookDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Isbn = book.Isbn,
                    DatePublished = book.DatePublished

                });

            }

            return Ok(booksDto);

        }


        //api/authors/books/bookId
        [HttpGet("books/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDto>))]
        public IActionResult GetAllAuthorsOfABook(int bookId)
        {
         
           if (!_bookRepository.BookExists(bookId))
                return NotFound();

            var authors = _authorRepository.GetAllAuthorsOfABook(bookId);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorsDto = new List<AuthorDto>();
            foreach (var author in authors)
            {
                authorsDto.Add(new AuthorDto()
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                });
            }

            return Ok(authorsDto);
        }

        //api/authors
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Author))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateAuthor([FromBody]Author authorToCreate)
        {
            if (authorToCreate == null)
                return BadRequest(ModelState);

            if (!_countryRepository.CountryExists(authorToCreate.Country.Id))
                ModelState.AddModelError("", "Country doesn't exist !");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            authorToCreate.Country = _countryRepository.GetCountry(authorToCreate.Country.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_authorRepository.CreateAuthor(authorToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving {authorToCreate.FirstName} {authorToCreate.LastName}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetAuthor", new { authorId = authorToCreate.Id }, authorToCreate);
        }


        //api/authors/authorId
        [HttpPut("{authorId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateAuthor(int authorId, [FromBody]Author authorToUpdate)
        {
            if (authorToUpdate == null)
                return BadRequest(ModelState);

            if (authorId != authorToUpdate.Id)
                return BadRequest(ModelState);

            if (!_authorRepository.AuthorExists(authorId))
                ModelState.AddModelError("", "Author doesn't exist !");


            if (!_authorRepository.AuthorExists(authorToUpdate.Country.Id))
                ModelState.AddModelError("", "Country doesn't exist !");

            if (!ModelState.IsValid)
                return StatusCode(404, ModelState);

            authorToUpdate.Country = _countryRepository.GetCountry(authorToUpdate.Country.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_authorRepository.CreateAuthor(authorToUpdate))
            {
                ModelState.AddModelError("", $"Something went wrong saving {authorToUpdate.FirstName} {authorToUpdate.LastName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        //api/authors/authorId
        [HttpDelete("{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]//No Content
        public IActionResult DeleteAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();

            var authorToDelete = _authorRepository.GetAuthor(authorId);

            if (_authorRepository.GetAllBooksOfAAuthor(authorId).Count() > 0)
            {
                ModelState.AddModelError("", $"Author {authorToDelete.FirstName}" +
                    $" cannot be deleted because it is used by at least one book");
                return StatusCode(409, $"Category {authorToDelete.FirstName}" +
                    $" cannot be deleted because it is used by at least one book");
            }


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_authorRepository.DeleteAuthor(authorToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong saving {authorToDelete.FirstName}");
                return StatusCode(500, $"Something went wrong saving {authorToDelete.FirstName}");
            }

            return NoContent();
        }

    }
}


