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
        IRepository<int, Review> _reviewRepository; 
        
        public BookService(IRepository<int, Book> bookRepository, IRepository<int, Author> authorRepository, IRepository<int, Publisher> publisherRepository, IRepository<int, Category> categoryRepository, IRepository<int, Location> locationRepository, IRepository<int, Stock> stockRepository, IRepository<int , Review> reviewRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _publisherRepository = publisherRepository;
            _categoryRepository = categoryRepository;
            _locationRepository = locationRepository;
            _stockRepository = stockRepository;
            _reviewRepository = reviewRepository;
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
                var books = await _bookRepository.Get(); 
                var bookExists = books.Where(b =>  b.ISBN == dto.ISBN);
                if (bookExists.Count() > 0)
                {
                    throw new BookAlreadyExistsException();
                }
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

        public async Task<IEnumerable<ReturnBookDTO>> GetAllBooks(int page , int limit)
        {
            try
            {
                var books = await ((BookRepository)_bookRepository).GetPaginated(page , limit);
                List<ReturnBookDTO> returnBooks = new List<ReturnBookDTO>();
                foreach (var book in books)
                {
                    var stock = await ((StockRepository)_stockRepository).GetStockByBookId(book.Id);
                    var avgRating = await ((ReviewRepository)_reviewRepository).GetAvgRatingOfBookId(book.Id);
                    var noOfReviews = await ((ReviewRepository)_reviewRepository).GetNoOfRatingsOfBookId(book.Id);
                    if (stock.Quantity > 0)
                    {
                        returnBooks.Add(new ReturnBookDTO
                        {
                            BookId = book.Id,
                            Title = book.Title,
                            Category = book.Category.Name,
                            Quantity = stock.Quantity,
                            publishedDate = book.PublishedDate,
                            rating = avgRating,
                            noOfRatings = noOfReviews,
                            floorNo = book.Location.Floor,
                            shelfNo = book.Location.Shelf
                        });
                    }
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
                var avgRating = await ((ReviewRepository)_reviewRepository).GetAvgRatingOfBookId(id);
                var noOfReviews = await ((ReviewRepository)_reviewRepository).GetNoOfRatingsOfBookId(id);
                return new ReturnBookDTO
                {
                    BookId = book.Id,
                    Title = book.Title,
                    Category = book.Category.Name,
                    Quantity = stock.Quantity,
                    publishedDate = book.PublishedDate,
                    rating = avgRating,
                    noOfRatings = noOfReviews,
                    floorNo = book.Location.Floor,
                    shelfNo = book.Location.Shelf
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
                    var avgRating = await ((ReviewRepository)_reviewRepository).GetAvgRatingOfBookId(book.Id);
                    var noOfReviews = await ((ReviewRepository)_reviewRepository).GetNoOfRatingsOfBookId(book.Id);
                    returnBooks.Add(new ReturnBookDTO
                    {
                        BookId = book.Id,
                        Title = book.Title,
                        publishedDate = book.PublishedDate,
                        Category = book.Category.Name,
                        Quantity = stock.Quantity,
                        rating = avgRating,
                        noOfRatings = noOfReviews,
                        floorNo = book.Location.Floor,
                        shelfNo = book.Location.Shelf
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

        public async Task<ReturnEditAuthorDTO> EditAuthor(EditAuthorDTO dto)
        {
            try
            {
                var author = await ((AuthorRepository)_authorRepository).GetAuthorByName(dto.Name);
                author.Name = dto.Name;
                author.Language = dto.AuthorLanguage;
                await _authorRepository.Update(author);
                return new ReturnEditAuthorDTO 
                {
                    AuthorId = author.Id,
                    AuthorName = author.Name,
                    AuthorLanguage = author.Language
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

        public async Task<ReturnEditpublicationDTO> EditPublication(EditpublicationDTO dto)
        {
            try
            {
                var publisher = await ((PublisherRepository)_publisherRepository).GetPublisherByName(dto.Name);
                publisher.Name = dto.Name;
                publisher.Address = dto.Address;
                var updatedPublisher = await _publisherRepository.Update(publisher);
                return new ReturnEditpublicationDTO
                {
                    Id = updatedPublisher.Id,
                    Name = updatedPublisher.Name,
                    Address = updatedPublisher.Address
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
