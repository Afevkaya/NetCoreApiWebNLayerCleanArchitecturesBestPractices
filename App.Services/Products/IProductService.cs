using App.Repositories.Products;

namespace App.Services.Products;

public interface IProductService
{
    Task<List<Product>> GetTopPriceProductsAsync(int count);
}