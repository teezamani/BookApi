using dotNetCoreAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetCoreAPI.Services
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly AppDbContext _categoryContext;

        public CategoryRepository(AppDbContext categoryContext)
        {
            _categoryContext = categoryContext;
        }

        public bool CategoryExists(int categoryId)
        {
            return _categoryContext.Categories.Any(c => c.Id == categoryId);
        }

        public ICollection<Book> GetAllBooksForACategory(int categoryId)
        {
            return _categoryContext.BookCategories.Where(c => c.CategoryId == categoryId).Select(b => b.Book).ToList();
        }

        public ICollection<Category> GetCategories()
        {
            return _categoryContext.Categories.OrderBy(c => c.Name).ToList();
        }

        public ICollection<Category> GetAllCategoriesForABook(int bookId)
        {
            return _categoryContext.BookCategories.Where(b => b.BookId == bookId).Select(c => c.Category).ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _categoryContext.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
        }

        //Not More Neeeded cAUSE I Changed the name of this method
        public ICollection<Category> GetCategoriesOfABook(int bookId)
        {
            throw new NotImplementedException();
        }
    }
}
