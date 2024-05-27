using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagemetApi.Controllers
{
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
        public async Task<ActionResult<ReservationReturnDTO>> ReserveBook(ReservationDTO reservation)
        {
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
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpDelete]
        [Route("cancel")]
        public async Task<ActionResult<ReservationReturnDTO>> CancelReservation(int reservationId)
        {
            try
            {
                var reservation = await _reservationService.CancelReservation(reservationId);
                return Ok(reservation);
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
