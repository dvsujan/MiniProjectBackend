using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagemetApi.Repositories
{
    public class PublisherRepository:AbstractRepositoryClass<int, Publisher>
    {
        private readonly LibraryManagementContext _context;
        public PublisherRepository(LibraryManagementContext context) : base(context)
        {

        }
        /// <summary>
        /// get the publisher object based on the name 
        /// </summary>
        /// <param name="name">string</param>
        /// <returns>publisherobj</returns>
        /// <exception cref="EntityNotFoundException">if not found in db</exception>
        public async Task<Publisher> GetPublisherByName(string name)
        {
            var publisher = await _dbSet.FirstOrDefaultAsync(p => p.Name == name);
            if (publisher == null)
            {
                throw new EntityNotFoundException();
            }
            return publisher;
        }
    }
}
