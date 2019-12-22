using dotNetCoreAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetCoreAPI.Services
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);
 
        ICollection<Category> GetAllCategoriesForABook(int bookId);
        ICollection<Book> GetAllBooksForACategory(int categoryId);
        bool CategoryExists(int categoryId);
        bool isDuplicateCategoryName(int categoryId, string categoryName);
    
    }
}
