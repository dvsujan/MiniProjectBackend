using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Models.DTO;
using LibraryManagemetApi.Repositories;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace LibraryManagemetApi.Services
{
    public class BookService : IBookService
    {
        IRepository<int, Book> _bookRepository;
        IRepository<int , Author> _authorRepository;
        IRepository<int, Publisher> _publisherRepository;
        IRepository<int, Category> _categoryRepository;
        IRepository<int, Location> _locationRepository; 
        IRepository<int , Stock> _stockRepository;

        public BookService(IRepository<int, Book> bookRepository, IRepository<int, Author> authorRepository, IRepository<int, Publisher> publisherRepository, IRepository<int, Category> categoryRepository, IRepository<int, Location> locationRepository, IRepository<int, Stock> stockRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _publisherRepository = publisherRepository;
            _categoryRepository = categoryRepository;
            _locationRepository = locationRepository;
            _stockRepository = stockRepository;
        }
        private async Task<int> CreateAuthorfNotExists(string authorName)
        {
            try
            {
                var author = await ((AuthorRepository)_authorRepository).GetAuthorByName(authorName);
                return author.Id;
            }
            catch (EntityNotFoundException)
            {
                Author author = new Author
                {
                    Name = authorName
                };
                await _authorRepository.Insert(author);
                return author.Id;
            }

        }
        private async Task<int> CreateCategoryIfNotExists(string categoryName)
        {
            try
            {
                var category = await ((CategoryRepository)_categoryRepository).GetCategoryByName(categoryName);
                return category.Id;
            }
            catch (EntityNotFoundException)
            {
                Category category = new Category
                {
                    Name = categoryName
                };
                await _categoryRepository.Insert(category);
                return category.Id;
            }
        }
        private async Task<int> createPublisherIfNotExists(string publisherName)
        {
            try
            {
                var publisher = await ((PublisherRepository)_publisherRepository).GetPublisherByName(publisherName);
                return publisher.Id;
            }
            catch (EntityNotFoundException)
            {
                Publisher publisher = new Publisher
                {
                    Name = publisherName
                };
                await _publisherRepository.Insert(publisher);
                return publisher.Id;
            }
        }
        public async Task<ReturnBookDTO> AddBook(AddBookDTO dto)
        {
            Book book; 
            try
            {
                int authorId = await CreateAuthorfNotExists(dto.AuthorName);
                int categoryId = await CreateCategoryIfNotExists(dto.CategoryName);
                int publisherId = await createPublisherIfNotExists(dto.publisherName);
                
                book = new Book
                {
                    Title = dto.Title,
                    AuthorId = authorId,
                    CategoryId = categoryId,
                    PublisherId = publisherId,
                    PublishedDate = dto.PublishedDate,
                    ISBN = dto.ISBN,
                    LocationId = dto.LocationId
                };
                await _bookRepository.Insert(book);
                Stock stock = new Stock
                {
                    BookId = book.Id,
                    Quantity = dto.Stock
                };
                await _stockRepository.Insert(stock);
                return new ReturnBookDTO
                {
                    BookId = book.Id,
                    Title = book.Title,
                    AuthorName = dto.AuthorName,
                    publishedDate = book.PublishedDate,
                    Category = dto.CategoryName,
                    Quantity = stock.Quantity
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ReturnBookDTO> DeleteBook(int id)
        {
            try
            {
                var book = await _bookRepository.GetOneById(id);
                var stock = await ((StockRepository)_stockRepository).GetStockByBookId(id);
                await _stockRepository.Delete(stock.Id);
                await _bookRepository.Delete(id);
                return new ReturnBookDTO
                {
                    BookId = book.Id,
                    Title = book.Title,
                    AuthorName = book.Author.Name,
                    publishedDate = book.PublishedDate,
                    Category = book.Category.Name,
                    Quantity = stock.Quantity
                };
            }
            catch (EntityNotFoundException)
            {
                throw new EntityNotFoundException();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task<IEnumerable<ReturnBookDTO>> GetAllBooks()
        {
            try
            {
                var books = await _bookRepository.Get();
                List<ReturnBookDTO> returnBooks = new List<ReturnBookDTO>();
                foreach (var book in books)
                {
                    var stock = await ((StockRepository)_stockRepository).GetStockByBookId(book.Id);
                    var category = await _categoryRepository.GetOneById(book.CategoryId);
                    returnBooks.Add(new ReturnBookDTO
                    {
                        BookId = book.Id,
                        Title = book.Title,
                        Category = category.Name,
                        Quantity = stock.Quantity
                    });
                }
                return returnBooks;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ReturnBookDTO> GetBook(int id)
        {
            try
            {
                var book = await _bookRepository.GetOneById(id);
                var stock = await ((StockRepository)_stockRepository).GetStockByBookId(id);
                var category = await _categoryRepository.GetOneById(book.CategoryId);
                return new ReturnBookDTO
                {
                    BookId = book.Id,
                    Title = book.Title,
                    Category = category.Name,
                    Quantity = stock.Quantity
                };
            }
            catch (EntityNotFoundException)
            {
                throw new EntityNotFoundException();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<ReturnBookDTO>> SearchBookByTitle(string title)
        {
            try
            {
                var books = await ((BookRepository)_bookRepository).GetBookByTitle(title);
                List<ReturnBookDTO> returnBooks = new List<ReturnBookDTO>();
                foreach (Book book in books)
                {
                    var stock = await ((StockRepository)_stockRepository).GetStockByBookId(book.Id);
                    var category = await _categoryRepository.GetOneById(book.CategoryId);
                    returnBooks.Add(new ReturnBookDTO
                    {
                        BookId = book.Id,
                        Title = book.Title,
                        Category = category.Name,
                        Quantity = stock.Quantity
                    });
                }
                return returnBooks;
            }
            catch (EntityNotFoundException)
            {
                throw new EntityNotFoundException();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ReturnBookDTO> UpdateBook(UpdateBookDTO dto)
        {
            try
            {
                var book = await _bookRepository.GetOneById(dto.BookId);
                var stock = await ((StockRepository)_stockRepository).GetStockByBookId(dto.BookId);
                book.Title = dto.Title;
                book.PublishedDate = dto.PublishedDate;
                book.ISBN = dto.ISBN;
                book.LocationId = dto.LocationId;
                await _bookRepository.Update(book);
                stock.Quantity = dto.Stock;
                await _stockRepository.Update(stock);
                return new ReturnBookDTO
                {
                    BookId = book.Id,
                    Title = book.Title,
                    AuthorName = book.Author.Name,
                    publishedDate = book.PublishedDate,
                    Category = book.Category.Name,
                    Quantity = stock.Quantity
                };
            }
            catch (EntityNotFoundException)
            {
                throw new EntityNotFoundException();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
    }
}
