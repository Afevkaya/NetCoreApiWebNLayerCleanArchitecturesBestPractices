using App.Repositories.Products;
using App.Services.Products.Create;
using App.Services.Products.Update;

namespace App.Services.Products;

public interface IProductService
{
    Task<ServiceResult<List<ProductResponse>>> GetAllAsync();
    Task<ServiceResult<List<ProductResponse>>> GetPagedAllListAsync(int pageNumber, int pageSize);
    Task<ServiceResult<List<ProductResponse>>> GetTopPriceAsync(int count);
    Task<ServiceResult<ProductResponse>> GetByIdAsync(int id);
    Task<ServiceResult<ProductCreateResponse>> CreateAsync(ProductCreateRequest request);
    Task<ServiceResult> UpdateAsync(int id, ProductUpdateRequest request);
    Task<ServiceResult> UpdateStockAsync(ProductUpdateStockRequest request);
    Task<ServiceResult> DeleteAsync(int id);
}