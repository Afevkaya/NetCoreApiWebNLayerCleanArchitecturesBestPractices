using App.Repositories.Products;

namespace App.Services.Products;

public interface IProductService
{
    Task<ServiceResult<List<ProductResponse>>> GetTopPriceProductsAsync(int count);
    Task<ServiceResult<ProductResponse>> GetProductByIdAsync(int id);
    Task<ServiceResult<ProductCreateResponse>> CreateProductAsync(ProductCreateRequest request);
    Task<ServiceResult> UpdateProductAsync(int id, ProductUpdateRequest request);
    Task<ServiceResult> DeleteProductAsync(int id);
}