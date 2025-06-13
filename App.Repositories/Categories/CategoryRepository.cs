using App.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories.Categories;

public class CategoryRepository(AppDbContext context) :GenericRepository<Category,int>(context), ICategoryRepository
{
    public Task<Category?> GetCategoryWithProductsAsync(int id)
    {
        return Context.Categories
            .Include(c => c.Products)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public IQueryable<Category> GetCategoriesByProducts()
    {
        return Context.Categories
            .Include(c => c.Products)
            .AsNoTracking();
    }
}