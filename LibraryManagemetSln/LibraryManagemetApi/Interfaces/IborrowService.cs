using LibraryManagemetApi.Models.DTO;

namespace LibraryManagemetApi.Interfaces
{
    public interface IborrowService
    {
        Task<BorrowReturnDTO> BorrowBook(BorrowDTO borrow);
        Task<ReturnReturnDTO> ReturnBook(ReturnDTO returnDTO);
        Task<GetBorrowedBooksDTO> GetBorrowedBooks(string email);
        Task<BorrowReturnDTO> renewBook(int userId, int BorrowId);
        Task<IEnumerable<BorrowReturnDTO>> GetDueBookeByUser(int userId);
    }
}
