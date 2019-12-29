using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNetCoreAPI.Model;

namespace dotNetCoreAPI.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _authorContext;

        public AuthorRepository(AppDbContext authorContext)
        {
            _authorContext = authorContext;
        }

        public bool AuthorExists(int authorId)
        {
            return _authorContext.Authors.Any(c => c.Id == authorId);
        }

        public bool CreateAuthor(Author author)
        {
            _authorContext.Add(author);
            return Save();
        }

        public bool DeleteAuthor(Author author)
        {
            _authorContext.Remove(author);
            return Save();
        }

        public ICollection<Author> GetAllAuthorsOfABook(int bookId)
        {
            return _authorContext.BookAuthors.Where(b => b.BookId == bookId).Select(a => a.Author).ToList();
        }

        public ICollection<Book> GetAllBooksOfAAuthor(int authorId)
        {
            return _authorContext.BookAuthors.Where(a => a.AuthorId== authorId).Select(b => b.Book).ToList();
        }

        public Author GetAuthor(int authorId)
        {
            return _authorContext.Authors.Where(a=>a.Id ==authorId).FirstOrDefault();
        }

        public ICollection<Author> GetAuthors()
        {
            return _authorContext.Authors.OrderBy(a => a.LastName).ToList();
        }

        public bool Save()
        {
            var saved = _authorContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateAuthor(Author author)
        {
            _authorContext.Update(author);
            return Save();
        }
    }
}
