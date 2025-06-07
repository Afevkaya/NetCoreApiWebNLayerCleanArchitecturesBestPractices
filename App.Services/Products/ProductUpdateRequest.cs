namespace App.Services.Products;

public record ProductUpdateRequest(string Name, decimal Price, int Stock);