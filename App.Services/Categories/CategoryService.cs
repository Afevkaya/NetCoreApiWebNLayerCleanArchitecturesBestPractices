using System.Net;
using App.Repositories.Categories;
using App.Repositories.UnitOfWorks;
using App.Services.Categories.Create;
using App.Services.Categories.Response;
using App.Services.Categories.Update;
using App.Services.Products;
using App.Services.Products.Response;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.Services.Categories;

public class CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IUnitOfWork unitOfWork):ICategoryService
{
    public async Task<ServiceResult<CategoryWithProductsResponse>> GetByIdWithProductsAsync(int id)
    {
        var categoryWithProducts = await categoryRepository.GetCategoryWithProductsAsync(id);
        if (categoryWithProducts == null)
            return ServiceResult<CategoryWithProductsResponse>.Fail("Kategori bulunamadı", HttpStatusCode.NotFound);
        
        var response = mapper.Map<CategoryWithProductsResponse>(categoryWithProducts);
        return ServiceResult<CategoryWithProductsResponse>.Success(response);
    }

    public async Task<ServiceResult<List<CategoryWithProductsResponse>>> GetCategoriesByProductsAsync()
    {
        var categories = await categoryRepository.GetCategoriesByProducts().ToListAsync();
        if (categories.Count == 0)
            return ServiceResult<List<CategoryWithProductsResponse>>.Fail("Kategori bulunamadı", HttpStatusCode.NotFound);

        var response = mapper.Map<List<CategoryWithProductsResponse>>(categories);
        return ServiceResult<List<CategoryWithProductsResponse>>.Success(response);
    }


    public async Task<ServiceResult<List<CategoryResponse>>> GetAllAsync()
    {
        var categories = await categoryRepository.GetAll().ToListAsync();
        if (categories.Count == 0)
            return ServiceResult<List<CategoryResponse>>.Fail("Kategori bulunamadı", HttpStatusCode.NotFound);

        var response = mapper.Map<List<CategoryResponse>>(categories);
        return ServiceResult<List<CategoryResponse>>.Success(response);
    }
    
    public async Task<ServiceResult<CategoryResponse>> GetByIdAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        if (category == null)
            return ServiceResult<CategoryResponse>.Fail("Kategori bulunamadı", HttpStatusCode.NotFound);

        var response = mapper.Map<CategoryResponse>(category);
        return ServiceResult<CategoryResponse>.Success(response);
    }
    
    public async Task<ServiceResult<int>> CreateAsync(CategoryCreateRequest request)
    {
        var anyCategory = await categoryRepository.Get(p => p.Name == request.Name).AnyAsync();
        if (anyCategory)
            return ServiceResult<int>.Fail("Aynı ürün bulunmaktadır ", HttpStatusCode.Conflict);
        
        var category = mapper.Map<Category>(request);
        await categoryRepository.InsertAsync(category);
        await unitOfWork.CommitAsync();
        return ServiceResult<int>.Success(category.Id);
    }

    public async Task<ServiceResult> UpdateAsync(int id, CategoryUpdateRequest request)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        if (category == null)
            return ServiceResult.Fail("Kategori bulunamadı", HttpStatusCode.NotFound);

        var anyCategory = await categoryRepository.Get(p => p.Name == request.Name && p.Id != id).AnyAsync();
        if (anyCategory)
            return ServiceResult.Fail("Aynı isimde kategori bulunmaktadır", HttpStatusCode.Conflict);

        mapper.Map(request, category);
        categoryRepository.Update(category);
        await unitOfWork.CommitAsync();
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        if (category == null)
            return ServiceResult.Fail("Kategori bulunamadı", HttpStatusCode.NotFound);

        categoryRepository.Delete(category);
        await unitOfWork.CommitAsync();
        return ServiceResult.Success();
    }
}