using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Models.DTO;

namespace LibraryManagemetApi.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<int , Review> _reviewRepository ;
        public ReviewService(IRepository<int , Review> reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        /// <summary>
        /// add a review to the book to the databse
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        public async Task<ReturnReviewDTO> AddReview(AddReviewDTO review)
        {
            try
            {

                Review reviewSave = new Review{
                    UserId = review.UserId,
                    BookId = review.BookId,
                    Rating = review.Rating,
                    Comment = review.Comment
                };
                var reviews = await _reviewRepository.Get();
                var userReviews = reviews.Where(r => r.UserId == review.UserId && r.BookId == review.BookId);
                if (userReviews.Count() > 0)
                {
                    throw new ReviewAlreadyExistException();
                }
                
                var res = await _reviewRepository.Insert(reviewSave);

                var resdto = new ReturnReviewDTO
                {
                    ReviewId = res.Id,
                    UserId = res.UserId,
                    BookId = res.BookId,
                    Rating = res.Rating,
                    Comment = res.Comment
                };
                return resdto;
            }
            catch
            {
                throw;
            }
        }
        
        /// <summary>
        /// delete the review from the database
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ReturnReviewDTO> DeleteReview(int reviewId, int userId)
        {
            try
            {
                var review = await _reviewRepository.GetOneById(reviewId);
                if(review.UserId != userId)
                {
                    throw new ForbiddenUserException();
                }
                await _reviewRepository.Delete(review.Id);
                return new ReturnReviewDTO
                {
                    ReviewId = review.Id,
                    UserId = review.UserId,
                    BookId = review.BookId,
                    Rating = review.Rating,
                    Comment = review.Comment
                };
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// get all reviews given by the user from userId
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ReturnReviewDTO>> GetReviewByUserId(int UserId)
        {
            try
            {
                var reviews = await _reviewRepository.Get();
                var userReviews = reviews.Where(r => r.UserId == UserId);
                List<ReturnReviewDTO> returnReviews = new List<ReturnReviewDTO>();
                foreach (var review in userReviews)
                {
                    returnReviews.Add(new ReturnReviewDTO
                    {
                        ReviewId = review.Id,
                        UserId = review.UserId,
                        BookId = review.BookId,
                        Rating = review.Rating,
                        Comment = review.Comment
                    });
                }
                return returnReviews;
            }
            catch
            {
                throw;
            }
        }
        
        /// <summary>
        /// get reviews for the book from bookId
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ReturnReviewDTO>> GetReviwsByBookId(int bookId)
        {
            try
            {
                var reviews = await _reviewRepository.Get();
                var bookReviews = reviews.Where(r => r.BookId == bookId);
                List<ReturnReviewDTO> returnReviews = new List<ReturnReviewDTO>();
                foreach (var review in bookReviews)
                {
                    returnReviews.Add(new ReturnReviewDTO
                    {
                        ReviewId = review.Id,
                        UserId = review.UserId,
                        BookId = review.BookId,
                        Rating = review.Rating,
                        Comment = review.Comment
                    });
                }
                return returnReviews;
            }
            catch
            {
                throw;
            }
        }
    }
}
