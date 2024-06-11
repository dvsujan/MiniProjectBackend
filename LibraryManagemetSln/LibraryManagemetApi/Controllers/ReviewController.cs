using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagemetApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly ILogger<ReviewController> _logger;
        public ReviewController(IReviewService reviewService, ILogger<ReviewController> logger)
        {
            _reviewService = reviewService;
            _logger = logger;
        }
        
        /// <summary>
        /// get reviews of the book Id
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReturnReviewDTO>>> GetReviewsByBookId(int bookId)
        {
            try
            {
                var reviews = await _reviewService.GetReviwsByBookId(bookId);
                _logger.LogInformation($"Fetched reviews for book {bookId}");
                return Ok(reviews);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning($"Book not found {bookId}");

                return NotFound(new ErrorDTO{
                    Code = "404",
                    Message = "Book not found"
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        /// <summary>
        /// Add a review for a book by the user
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ReturnReviewDTO>> AddReview(AddReviewDTO review)
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            if(userId != review.UserId)
            {
                _logger.LogError($"Forbidden user {userId}");
                return StatusCode(StatusCodes.Status403Forbidden,new ErrorDTO
                {
                    Code="403",
                    Message="Forbidden User"
                });
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDTO
                {
                    Code="400", 
                    Message="Invalid Data"
                });
            }
            try
            {
                var addedReview = await _reviewService.AddReview(review);
                _logger.LogInformation($"Added review {addedReview.ReviewId} for book {addedReview.BookId}");
                return Ok(addedReview);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning($"Book not found {review.BookId}");
                return NotFound(new ErrorDTO
                {
                    Code="404",
                    Message="Book not found"
                });
            }
            catch (ReviewAlreadyExistException)
            {
                _logger.LogWarning($"Review already exist {review.BookId}");
                return StatusCode(StatusCodes.Status409Conflict, 
                new ErrorDTO
                {
                    Code="409",
                    Message="Review already exist"
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        /// <summary>
        /// Delete a review by the UserId 
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status410Gone)]
        public async Task<ActionResult<ReturnReviewDTO>> DeleteReview(int reviewId, int userId)
        {
            var userIdLogged = int.Parse(User.FindFirst("UserId").Value);
            if (userId != userIdLogged)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorDTO
                {
                    Code="403",
                    Message="Forbidden User"
                });
            }
            try
            {
                var deletedReview = await _reviewService.DeleteReview(reviewId, userId);
                _logger.LogInformation($"Deleted review {deletedReview.ReviewId} for user {userId}");
                return Ok(deletedReview);
            }
            catch (ForbiddenUserException)
            {
                _logger.LogWarning($"Forbidden user {userId}");
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorDTO
                {
                    Code="403",
                    Message="Forbidden User"
                });
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning($"Review not found {reviewId}");
                return NotFound(new ErrorDTO
                {
                    Code = "404",
                    Message = "Review not found"
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        /// <summary>
        ///  Gets the all the reviews by the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("user")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<ReturnReviewDTO>>> GetReviewsByUserId(int userId)
        {
            var userIdLogged = int.Parse(User.FindFirst("UserId").Value);
            if (userId != userIdLogged)
            {
                _logger.LogWarning($"Forbidden user {userIdLogged}");
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorDTO
                {
                    Code ="403",
                    Message="Forbidden User"
                });
            }
            try
            {
                var reviews = await _reviewService.GetReviewByUserId(userId);
                _logger.LogInformation($"Fetched reviews for user {userId}");
                return Ok(reviews);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning($"User not found {userId}");
                return NotFound(new ErrorDTO
                {
                    Code="404",
                    Message="User not found"
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
