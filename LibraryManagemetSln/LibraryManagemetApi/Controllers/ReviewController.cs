using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models.DTO;
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
        [HttpPost]
        public async Task<ActionResult<ReturnReviewDTO>> AddReview(AddReviewDTO review)
        {
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
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpDelete]
        public async Task<ActionResult<ReturnReviewDTO>> DeleteReview(int reviewId)
        {
            try
            {
                var deletedReview = await _reviewService.DeleteReview(reviewId);
                return Ok(deletedReview);
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
        public async Task<ActionResult<IEnumerable<ReturnReviewDTO>>> GetReviewsByUserId(int userId)
        {
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
