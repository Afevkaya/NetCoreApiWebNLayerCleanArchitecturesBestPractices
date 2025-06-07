using System.Net;
using App.Repositories.Products;
using App.Repositories.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace App.Services.Products;

public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork): IProductService
{
    // Fast Fail Pattern: Önce olumsuz durumları kontrol et ve erken çık.
    // Guard Clause Pattern: Parametrelerin geçerliliğini kontrol et ve hataları erken bildir.
    public async Task<ServiceResult<List<ProductResponse>>> GetAllAsync()
    {
        var products = await productRepository.GetAll().ToListAsync();
        if (products.Count == 0)
            return ServiceResult<List<ProductResponse>>.Fail("No products found", HttpStatusCode.NotFound);
        
        var productResponses = products.Select(p => new ProductResponse(p.Id, p.Name, p.Price, p.Stock)).ToList();
        return ServiceResult<List<ProductResponse>>.Success(productResponses);
    }
    public async Task<ServiceResult<List<ProductResponse>>> GetTopPriceAsync(int count)
    {
        var products = await productRepository.GetTopPriceProductsAsync(count);
        if (products.Count == 0)
            return ServiceResult<List<ProductResponse>>.Fail("No products found", HttpStatusCode.NotFound);
        
        var productResponses = products.Select(p => new ProductResponse(p.Id, p.Name, p.Price, p.Stock)).ToList();
        return ServiceResult<List<ProductResponse>>.Success(productResponses);
    }
    public async Task<ServiceResult<ProductResponse>> GetByIdAsync(int id)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product == null)
            return ServiceResult<ProductResponse>.Fail("Product not found", HttpStatusCode.NotFound);
        
        var productResponse = new ProductResponse(product.Id, product.Name, product.Price, product.Stock);
        return ServiceResult<ProductResponse>.Success(productResponse);
    }
    public async Task<ServiceResult<ProductCreateResponse>> CreateAsync(ProductCreateRequest request)
    {
        if (request == null)
            return ServiceResult<ProductCreateResponse>.Fail("Invalid request");

        var product = new Product
        {
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock
        };
        
        await productRepository.InsertAsync(product);
        
        var result = await unitOfWork.CommitAsync();
        return result <= 0 ? ServiceResult<ProductCreateResponse>.Fail("Failed to create product", HttpStatusCode.InternalServerError) 
            : ServiceResult<ProductCreateResponse>.Success(new ProductCreateResponse(product.Id));
    }
    public async Task<ServiceResult> UpdateAsync(int id, ProductUpdateRequest request)
    {
        if (request == null)
            return ServiceResult.Fail("Invalid request");

        var product = await productRepository.GetByIdAsync(id);
        if (product == null)
            return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
        
        product.Name = request.Name;
        product.Price = request.Price;
        product.Stock = request.Stock;
        
        productRepository.Update(product);
        var result = await unitOfWork.CommitAsync();
        
        return result <= 0 ? ServiceResult.Fail("Failed to update product", HttpStatusCode.InternalServerError) 
            : ServiceResult.Success();
    }
    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product == null)
            return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
        
        productRepository.Delete(product);
        var result = await unitOfWork.CommitAsync();
        
        return result <= 0 ? ServiceResult.Fail("Failed to delete product", HttpStatusCode.InternalServerError) 
            : ServiceResult.Success();
    }
}