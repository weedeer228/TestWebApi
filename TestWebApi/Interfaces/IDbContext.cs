namespace TestWebApi.Interfaces;

public interface IDbContext<K, T> where T : class
{
    public Task<T?> GetAsync(K key);

    public Task<ICollection<T>> GetAllAsync();

    public Task<Game?> CreateAsync(T entity);

    public Task<Game?> UpdateAsync(K key, T entity);

    public Task<bool> DeleteAsync(K key);

    public Task<IList<T>> GetByAsync(Func<T, bool> predicate) => null;
}
