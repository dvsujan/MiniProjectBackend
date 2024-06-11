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
        private readonly ILogger<PaymentContorller> _logger;
        public PaymentContorller(IPaymentService paymentService, ILogger<PaymentContorller> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }
        
        /// <summary>
        /// add new card for the user
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
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
                _logger.LogWarning($"Forbidden user {userId}");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var addedCard = await _paymentService.AddCard(card);
                _logger.LogInformation($"Added new card {addedCard.CardId}{card.UserId}");
                return Ok(addedCard);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        ///  Delete the card for the user 
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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
                _logger.LogInformation($"Deleted card {deletedCard.CardId} for user {userId}");
                return Ok(deletedCard);
            }
            catch (ForbiddenUserException)
            {
                _logger.LogWarning($"Forbidden user {userId}");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("cards")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ResponseCardDTO>>> GetCards(int userId)
        {
            var userIdLogged = int.Parse(User.FindFirst("UserId").Value);
            if (userId != userIdLogged)
            {
                _logger.LogInformation($"Forbidden user {userId}");
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorDTO
                {
                    Code = "403",
                    Message = "Forbidden User"
                });
            }
            try
            {

                var cards = await _paymentService.GetAllUserCards(userId);
                _logger.LogInformation($"Fetched all cards for user {userId}");
                return Ok(cards);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning($"User not found {userId}");
                return NotFound(new ErrorDTO
                {
                    Code = "404",
                    Message = "User not found"
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("payFine")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentReturnDTO>> Pay(PaymentDTO payment)
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            if (userId != payment.UserId)
            {
                _logger.LogWarning($"Forbidden user {userId}");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDTO
                {
                    Code = "400",
                    Message = "Invalid Data"
                });
            }
            try
            {
                var paymentResponse = await _paymentService.PayFine(payment);
                _logger.LogInformation($"Paid fine {paymentResponse.PaymentId} for user {userId}");
                return Ok(paymentResponse);
            }
            catch(ForbiddenBorrowException)
            {
                _logger.LogWarning($"Forbidden borrow {payment.BorrowId}");
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorDTO
                {
                    Code = "403",
                    Message = "Forbidden borrow"
                });
            }
            catch (PaymentAlreadyDoneException)
            {
                _logger.LogWarning($"Payment already done {payment.BorrowId}");
                return StatusCode(StatusCodes.Status409Conflict, new ErrorDTO
                {
                    Code = "409",
                    Message = "Payment already done"
                });
            }
            catch (ForbiddenCardException)
            {
                _logger.LogWarning($"Forbidden card {payment.CardId}");
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorDTO
                {
                    Code = "403",
                    Message = "Forbidden card"
                }); 
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning($"Not found borrow {payment.BorrowId}");
                return NotFound(new ErrorDTO
                {
                    Code = "404",
                    Message = "Not found borrow"
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
