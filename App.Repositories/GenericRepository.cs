using System.Linq.Expressions;
using App.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories;

public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : class
{
    protected AppDbContext Context = context;
    private readonly DbSet<T> _dbSet = context.Set<T>();
    
    public IQueryable<T> GetAll() => _dbSet.AsNoTracking();

    public IQueryable<T> Get(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).AsNoTracking();

    public async ValueTask<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

    public async ValueTask InsertAsync(T entity) => await _dbSet.AddAsync(entity);

    public void Update(T entity) => _dbSet.Update(entity);

    public void Delete(T entity) => _dbSet.Remove(entity);
}