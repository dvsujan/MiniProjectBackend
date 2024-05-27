using LibraryManagemetApi.Models.DTO;

namespace LibraryManagemetApi.Interfaces
{
    public interface IborrowService
    {
        Task<BorrowReturnDTO> BorrowBook(BorrowDTO borrow);
        Task<ReturnReturnDTO> ReturnBook(ReturnDTO returnDTO);
        Task<IEnumerable<BorrowReturnDTO>> GetBorrowedBooks(int UserId);
        Task<BorrowReturnDTO> renewBook(int userId, int BorrowId);
        Task<IEnumerable<BorrowReturnDTO>> GetDueBookeByUser(int userId);
        Task<BorrowReturnDTO> BorrowReservedBook(int userId, int bookId);
    }
}
