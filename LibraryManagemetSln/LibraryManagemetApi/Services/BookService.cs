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

        /// <summary>
        /// check if the author exists in the database if not create a new author
        /// </summary>
        /// <param name="authorName"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// checks if the category exists in the database if not create a new category
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
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

        /// <summary>
        ///  check if the publisher exist in database if not create a new publisher object
        /// </summary>
        /// <param name="publisherName"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Adds new book to the database
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="BookAlreadyExistsException">if book already exists then throws exception</exception>
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

        /// <summary>
        /// delete the book from the database 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException">throws if book not found in the databse</exception>
        /// <exception cref="Exception">general exception</exception>
                
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

        /// <summary>
        /// returns all the books in the database paginated based on the page and limit given 
        /// </summary>
        /// <param name="page">pageno</param>
        /// <param name="limit">noofitems per page</param>
        /// <returns></returns>
        /// <exception cref="Exception">general exception</exception>

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
                            ISBN = book.ISBN,
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

        /// <summary>
        /// Gets the book by the bookId from the databse 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException">throws if book id does not exist in databse</exception>
        /// <exception cref="Exception">general exception</exception>
        
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
                    ISBN = book.ISBN,
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
        
        /// <summary>
        /// Seacches the Book By Tite 
        /// </summary>
        /// <param name="title">string title</param>
        /// <returns>list of booktitles</returns>
        /// <exception cref="EntityNotFoundException">throws if not found </exception>
        /// <exception cref="Exception">General exception</exception>
        public async Task<IEnumerable<ReturnBookDTO>> SearchBookByTitlePaginated(string title, int page , int limit)
        {
            try
            {
                var books = await ((BookRepository)_bookRepository).GetBookByTitle(title);

                books = books.Skip((page - 1) * limit).Take(limit);

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
                        ISBN = book.ISBN,
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
        
        /// <summary>
        /// updates the book based 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException">throws if book not found</exception>
        /// <exception cref="Exception"></exception>
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
                    Quantity = stock.Quantity,
                    ISBN = book.ISBN,
                    
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

        /// <summary>
        /// used to edit author name
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException">throws if author id is not found</exception>
        /// <exception cref="Exception"></exception>

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

        /// <summary>
        /// edits the publication of the book 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException">thorws if PublicationId is not found</exception>
        /// <exception cref="Exception">Geeral Exception</exception>
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

        /// <summary>
        /// Get all the location in the library
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Location>> GetAllLocationsInLibrary()
        {
            try
            {
                return await _locationRepository.Get();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
    }
}
