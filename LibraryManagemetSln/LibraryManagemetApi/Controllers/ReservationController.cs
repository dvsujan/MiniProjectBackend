using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Models.DTO;
using log4net;
using log4net.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagemetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly ILogger<ReservationController> _logger; 
        public ReservationController(IReservationService reservationService, ILogger<ReservationController> logger)
        {
            _reservationService = reservationService;
            _logger = logger;
        }
        /// <summary>
        /// reserve a new book
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("reserve")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status410Gone)]
        public async Task<ActionResult<ReservationReturnDTO>> ReserveBook(ReservationDTO reservation)
        {
            var userIdLogged = int.Parse(User.FindFirst("UserId").Value);
            if (reservation.UserId != userIdLogged)
            {
                _logger.LogWarning($"Forbidden User {userIdLogged}");
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorDTO
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
                var reservedBook = await _reservationService.ReserveBook(reservation);
                _logger.LogInformation($"Reserved book {reservedBook.BookId} for user {reservedBook.UserId}");
                return Ok(reservedBook);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning($"Book not found {reservation.BookId}");
                return NotFound(new ErrorDTO
                {
                    Code="404",
                    Message="Book not found"
                });
            }
            catch (BookOutOfStockException)
            {
                _logger.LogWarning($"Book out of stock {reservation.BookId}");
                return StatusCode(StatusCodes.Status410Gone, new ErrorDTO
                {
                    Code="410",
                    Message="Book out of stock"
                });
            }
            catch(BookAlreadyReservedException)
            {
                _logger.LogWarning($"Book already reserved {reservation.BookId}");
                return StatusCode(StatusCodes.Status409Conflict, new ErrorDTO
                {
                    Code="409",
                    Message="Book already reserved"
                });
            }
            catch (BookAlreadyBorrowedException)
            {
                _logger.LogWarning($"Book Already Borrowed {reservation.BookId}"); 
                return StatusCode(StatusCodes.Status409Conflict, new ErrorDTO
                {
                    Code="409",
                    Message="Book Already Borrowed"
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// CancelReservation of the user
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("cancel")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ReservationReturnDTO>> CancelReservation(int reservationId, int userId)
        {
            var userIdLogged = int.Parse(User.FindFirst("UserId").Value);
            if (userId != userIdLogged)
            {
                _logger.LogWarning($"Forbidden User {userIdLogged}");
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorDTO
                {
                    Code="403",
                    Message="Forbidden User"
                });
            }
            try
            {
                var reservation = await _reservationService.CancelReservation(reservationId, userId);
                _logger.LogInformation($"Cancelled reservation {reservationId} for user {userId}");
                return Ok(reservation);
            }
            catch (ForbiddenUserException)
            {
                _logger.LogWarning($"Forbidden user {userId}");
                return StatusCode(StatusCodes.Status403Forbidden,new ErrorDTO
                {
                    Code="403",
                    Message="Forbidden User"
                });
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning($"Reservation not found {reservationId}");
                return NotFound(new ErrorDTO
                {
                    Code="404",
                    Message="Reservation not found"
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
