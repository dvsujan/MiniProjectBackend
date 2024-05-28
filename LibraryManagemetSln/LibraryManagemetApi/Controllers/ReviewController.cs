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
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnReviewDTO>>> GetReviewsByBookId(int bookId)
        {
            try
            {
                var reviews = await _reviewService.GetReviwsByBookId(bookId);
                return Ok(reviews);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ReturnReviewDTO>> AddReview(AddReviewDTO review)
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            if(userId != review.UserId)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var addedReview = await _reviewService.AddReview(review);
                return Ok(addedReview);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(review);
            }
            catch (ReviewAlreadyExistException)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<ReturnReviewDTO>> DeleteReview(int reviewId, int userId)
        {
            var userIdLogged = int.Parse(User.FindFirst("UserId").Value);
            if (userId != userIdLogged)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            try
            {
                var deletedReview = await _reviewService.DeleteReview(reviewId, userId);
                return Ok(deletedReview);
            }
            catch (ForbiddenUserException)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(reviewId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet]
        [Route("user")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReturnReviewDTO>>> GetReviewsByUserId(int userId)
        {
            var userIdLogged = int.Parse(User.FindFirst("UserId").Value);
            if (userId != userIdLogged)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            try
            {
                var reviews = await _reviewService.GetReviewByUserId(userId);
                return Ok(reviews);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
