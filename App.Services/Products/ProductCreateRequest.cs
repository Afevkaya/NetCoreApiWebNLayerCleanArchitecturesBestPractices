namespace App.Services.Products;

public record ProductCreateRequest(string Name, decimal Price, int Stock);