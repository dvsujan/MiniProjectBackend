using LibraryManagemetApi.Models.DTO;

namespace LibraryManagemetApi.Interfaces
{
    public interface IBookService
    {
        Task<ReturnBookDTO> AddBook(AddBookDTO dto);
        Task<IEnumerable<ReturnBookDTO>> GetAllBooks();
        Task<ReturnBookDTO> UpdateBook(UpdateBookDTO id);
        Task<ReturnBookDTO> DeleteBook(int id);
        Task<IEnumerable<ReturnBookDTO>> SearchBookByTitle(string title);
        Task<ReturnBookDTO> GetBook(int id);
    }
}
