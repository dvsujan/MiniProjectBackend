using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagemetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowController : ControllerBase
    {
        private readonly IborrowService _borrowService;
        public BorrowController(IborrowService borrowService)
        {
            _borrowService = borrowService;
        }

        [HttpPost]
        [Route("borrow")]
        public async Task<ActionResult<BorrowReturnDTO>> BorrowBook(BorrowDTO borrow)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var borrowedBook = await _borrowService.BorrowBook(borrow);
                return Ok(borrowedBook);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(borrow);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("borrowed")]
        public async Task<ActionResult<IEnumerable<BorrowReturnDTO>>> GetBorrowedBooks(int userId)
        {
            try
            {
                var borrowedBooks = await _borrowService.GetBorrowedBooks(userId);
                return Ok(borrowedBooks);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(userId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("return")]
        public async Task<ActionResult<ReturnReturnDTO>> ReturnBook(ReturnDTO returnDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var returnedBook = await _borrowService.ReturnBook(returnDTO);
                return Ok(returnedBook);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(returnDTO);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("renew")]
        public async Task<ActionResult<BorrowReturnDTO>> RenewBook(int userId, int BookId)
        {
            try
            {
                var renewedBook = await _borrowService.renewBook(userId, BookId);
                return Ok(renewedBook);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(userId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [Route("due")]
        public async Task<ActionResult<IEnumerable<BorrowReturnDTO>>> GetDueBooksByUser(int userId)
        {
            try
            {
                var dueBooks = await _borrowService.GetDueBookeByUser(userId);
                return Ok(dueBooks);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(userId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [Route("reserved")]
        public async Task<ActionResult<BorrowReturnDTO>> BorrowReservedBook(int userId, int bookId)
        {
            try
            {
                var reservedBook = await _borrowService.BorrowReservedBook(userId, bookId);
                return Ok(reservedBook);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(userId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
