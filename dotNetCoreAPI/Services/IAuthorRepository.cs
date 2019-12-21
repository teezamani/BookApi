using dotNetCoreAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetCoreAPI.Services
{
   public interface IAuthorRepository
    {
        ICollection<Author> GetAuthors();
        Author GetAuthor(int authorId);
        ICollection<Author> GetAllAuthorsOfABook(int bookId);
        ICollection<Book> GetAllBookOfAAuthor(int authorId);
        bool AuthorExists(int authorId);
    }
}
