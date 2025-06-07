namespace App.Services.Products;

public record ProductUpdateRequest(int Id, string Name, decimal Price, int Stock);