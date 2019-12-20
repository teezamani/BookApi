using dotNetCoreAPI.Dtos;
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
    public class ReviewersController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private  IReviewRepository _reviewRepository;
        public ReviewersController(IReviewerRepository reviewerRepository,IReviewRepository reviewRepository)
        {
            _reviewerRepository = reviewerRepository;
            _reviewRepository = reviewRepository;
        }

        //api/reviewers
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        public IActionResult GetReviewers()
        {
            var reviewers = _reviewerRepository.GetReviewers();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewersDto = new List<ReviewerDto>();
            foreach (var reviewer in reviewers)
            {
                reviewersDto.Add(new ReviewerDto
                {
                    Id = reviewer.Id,
                    FirstName = reviewer.FirstName,
                    LastName= reviewer.LastName
                });
            }
            return Ok(reviewersDto);
        }


        //api/reviewers/id
        [HttpGet("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ReviewerDto))]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            var reviewer = _reviewerRepository.GetReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerDto = new ReviewerDto()
                {
                    Id = reviewer.Id,
                    FirstName = reviewer.FirstName,
                    LastName = reviewer.LastName
                };
            
            return Ok(reviewerDto);
        }

        // To GetAllReviewsOfAReviewer
        //api/review/reviewerId/reviews
        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        public IActionResult GetAllReviewsOfAReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            var reviews = _reviewerRepository.GetAllReviewsOfAReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewsDto = new List<ReviewDto>();
            foreach (var  review in reviews)
            {
                reviewsDto.Add(new ReviewDto()
                {
                    Id = review.Id,
                    Headline = review.Headline,
                    Rating =review.Rating,
                    ReviewText = review.ReviewText
                });
            }

            return Ok(reviewsDto);
        }
        //But we can only test after we Implement IReviewRepository
        // To Get Reviewer of a review
        //api/reviewers/reviewId/reviewer
        [HttpGet("{reviewId}/reviewer")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ReviewerDto))]
        public IActionResult GetReviewerOfAReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var reviewer = _reviewerRepository.GetReviewer(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerDto = new ReviewerDto()
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                LastName = reviewer.LastName
            };

            return Ok(reviewerDto);
        }
        
    }
}
