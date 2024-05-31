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
    public class PaymentContorller : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentContorller(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        
        [HttpPost]
        [Route("addcard")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseCardDTO>> AddCard(AddCardDTO card)
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            if(userId != card.UserId)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var addedCard = await _paymentService.AddCard(card);
                return Ok(addedCard);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpDelete]
        [Route("deletecard")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseCardDTO>> DeleteCard(int cardId, int userId)
        {
            try
            {
                var deletedCard = await _paymentService.DeleteCard(cardId, userId);
                return Ok(deletedCard);
            }
            catch (ForbiddenUserException)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("cards")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ResponseCardDTO>>> GetCards(int userId)
        {
            var userIdLogged = int.Parse(User.FindFirst("UserId").Value);
            if (userId != userIdLogged)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            try
            {

                var cards = await _paymentService.GetAllUserCards(userId);
                return Ok(cards);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("payFine")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentReturnDTO>> Pay(PaymentDTO payment)
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            if (userId != payment.UserId)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var paymentResponse = await _paymentService.PayFine(payment);
                return Ok(paymentResponse);
            }
            catch(ForbiddenBorrowException)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            catch (ForbiddenCardException)
            {
                return StatusCode(StatusCodes.Status403Forbidden); 
            }
            catch (EntityNotFoundException)
            {
                return NotFound(payment);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
