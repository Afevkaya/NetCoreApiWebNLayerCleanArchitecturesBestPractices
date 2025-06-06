using App.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories.Products;

public class ProductRepository(AppDbContext context) : GenericRepository<Product>(context), IProductRepository
{
    public Task<List<Product>> GetTopPriceProductAsync(int count) 
        => Context.Products.OrderByDescending(p=> p.Price).Take(count).ToListAsync();
    
}