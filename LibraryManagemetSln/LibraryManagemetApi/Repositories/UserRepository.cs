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
