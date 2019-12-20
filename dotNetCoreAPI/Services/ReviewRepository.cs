using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNetCoreAPI.Model;

namespace dotNetCoreAPI.Services
{
    public class ReviewRepository : IReviewRepository
    {
        private  AppDbContext _reviewContext;

        public ReviewRepository(AppDbContext reviewContext)
        {
            _reviewContext = reviewContext;
        }

        public ICollection<Review> GetAllReviewsOfABook(int bookId)
        {
            return _reviewContext.Reviews.Where(b => b.Book.Id == bookId).ToList();
        }

        public Book GetBookOfAReview(int reviewId)
        {
            var bookId = _reviewContext.Reviews.Where(r => r.Id == reviewId).Select(b => b.Book.Id).FirstOrDefault();
            return _reviewContext.Books.Where(b => b.Id == bookId).FirstOrDefault();
        }

        public Review GetReview(int reviewId)
        {
            return _reviewContext.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _reviewContext.Reviews.OrderBy(r => r.Rating).ToList();
        }

        public bool ReviewExists(int reviewId)
        {
            return _reviewContext.Reviews.Any(r => r.Id == reviewId);
        }
    }
}
