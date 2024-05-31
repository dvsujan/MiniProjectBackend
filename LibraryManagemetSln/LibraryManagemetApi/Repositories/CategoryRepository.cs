using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagemetApi.Repositories
{
    public class CategoryRepository:AbstractRepositoryClass<int, Category>
    {
        private readonly LibraryManagementContext _context;
        public CategoryRepository(LibraryManagementContext context) : base(context)
        {
        }
        /// <summary>
        /// returns the category based on the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>category obj</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<Category> GetCategoryByName(string name)
        {
            var category = await _dbSet.FirstOrDefaultAsync(c => c.Name == name);
            if (category == null)
            {
                throw new EntityNotFoundException();
            }
            return category;
        }
    }
}
