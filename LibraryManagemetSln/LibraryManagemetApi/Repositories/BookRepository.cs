﻿using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagemetApi.Repositories
{
    public class BookRepository : AbstractRepositoryClass<int, Book>
    {
        private readonly LibraryManagementContext _context;
        public BookRepository(LibraryManagementContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Book>> GetBookByTitle(string title)
        {
            var books = await _dbSet.Where(b => b.Title.Contains(title)).Include(b=>b.Author).Include(b=>b.Category).Include(b=>b.Location).ToListAsync();
            return books;
        }
        public override async Task<IEnumerable<Book>> Get()
        {
            var books = await _dbSet.Include(b => b.Location).Include(b=>b.Author).Include(b => b.Category).ToListAsync();
            return books;
        }

        public async Task<IEnumerable<Book>> GetPaginated(int page , int limit)
        {
            var books = await _dbSet.Include(b => b.Location).Include(b=>b.Category).Skip((page - 1) * limit).Take(limit).ToListAsync();

            return books;
        }

        public override async Task<Book> GetOneById(int id)
        {
            var book = await _dbSet.Include(b => b.Location).Include(b=>b.Author).Include(b => b.Category).FirstOrDefaultAsync(b => b.Id == id);
            return book ?? throw new EntityNotFoundException();
        }
    }
}
