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

        public async Task<ReturnReviewDTO> DeleteReview(int reviewId)
        {
            try
            {
                var review = await _reviewRepository.GetOneById(reviewId);
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
