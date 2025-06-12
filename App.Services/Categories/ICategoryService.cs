using App.Services.Categories.Create;
using App.Services.Categories.Response;
using App.Services.Categories.Update;

namespace App.Services.Categories;

public interface ICategoryService
{
    Task<ServiceResult<CategoryWithProductsResponse>> GetByIdWithProductsAsync(int id);
    Task<ServiceResult<List<CategoryWithProductsResponse>>> GetCategoriesByProductsAsync();
    Task<ServiceResult<List<CategoryResponse>>> GetAllAsync();
    Task<ServiceResult<CategoryResponse>> GetByIdAsync(int id);
    Task<ServiceResult<int>> CreateAsync(CategoryCreateRequest request);
    Task<ServiceResult> UpdateAsync(int id, CategoryUpdateRequest request);
    Task<ServiceResult> DeleteAsync(int id);
}