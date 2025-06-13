using System.Linq.Expressions;
using App.Repositories.BaseEntites;
using App.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories;

public class GenericRepository<T,TId>(AppDbContext context) 
    : IGenericRepository<T, TId> where T 
    : BaseEntity<TId> where TId : struct
{
    protected AppDbContext Context = context;
    private readonly DbSet<T> _dbSet = context.Set<T>();
    
    public IQueryable<T> GetAll() => _dbSet.AsNoTracking();
    public IQueryable<T> Get(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).AsNoTracking();
    public async Task<bool> AnyAsync(TId id) => await _dbSet.AnyAsync(x=>x.Id.Equals(id));
    public async ValueTask<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
    public async ValueTask InsertAsync(T entity) => await _dbSet.AddAsync(entity);
    public void Update(T entity) => _dbSet.Update(entity);
    public void Delete(T entity) => _dbSet.Remove(entity);
}