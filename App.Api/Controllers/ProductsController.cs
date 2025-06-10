using App.Api.Controllers.Base;
using App.Services.Products;
using App.Services.Products.Create;
using App.Services.Products.Update;
using App.Services.Products.UpdateStock;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers;

public class ProductsController(IProductService productService) : CustomController
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => 
        CreateActionResult( await productService.GetAllAsync());
    
    [HttpGet("{pageNumber:int}/{pageSize:int}")]
    public async Task<IActionResult> GetPagedAllList(int pageNumber, int pageSize) => 
        CreateActionResult(await productService.GetPagedAllListAsync(pageNumber, pageSize));
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) => 
        CreateActionResult(await productService.GetByIdAsync(id));
    
    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateRequest request) => 
        CreateActionResult(await productService.CreateAsync(request));
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ProductUpdateRequest request) => 
        CreateActionResult(await productService.UpdateAsync(id,request));
    
    [HttpPatch("update-stock")]
    public async Task<IActionResult> UpdateStock(int id, ProductUpdateStockRequest request) =>
        CreateActionResult(await productService.UpdateStockAsync(request));
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => 
        CreateActionResult(await productService.DeleteAsync(id));
}