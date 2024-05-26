using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagemetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<ReturnBookDTO>>> GetAllBooks()
        {
            try
            {
                var books = await _bookService.GetAllBooks();
                return Ok(books);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<ReturnBookDTO>> AddBook(AddBookDTO book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var addedBook = await _bookService.AddBook(book);
                return Ok(addedBook);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(book);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<ReturnBookDTO>> UpdateBook(UpdateBookDTO book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var updatedBook = await _bookService.UpdateBook(book);
                return Ok(updatedBook);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(book);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpDelete]
        [Route("delete")]
        public async Task<ActionResult<ReturnBookDTO>> DeleteBook(int id)
        {
            try
            {
                var deletedBook = await _bookService.DeleteBook(id);
                return Ok(deletedBook);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(id);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<ReturnBookDTO>>> SearchBookByTitle(string title)
        {
            try
            {
                var books = await _bookService.SearchBookByTitle(title);
                return Ok(books);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(title);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<ReturnBookDTO>> GetBook(int id)
        {
            try
            {
                var book = await _bookService.GetBook(id);
                return Ok(book);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(id);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
