using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Models.DTO;
using log4net.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagemetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BookController> _logger;
        public BookController(IBookService bookService, ILogger<BookController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        /// <summary>
        ///   Returns all the books in the database in ReturnDtoformat
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReturnBookDTO>>> GetAllBooks(int page=1 , int limit = 10)
        {
            try
            {
                var books = await _bookService.GetAllBooks(page , limit);
                _logger.LogInformation("Fetched All Books");
                return Ok(books);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Adds new book and checks the conditions before ading    
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        [Authorize(Roles="2")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

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
                _logger.LogWarning("Book Not Found");
                return NotFound(book);
            }
            catch (BookAlreadyBorrowedException)
            {
                _logger.LogWarning("book already Borrowed");
                return StatusCode(StatusCodes.Status409Conflict); 
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// updates the book 
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [Authorize(Roles="2")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReturnBookDTO>> UpdateBook(UpdateBookDTO book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var updatedBook = await _bookService.UpdateBook(book);
                _logger.LogInformation($"{book.BookId} Updated");

                return Ok(updatedBook);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning("Book Not Found While updating Book"); 
                return NotFound(book);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message); 
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// delete the book 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [Authorize(Roles="2")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReturnBookDTO>> DeleteBook(int id)
        {
            try
            {
                var deletedBook = await _bookService.DeleteBook(id);
                _logger.LogInformation($"BookId: {id} Deleated"); 

                return Ok(deletedBook);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning("Entity not found while deleting book");
                return NotFound(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// edit the author of the book based on the author name 
        /// </summary>
        /// <param name="editAuthor"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("editAuthor")]
        [Authorize(Roles="2")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReturnEditAuthorDTO>> EditAuthor(EditAuthorDTO editAuthor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var editedBook = await _bookService.EditAuthor(editAuthor);
                _logger.LogInformation($"{editAuthor.Name} edited");
                return Ok(editedBook);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning("Entity not found while editing author");
                return NotFound(editAuthor);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        /// <summary>
        /// edit the publisher of the book based on the publisher name
        /// </summary>
        /// <param name="editPublisher"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("editPublisher")]
        [Authorize(Roles="2")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                _logger .LogWarning($"Entity not found while editing publisher ");
                return NotFound(editPublisher);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// search the book based on the book title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("search")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReturnBookDTO>>> SearchBookByTitle(string title)
        {
            try
            {
                var books = await _bookService.SearchBookByTitle(title);
                return Ok(books);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning("Entity not found while searching book by title");
                return NotFound(title);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get the book based on the book id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReturnBookDTO>> GetBook(int id)
        {
            try
            {
                var book = await _bookService.GetBook(id);
                return Ok(book);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning("Entity not found while getting book");
                return NotFound(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message); 
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
