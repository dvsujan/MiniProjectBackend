using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Models.DTO;
using LibraryManagemetApi.Repositories;

namespace LibraryManagemetApi.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<int, Payment> _paymentRepository;
        private readonly IRepository<int, Card> _cardRepository;
        private readonly IRepository<int, User> _userRepository;
        private readonly IRepository<int, Borrowed> _borrowedRepository;

        public PaymentService(IRepository<int, Payment> paymentRepository,IRepository<int , Card> cardRepository,  IRepository<int, User> userRepository, IRepository<int, Borrowed> borrowedRepository)
        {
            _paymentRepository = paymentRepository;
            _cardRepository = cardRepository; 
            _userRepository = userRepository;
            _borrowedRepository = borrowedRepository;
        }

        public async Task<ResponseCardDTO> AddCard(AddCardDTO card)
        {
            try
            {
                Card cardsave = new Card
                {
                    UserId = card.UserId,
                    CardNumber = card.CardNumber,
                    CVV = card.CVV,
                    ExpDate = card.ExpiryDate
                };
                await _cardRepository.Insert(cardsave);
                return new ResponseCardDTO
                {
                    CardId = cardsave.Id,
                    cardNumber = cardsave.CardNumber,
                    cvv = cardsave.CVV,
                    expiryDate = cardsave.ExpDate
                };
            }
            catch 
            {
                throw;
            }
        }
        public async Task<ResponseCardDTO> DeleteCard(int cardId)
        {
            try
            {
                var card = await _cardRepository.GetOneById(cardId);
                await _cardRepository.Delete(card.Id);
                return new ResponseCardDTO
                {
                    CardId = card.Id,
                    cardNumber = card.CardNumber,
                    cvv = card.CVV,
                    expiryDate = card.ExpDate
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<ResponseCardDTO>> GetAllUserCards(int userId)
        {
            try
            {
                var cards = await _cardRepository.Get();
                List<ResponseCardDTO> responseCardDTOs = new List<ResponseCardDTO>();
                foreach (var card in cards)
                {
                    if (card.UserId == userId)
                    {
                        responseCardDTOs.Add(new ResponseCardDTO
                        {
                            CardId = card.Id,
                            cardNumber = card.CardNumber,
                            cvv = card.CVV,
                            expiryDate = card.ExpDate
                        });
                    }
                }
                return responseCardDTOs;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<PaymentReturnDTO>> GetUserPaymentHistory(int userId)
        {
            try
            {
                var payments = await _paymentRepository.Get();
                List<PaymentReturnDTO> paymentReturnDTOs = new List<PaymentReturnDTO>();
                foreach (var payment in payments)
                {
                    if (payment.UserId == userId)
                    {
                        paymentReturnDTOs.Add(new PaymentReturnDTO
                        {
                            PaymentId = payment.Id,
                            Amount = payment.Amount,
                            PaymentDate = payment.PaymentDate
                        });
                    }
                }
                return paymentReturnDTOs;
            }
            catch
            {
                throw;
            }
        }

        public async Task<PaymentReturnDTO> PayFine(PaymentDTO payment)
        {
            try
            {
                var user = await _userRepository.GetOneById(payment.UserId);
                var card = await _cardRepository.GetOneById(payment.CardId);
                var borrowed = await _borrowedRepository.GetOneById(payment.BorrowId);
                if (borrowed.ReturnDate < DateTime.Now)
                {
                    var days = (DateTime.Now - borrowed.ReturnDate).Value.Days;
                    decimal fine = days * 5;
                    Payment paymentSave = new Payment
                    {
                        UserId = payment.UserId,
                        Amount = fine,
                        PaymentDate = DateTime.Now
                    };
                    await _paymentRepository.Insert(paymentSave);
                    return new PaymentReturnDTO
                    {
                        PaymentId = paymentSave.Id,
                        Amount = paymentSave.Amount,
                        PaymentDate = paymentSave.PaymentDate
                    };
                }
                else
                {
                    return new PaymentReturnDTO
                    {
                        PaymentId = 0,
                        Amount = 0,
                        PaymentDate = DateTime.Now
                    };
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
