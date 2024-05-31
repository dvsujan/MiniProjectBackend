﻿using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagemetApi.Repositories
{
    public class AuthorRepository: AbstractRepositoryClass<int, Author>
    {
        private readonly LibraryManagementContext _context;
        public AuthorRepository(LibraryManagementContext context) : base(context)
        {
        }
        /// <summary>
        /// gets the user by name 
        /// </summary>
        /// <param name="name">string</param>
        /// <returns>entity</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<Author> GetAuthorByName(string name)
        {
            var author = await _dbSet.FirstOrDefaultAsync(x => x.Name == name);
            if (author == null)
            {
                throw new EntityNotFoundException();
            }
            return author;
        }
    }
}
