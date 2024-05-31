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
        private readonly ILogger<BorrowController> _logger;
        public BorrowController(IborrowService borrowService, ILogger<BorrowController> logger)
        {
            _borrowService = borrowService;
            _logger = logger;
        }

        /// <summary>
        /// Borrow a book based on the userId and bookId
        /// </summary>
        /// <param name="borrow"></param>
        /// <returns></returns>
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
                _logger.LogWarning($"Forbidden User {userIdLogged}");

                return StatusCode(StatusCodes.Status403Forbidden);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var borrowedBook = await _borrowService.BorrowBook(borrow);
                _logger.LogInformation($"Borrowed Book {borrowedBook.BookId} by User {borrowedBook.UserId}");
                return Ok(borrowedBook);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning($"Entity Not Found {borrow.UserId} or {borrow.BookId}");
                return NotFound(borrow);
            }
            catch (BookOutOfStockException)
            {
                _logger.LogWarning($"Book Out Of Stock {borrow.BookId}"); 
                return StatusCode(StatusCodes.Status410Gone);
            }
            catch (BookAlreadyReservedException)
            {
                _logger.LogWarning($"Book Already Reserved {borrow.BookId}");
                return StatusCode(StatusCodes.Status409Conflict);
            }
            catch (BookAlreadyBorrowedException)
            {
                _logger.LogWarning($"Book Already Borrowed {borrow.BookId}");
                return StatusCode(StatusCodes.Status409Conflict);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }
        /// <summary>
        /// Gets the borrowed books by the userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
                _logger.LogWarning($"Forbidden User {userIdLogged}");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            try
            {
                var borrowedBooks = await _borrowService.GetBorrowedBooks(userId);
                _logger.LogInformation($"Fetched Borrowed Books by User {userId}");
                return Ok(borrowedBooks);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning($"Entity Not Found {userId}");
                return NotFound(userId);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// return the borrowed based on the userId and bookId
        /// </summary>
        /// <param name="returnDTO"></param>
        /// <returns></returns>
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
                _logger.LogWarning($"Forbidden User {userIdLogged}");
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var returnedBook = await _borrowService.ReturnBook(returnDTO);
                _logger.LogInformation($"Returned Book {returnDTO.BookId} by User {returnDTO.UserId}");
                return Ok(returnedBook);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning($"Entity Not Found {returnDTO.UserId} or {returnDTO.BookId}");
                return NotFound(returnDTO);
            }
            catch (BookNotBorrowedException)
            {
                _logger.LogWarning($"Book Not Borrowed {returnDTO.BookId}");
                return StatusCode(StatusCodes.Status409Conflict);
            }
            catch (BookOverDueException)
            {
                _logger.LogWarning($"Book Over Due {returnDTO.BookId}");
                return StatusCode(StatusCodes.Status402PaymentRequired);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        

        /// <summary>
        /// renew a borrowed Book
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="BookId">BookId</param>
        /// <returns></returns>
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
                _logger.LogWarning($"Forbidden User {userIdLogged}");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            try
            {
                var renewedBook = await _borrowService.renewBook(userId, BookId);
                _logger.LogInformation($"Renewed Book {BookId} by User {userId}");
                return Ok(renewedBook);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning($"Entity Not Found {userId} or {BookId}");
                return NotFound(userId);
            }
            catch (BookNotBorrowedException)
            {
                _logger.LogWarning($"Book Not Borrowed {BookId}");
                return StatusCode(StatusCodes.Status409Conflict);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        /// <summary>
        /// Get Due Books By the userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
                _logger.LogWarning($"Forbidden User {userIdLogged}");
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            try
            {
                var dueBooks = await _borrowService.GetDueBookeByUser(userId);
                return Ok(dueBooks);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning($"Entity Not Found {userId}");
                return NotFound(userId);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Borrow reserved Book By the userId and BookId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
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
                _logger.LogWarning($"Forbidden User {userIdLogged}");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            try
            {
                var reservedBook = await _borrowService.BorrowReservedBook(userId, bookId);
                _logger.LogInformation($"Borrowed Reserved Book {bookId} by User {userId}");
                return Ok(reservedBook);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning($"Entity Not Found {userId} or {bookId}");
                return NotFound(userId);
            }
            catch (BookNotReservedException)
            {
                _logger.LogWarning($"Book Not Reserved {bookId}");
                return StatusCode(StatusCodes.Status409Conflict);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
