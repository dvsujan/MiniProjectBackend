using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace LibraryManagemetApi.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        [HttpPost]
        [Route("reserve")]
        [Authorize]
        public async Task<ActionResult<ReservationReturnDTO>> ReserveBook(ReservationDTO reservation)
        {
            var userIdLogged = int.Parse(User.FindFirst("UserId").Value);
            if (reservation.UserId != userIdLogged)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var reservedBook = await _reservationService.ReserveBook(reservation);
                return Ok(reservedBook);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(reservation);
            }
            catch (BookOutOfStockException)
            {
                return StatusCode(StatusCodes.Status410Gone);
            }
            catch(BookAlreadyReservedException)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpDelete]
        [Route("cancel")]
        [Authorize]
        public async Task<ActionResult<ReservationReturnDTO>> CancelReservation(int reservationId, int userId)
        {
            var userIdLogged = int.Parse(User.FindFirst("UserId").Value);
            if (userId != userIdLogged)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            try
            {
                var reservation = await _reservationService.CancelReservation(reservationId, userId);
                return Ok(reservation);
            }
            catch (ForbiddenUserException)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(reservationId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
