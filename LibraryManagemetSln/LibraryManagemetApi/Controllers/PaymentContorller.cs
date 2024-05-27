using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models.DTO;
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
        public async Task<ActionResult<ResponseCardDTO>> AddCard(AddCardDTO card)
        {
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
        public async Task<ActionResult<ResponseCardDTO>> DeleteCard(int cardId)
        {
            try
            {
                var deletedCard = await _paymentService.DeleteCard(cardId);
                return Ok(deletedCard);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("cards")]
        public async Task<ActionResult<IEnumerable<ResponseCardDTO>>> GetCards(int userId)
        {
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
        [Route("pay")]
        public async Task<ActionResult<PaymentReturnDTO>> Pay(PaymentDTO payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var paymentResponse = await _paymentService.PayFine(payment);
                return Ok(paymentResponse);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
