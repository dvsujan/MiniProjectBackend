using LibraryManagemetApi.Models;
using LibraryManagemetApi.Models.DTO;

namespace LibraryManagemetApi.Interfaces
{
    public interface IBookService
    {
        Task<ReturnBookDTO> AddBook(AddBookDTO dto);
        Task<IEnumerable<ReturnBookDTO>> GetAllBooks(int page , int limit);
        Task<ReturnBookDTO> UpdateBook(UpdateBookDTO id);
        Task<ReturnBookDTO> DeleteBook(int id);
        Task<IEnumerable<ReturnBookDTO>> SearchBookByTitle(string title);
        Task<ReturnBookDTO> GetBook(int id);
        Task<ReturnEditAuthorDTO> EditAuthor(EditAuthorDTO dto); 
        Task<ReturnEditpublicationDTO> EditPublication(EditpublicationDTO dto);
        Task<IEnumerable<Location>> GetAllLocationsInLibrary(); 
    }
}
