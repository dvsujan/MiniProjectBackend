using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagemetApi.Repositories
{
    public abstract class AbstractRepositoryClass<K, T> : IRepository<K, T> where T : class
    {
        protected readonly LibraryManagementContext _context;
        protected readonly DbSet<T> _dbSet;
        public AbstractRepositoryClass(LibraryManagementContext context)
        {
            this._context = context;
            this._dbSet = context.Set<T>();
        }
        
        public async virtual Task<T> Delete(K id)
        {
            var ob = await GetOneById(id);
            if (ob == null)
            {
                return null;
            }
            _dbSet.Remove(ob);
            await _context.SaveChangesAsync();
            return ob;
        }

        public async virtual Task<IEnumerable<T>> Get()
        {
            return await _dbSet.ToListAsync();
        }

        public async virtual Task<T> GetOneById(K id)
        {
            T ob = await _dbSet.FindAsync(id);
            if (ob == null)
            {
                throw new EntityNotFoundException(); 
            }
            return ob;
        }
        
        public async virtual Task<T> Insert(T entity)
        {
            var ob = await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return ob.Entity;
        }

        public async virtual Task<T> Update(T entity)
        {
            _dbSet.Attach(entity); 
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
