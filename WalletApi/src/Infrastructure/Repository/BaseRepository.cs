using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WalletApi.Application.Interfaces.Repositories;
using WalletApi.Infrastructure.Data;

namespace WalletApi.Infrastructure.Repository;

public class BaseRepository<T>(AppDbContext context) : IBaseRepository<T>
    where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(string id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public IQueryable<T> AsQueryable()
    {
        return _dbSet.AsNoTracking();
    }
    
}