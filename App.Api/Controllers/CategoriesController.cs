using App.Api.Controllers.Base;
using App.Repositories.Categories;
using App.Services.Categories;
using App.Services.Categories.Create;
using App.Services.Categories.Update;
using App.Services.Filters;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers;

public class CategoriesController(ICategoryService categoryService) : CustomController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await categoryService.GetAllAsync();
        return CreateActionResult(categories);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await categoryService.GetByIdAsync(id);
        return CreateActionResult(category);
    }

    [HttpGet("{id:int}/products")]
    public async Task<IActionResult> GetWithProductsById(int id)
    {
        var categoryWithProducts = await categoryService.GetByIdWithProductsAsync(id);
        return CreateActionResult(categoryWithProducts);
    }
    
    [HttpGet("products")]
    public async Task<IActionResult> GetAllWithProducts()
    {
        var categoriesWithProducts = await categoryService.GetCategoriesByProductsAsync();
        return CreateActionResult(categoriesWithProducts);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryCreateRequest categoryRequest)
    {
        var createdCategory = await categoryService.CreateAsync(categoryRequest);
        return CreateActionResult(createdCategory);
    }
    
    [ServiceFilter(typeof(NotFoundFilter<Category,int>))]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateRequest categoryRequest)
    {
        var updatedCategory = await categoryService.UpdateAsync(id, categoryRequest);
        return CreateActionResult(updatedCategory);
    }
    
    [ServiceFilter(typeof(NotFoundFilter<Category,int>))]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletedCategory = await categoryService.DeleteAsync(id);
        return CreateActionResult(deletedCategory);
    }
}