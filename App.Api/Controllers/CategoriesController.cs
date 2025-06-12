using App.Api.Controllers.Base;
using App.Services.Categories;
using App.Services.Categories.Create;
using App.Services.Categories.Update;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers;

public class CategoriesController(ICategoryService categoryService) : CustomController
{
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await categoryService.GetAllAsync();
        return CreateActionResult(categories);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        var category = await categoryService.GetByIdAsync(id);
        return CreateActionResult(category);
    }

    [HttpGet("{id:int}/products")]
    public async Task<IActionResult> GetCategoryWithProducts(int id)
    {
        var categoryWithProducts = await categoryService.GetByIdWithProductsAsync(id);
        return CreateActionResult(categoryWithProducts);
    }
    
    [HttpGet("products")]
    public async Task<IActionResult> GetCategoryWithProducts()
    {
        var categoriesWithProducts = await categoryService.GetCategoriesByProductsAsync();
        return CreateActionResult(categoriesWithProducts);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateRequest categoryRequest)
    {
        var createdCategory = await categoryService.CreateAsync(categoryRequest);
        return CreateActionResult(createdCategory);
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateRequest categoryRequest)
    {
        var updatedCategory = await categoryService.UpdateAsync(id, categoryRequest);
        return CreateActionResult(updatedCategory);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var deletedCategory = await categoryService.DeleteAsync(id);
        return CreateActionResult(deletedCategory);
    }
}