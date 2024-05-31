using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models.DTO;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        public async Task<ActionResult<BorrowReturnDTO>> BorrowBook(BorrowDTO borrow)
        {
            var userIdLogged = int.Parse(User.FindFirst("UserId").Value);
            if (borrow.UserId != userIdLogged)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
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
            catch (BookOutOfStockException)
            {
                return StatusCode(StatusCodes.Status410Gone);
            }
            catch (BookAlreadyReservedException)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }
            catch (BookAlreadyBorrowedException)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        [HttpGet]
        [Route("borrowed")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BorrowReturnDTO>>> GetBorrowedBooks(int userId)
        {
            var userIdLogged = int.Parse(User.FindFirst("UserId").Value);
            if (userId != userIdLogged)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

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
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status402PaymentRequired)]
        public async Task<ActionResult<ReturnReturnDTO>> ReturnBook(ReturnDTO returnDTO)
        {
            var userIdLogged = int.Parse(User.FindFirst("UserId").Value);
            if (returnDTO.UserId != userIdLogged)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

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
            catch (BookNotBorrowedException)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }
            catch (BookOverDueException)
            {
                return StatusCode(StatusCodes.Status402PaymentRequired);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("renew")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BorrowReturnDTO>> RenewBook(int userId, int BookId)
        {
            var userIdLogged = int.Parse(User.FindFirst("UserId").Value);
            if (userId != userIdLogged)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            try
            {
                var renewedBook = await _borrowService.renewBook(userId, BookId);
                return Ok(renewedBook);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(userId);
            }
            catch (BookNotBorrowedException)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("due")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BorrowReturnDTO>>> GetDueBooksByUser(int userId)
        {
            var userIdLogged = int.Parse(User.FindFirst("UserId").Value);
            if (userId != userIdLogged)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

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
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<BorrowReturnDTO>> BorrowReservedBook(int userId, int bookId)
        {
            var userIdLogged = int.Parse(User.FindFirst("UserId").Value);
            if (userId != userIdLogged)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            try
            {
                var reservedBook = await _borrowService.BorrowReservedBook(userId, bookId);
                return Ok(reservedBook);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(userId);
            }
            catch (BookNotReservedException)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
