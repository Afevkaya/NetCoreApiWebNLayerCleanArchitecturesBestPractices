using System.Linq.Expressions;

namespace App.Repositories;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> GetAll();
    IQueryable<T> Get(Expression<Func<T, bool>> predicate);
    ValueTask<T?> GetByIdAsync(int id);
    ValueTask InsertAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    
}