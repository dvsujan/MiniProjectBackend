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
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly ILogger _logger; 
        public ReservationController(IReservationService reservationService, ILogger logger)
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        public async Task<ActionResult<ReservationReturnDTO>> ReserveBook(ReservationDTO reservation)
        {
            var userIdLogged = int.Parse(User.FindFirst("UserId").Value);
            if (reservation.UserId != userIdLogged)
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
                var reservedBook = await _reservationService.ReserveBook(reservation);
                _logger.LogInformation($"Reserved book {reservedBook.BookId} for user {reservedBook.UserId}");
                return Ok(reservedBook);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning($"Book not found {reservation.BookId}");
                return NotFound(reservation);
            }
            catch (BookOutOfStockException)
            {
                _logger.LogWarning($"Book out of stock {reservation.BookId}");
                return StatusCode(StatusCodes.Status410Gone);
            }
            catch(BookAlreadyReservedException)
            {
                _logger.LogWarning($"Book already reserved {reservation.BookId}");
                return StatusCode(StatusCodes.Status409Conflict);
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ReservationReturnDTO>> CancelReservation(int reservationId, int userId)
        {
            var userIdLogged = int.Parse(User.FindFirst("UserId").Value);
            if (userId != userIdLogged)
            {
                _logger.LogWarning($"Forbidden User {userIdLogged}");
                return StatusCode(StatusCodes.Status403Forbidden);
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
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning($"Reservation not found {reservationId}");
                return NotFound(reservationId);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
