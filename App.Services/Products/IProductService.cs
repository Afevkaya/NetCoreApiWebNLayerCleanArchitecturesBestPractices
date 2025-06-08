using App.Repositories.Products;

namespace App.Services.Products;

public interface IProductService
{
    Task<ServiceResult<List<ProductResponse>>> GetAllAsync();
    Task<ServiceResult<List<ProductResponse>>> GetPagedAllListAsync(int pageNumber, int pageSize);
    Task<ServiceResult<List<ProductResponse>>> GetTopPriceAsync(int count);
    Task<ServiceResult<ProductResponse>> GetByIdAsync(int id);
    Task<ServiceResult<ProductCreateResponse>> CreateAsync(ProductCreateRequest request);
    Task<ServiceResult> UpdateAsync(int id, ProductUpdateRequest request);
    Task<ServiceResult> DeleteAsync(int id);
}