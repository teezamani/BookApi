using dotNetCoreAPI.Dtos;
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
    public class CategoriesController : Controller
    {
        private ICategoryRepository _categoryRepository;
        private IBookRepository _bookRepository;
        public CategoriesController(ICategoryRepository categoryRepository, IBookRepository bookRepository)
        {
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
        }


        //api/categories
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoriesDto = new List<CategoryDto>();
            foreach (var category in categories)
            {
                categoriesDto.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }
            return Ok(categoriesDto);
        }


        //api/categories/Id
        [HttpGet("{categoryId}", Name = "GetCategory")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var category = _categoryRepository.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryDto = new CategoryDto()
                          
                {
                    Id = category.Id,
                    Name = category.Name
                };
        
            return Ok(categoryDto);
        }

        
        //api/categories/books/bookId
        [HttpGet("books/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        public IActionResult GetAllCategoriesForABook(int bookId)
        {
            if (!_bookRepository.BookExists(bookId))
                return NotFound();

            var categories = _categoryRepository.GetAllCategoriesForABook(bookId);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoriesDto = new List<CategoryDto>();
            foreach (var category  in categories)
            {
                categoriesDto.Add(new CategoryDto()
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }

            return Ok(categoriesDto);
        }


        //To Do GetAllBooksForACategory

        //api/categories/categoryId/books
        [HttpGet("{categoryId}/books")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        public IActionResult GetAllBooksForACategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var books = _categoryRepository.GetAllBooksForACategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var booksDto = new List<BookDto>();

            foreach (var book in books)
            {
                booksDto.Add(new BookDto
                {
                    Id = book.Id,
                    Title =book.Title,
                    Isbn = book.Isbn,
                    DatePublished = book.DatePublished

                });

            }

            return Ok(booksDto);

        }

        //api/categories
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Category))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateCategory([FromBody]Category categoryToCreate)
        {
            if (categoryToCreate == null)
                return BadRequest(ModelState);

            var category = _categoryRepository.GetCategories()
                .Where(c => c.Name.Trim().ToUpper() == categoryToCreate.Name
                .Trim().ToUpper()).FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("", $"Category { categoryToCreate.Name } already exists");
                return StatusCode(422, $"Category { categoryToCreate.Name } already exists");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.CreateCategory(categoryToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving {categoryToCreate.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategory", new { categoryId = categoryToCreate.Id }, categoryToCreate);
        }

        //api/categories/categoryId
        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]//No Content
        public IActionResult UpdateCategory(int categoryId, [FromBody]Category updatedCategoryInfo)
        {
            if (updatedCategoryInfo == null)
                return BadRequest(ModelState);

            if (categoryId != updatedCategoryInfo.Id)
                return BadRequest(ModelState);

            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            if (_categoryRepository.isDuplicateCategoryName(categoryId, updatedCategoryInfo.Name))
            {
                ModelState.AddModelError("", $"Category {updatedCategoryInfo.Name} already exists");
                return StatusCode(422, $"Category {updatedCategoryInfo.Name} already exists");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.UpdateCategory(updatedCategoryInfo))
            {
                ModelState.AddModelError("", $"Something went wrong saving {updatedCategoryInfo.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //api/categories/categoryId
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]//No Content
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var categoryToDelete = _categoryRepository.GetCategory(categoryId);

            if (_categoryRepository.GetAllBooksForACategory(categoryId).Count() > 0)
            {
                ModelState.AddModelError("", $"Category {categoryToDelete.Name}" +
                    $" cannot be deleted because it is used by at least one author");
                return StatusCode(409, $"Category {categoryToDelete.Name}" +
                    $" cannot be deleted because it is used by at least one book");
            }


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong saving {categoryToDelete.Name}");
                return StatusCode(500, $"Something went wrong saving {categoryToDelete.Name}");
            }

            return NoContent();
        }
    }
}
