using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Models.DTO;

namespace LibraryManagemetApi.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<int , Review> _reviewRepository ;
        private readonly IRepository<int, User> _userRepository;
        private readonly IRepository<int, Book> _bookRepository; 
        public ReviewService(IRepository<int , Review> reviewRepository, IRepository<int , User> userRepository, IRepository<int , Book> bookRepository)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _bookRepository = bookRepository; 
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
                var user = await _userRepository.GetOneById(res.UserId);

                var resdto = new ReturnReviewDTO
                {
                    ReviewId = res.Id,
                    UserId = res.UserId,
                    BookId = res.BookId,
                    Rating = res.Rating,
                    Comment = res.Comment,
                    UserName = user.Username
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
                var user = await _userRepository.GetOneById(review.UserId);

                return new ReturnReviewDTO
                {
                    ReviewId = review.Id,
                    UserId = review.UserId,
                    BookId = review.BookId,
                    Rating = review.Rating,
                    Comment = review.Comment, 
                    UserName = user.Username,
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
        public async Task<IEnumerable<ReturnUserReviewDTO>> GetReviewByUserId(int UserId)
        {
            try
            {
                var reviews = await _reviewRepository.Get();
                var userReviews = reviews.Where(r => r.UserId == UserId);
                List<ReturnUserReviewDTO> returnReviews = new List<ReturnUserReviewDTO>();
                foreach (var review in userReviews)
                {
                    var user = await _userRepository.GetOneById(review.UserId);
                    var Book = await _bookRepository.GetOneById(review.BookId);
                    returnReviews.Add(new ReturnUserReviewDTO 
                    {
                        ReviewId = review.Id,
                        UserId = review.UserId,
                        BookId = review.BookId,
                        Rating = review.Rating,
                        Comment = review.Comment,
                        BookTitle = Book.Title
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
                    var User = await _userRepository.GetOneById(review.UserId);
                    returnReviews.Add(new ReturnReviewDTO
                    {
                        ReviewId = review.Id,
                        UserId = review.UserId,
                        BookId = review.BookId,
                        Rating = review.Rating,
                        Comment = review.Comment, 
                        UserName = User.Username, 
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
