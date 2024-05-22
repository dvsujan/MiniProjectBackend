namespace LibraryManagemetApi.Interfaces
{
    public interface IRepository<K, T> where T:class
    {
        Task<IEnumerable<T>> Get();
        Task<T> GetOneById(K id);
        Task<T> Insert(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(K id);
    }
}
