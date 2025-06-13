using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories;

public interface IGenericRepository<T, TId> where T : class where TId : struct
{
    IQueryable<T> GetAll();
    IQueryable<T> Get(Expression<Func<T, bool>> predicate);
    Task<bool> AnyAsync(TId id);
    ValueTask<T?> GetByIdAsync(int id);
    ValueTask InsertAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    
}