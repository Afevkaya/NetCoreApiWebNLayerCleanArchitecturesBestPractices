using System.Net;
using App.Repositories.Products;
using App.Repositories.UnitOfWorks;
using App.Services.Categories;
using App.Services.Products.Create;
using App.Services.Products.Response;
using App.Services.Products.Update;
using App.Services.Products.UpdateStock;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.Services.Products;

public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper, ICategoryService categoryService): IProductService
{
    // Fast Fail Pattern: Önce olumsuz durumları kontrol et ve erken çık.
    // Guard Clause Pattern: Parametrelerin geçerliliğini kontrol et ve hataları erken bildir.
    public async Task<ServiceResult<List<ProductResponse>>> GetAllAsync()
    {
        #region Exception Handling Example
        // throw new CriticalException("Kritik bir hata oluştu, lütfen daha sonra tekrar deneyin.");
        // throw new Exception("Hata oluştu, lütfen daha sonra tekrar deneyin.");
        #endregion
        
        var products = await productRepository.GetAll().ToListAsync();
        if (products.Count == 0)
            return ServiceResult<List<ProductResponse>>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);

        #region Manuel Mapping Example
        // var productResponses = products.Select(p => new ProductResponse(p.Id, p.Name, p.Price, p.Stock)).ToList();
        #endregion
        
        var productResponses = mapper.Map<List<ProductResponse>>(products);
        return ServiceResult<List<ProductResponse>>.Success(productResponses);
    }
    public async Task<ServiceResult<List<ProductResponse>>> GetPagedAllListAsync(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0 || pageSize <= 0)
            return ServiceResult<List<ProductResponse>>.Fail("Geçersiz parametre");
        
        var products = await productRepository.GetAll()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        if (products.Count == 0)
            return ServiceResult<List<ProductResponse>>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);
        
        #region Manuel Mapping Example
        // var productResponses = products.Select(p => new ProductResponse(p.Id, p.Name, p.Price, p.Stock)).ToList();
        #endregion
        
        var productResponses = mapper.Map<List<ProductResponse>>(products);
        return ServiceResult<List<ProductResponse>>.Success(productResponses);
    }
    public async Task<ServiceResult<List<ProductResponse>>> GetTopPriceAsync(int count)
    {
        var products = await productRepository.GetTopPriceProductsAsync(count);
        if (products.Count == 0)
            return ServiceResult<List<ProductResponse>>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);
        
        #region Manuel Mapping Example
        // var productResponses = products.Select(p => new ProductResponse(p.Id, p.Name, p.Price, p.Stock)).ToList();
        #endregion
        
        var productResponses = mapper.Map<List<ProductResponse>>(products);
        return ServiceResult<List<ProductResponse>>.Success(productResponses);
    }
    public async Task<ServiceResult<ProductResponse>> GetByIdAsync(int id)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product == null)
            return ServiceResult<ProductResponse>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);
        
        #region Manuel Mapping Example
        // var productResponse = new ProductResponse(product.Id, product.Name, product.Price, product.Stock);
        #endregion
        
        var productResponse = mapper.Map<ProductResponse>(product);
        return ServiceResult<ProductResponse>.Success(productResponse);
    }
    public async Task<ServiceResult<ProductCreateResponse>> CreateAsync(ProductCreateRequest request)
    {
        var anyProduct = await productRepository.Get(p => p.Name == request.Name).AnyAsync();
        if (anyProduct)
            return ServiceResult<ProductCreateResponse>.Fail("Aynı ürün bulunmaktadır ", HttpStatusCode.Conflict);
        
        var category = await categoryService.GetByIdAsync(request.CategoryId);
        if (category.IsFail)
            return ServiceResult<ProductCreateResponse>.Fail(category.ErrorMessages!, category.HttpStatusCode);

        var product = mapper.Map<Product>(request);
        await productRepository.InsertAsync(product);
        var result = await unitOfWork.CommitAsync();
        return result <= 0 
            ? ServiceResult<ProductCreateResponse>.Fail("Sistemsel hata meydana geldi", HttpStatusCode.InternalServerError) 
            : ServiceResult<ProductCreateResponse>.SuccessAsCreated(new ProductCreateResponse(product.Id),$"api/products/{product.Id}");
    }
    public async Task<ServiceResult> UpdateAsync(int id, ProductUpdateRequest request)
    {
        var isProductNameExist = await productRepository.Get(p => p.Name == request.Name && p.Id != id).AnyAsync();
        if (isProductNameExist)
            return ServiceResult.Fail("Aynı ürün bulunmaktadır ", HttpStatusCode.Conflict);
        
        var category = await categoryService.GetByIdAsync(request.CategoryId);
        if (category.IsFail)
            return ServiceResult.Fail(category.ErrorMessages!, category.HttpStatusCode);
        
        #region Manuel Mapping Example
        // product.Name = request.Name;
        // product.Price = request.Price;
        // product.Stock = request.Stock;
        #endregion
        
        var product = mapper.Map<Product>(request);
        product.Id = id;
        productRepository.Update(product);
        var result = await unitOfWork.CommitAsync();
        return result <= 0 
            ? ServiceResult.Fail("Sistemsel hata meydana geldi", HttpStatusCode.InternalServerError) 
            : ServiceResult.Success();
    }
    public async Task<ServiceResult> UpdateStockAsync(ProductUpdateStockRequest request)
    {
        var product = await productRepository.GetByIdAsync(request.Id);
        if (product == null)
            return ServiceResult.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);

        if (request.Stock < 0)
            return ServiceResult.Fail("Stok miktarı negatif olamaz");
        
        product.Stock = request.Stock;
        productRepository.Update(product);
        var result = await unitOfWork.CommitAsync();
        
        return result <= 0 
            ? ServiceResult.Fail("Sistemsel hata meydana geldi", HttpStatusCode.InternalServerError) 
            : ServiceResult.Success();
    }
    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var product = await productRepository.GetByIdAsync(id);
        productRepository.Delete(product!);
        var result = await unitOfWork.CommitAsync();
        
        return result <= 0 
            ? ServiceResult.Fail("Sistemsel hata meydana geldi", HttpStatusCode.InternalServerError) 
            : ServiceResult.Success();
    }
}