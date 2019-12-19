using dotNetCoreAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetCoreAPI.Services
{
   public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetAllReviewsOfABook(int bookId);
        Review GetBookOfAReview(int reviewId);
        bool ReviewExists(int reviewId);
    }
}