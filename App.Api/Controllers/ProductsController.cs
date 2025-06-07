using App.Api.Controllers.Base;
using App.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers;

public class ProductsController(IProductService productService) : CustomController
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => CreateActionResult( await productService.GetAllAsync());
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) => CreateActionResult(await productService.GetByIdAsync(id));
    
    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateRequest request) => CreateActionResult(await productService.CreateAsync(request));
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ProductUpdateRequest request) => CreateActionResult(await productService.UpdateAsync(id,request));
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => CreateActionResult(await productService.DeleteAsync(id));
}