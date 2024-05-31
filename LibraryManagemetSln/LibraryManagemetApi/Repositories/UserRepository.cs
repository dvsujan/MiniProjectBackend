using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagemetApi.Repositories
{
    public class UserRepository: AbstractRepositoryClass<int, User>
    {
        private readonly LibraryManagementContext _context;
        public UserRepository(LibraryManagementContext context) : base(context)
        {
        }

        /// <summary>
        /// returns the user object based on the email 
        /// </summary>
        /// <param name="email">string</param>
        /// <returns>returns user object</returns>
        /// <exception cref="EntityNotFoundException">thorws if user does not exist</exception>
        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                throw new EntityNotFoundException();
            }
            return user;
        }
    }
}
