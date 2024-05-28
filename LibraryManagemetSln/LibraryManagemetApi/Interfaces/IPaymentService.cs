using LibraryManagemetApi.Models.DTO;

namespace LibraryManagemetApi.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentReturnDTO> PayFine(PaymentDTO payment);
        Task<IEnumerable<ResponseCardDTO>> GetAllUserCards(int userId);
        Task<ResponseCardDTO> AddCard(AddCardDTO card);
        Task<ResponseCardDTO> DeleteCard(int cardId, int userId);
        Task<IEnumerable<PaymentReturnDTO>> GetUserPaymentHistory(int userId);
    }
}
