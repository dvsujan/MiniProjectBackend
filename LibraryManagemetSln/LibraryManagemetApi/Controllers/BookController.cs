using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace LibraryManagemetApi.Controllers
{
    [ExcludeFromCodeCoverage]
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
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReturnBookDTO>>> GetAllBooks(int page=1 , int limit = 10)
        {
            try
            {
                var books = await _bookService.GetAllBooks(page , limit);
                return Ok(books);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [Route("add")]
        [Authorize(Roles="2")]
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
            catch (BookAlreadyBorrowedException)
            {
                return StatusCode(StatusCodes.Status409Conflict); 
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut]
        [Route("update")]
        [Authorize(Roles="2")]
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
        [Authorize(Roles="2")]
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
        [HttpPost]
        [Route("editAuthor")]
        [Authorize(Roles="2")]
        public async Task<ActionResult<ReturnEditAuthorDTO>> EditAuthor(EditAuthorDTO editAuthor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var editedBook = await _bookService.EditAuthor(editAuthor);
                return Ok(editedBook);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(editAuthor);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("editPublisher")]
        [Authorize(Roles="2")]
        public async Task<ActionResult<ReturnEditpublicationDTO>> EditPublisher(EditpublicationDTO editPublisher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var editedBook = await _bookService.EditPublication(editPublisher);
                return Ok(editedBook);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(editPublisher);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }   

        [HttpGet]
        [Route("search")]
        [Authorize]
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
        [Authorize]
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
