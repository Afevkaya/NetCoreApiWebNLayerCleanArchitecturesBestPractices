using App.Services.Products;
using App.Services.Products.Response;

namespace App.Services.Categories.Response;

public record CategoryWithProductsResponse(int Id, string Name, List<ProductResponse> Products);