using System.Linq.Expressions;

namespace WalletApi.Application.Interfaces.Repositories;

public interface IBaseRepository <T> where T : class
{
    Task<T?> GetByIdAsync(string id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task SaveChangesAsync();
    IQueryable<T> AsQueryable();
}