namespace App.Services.Products.Response;

public record ProductResponse(int Id, string Name, decimal Price, int Stock, int CategoryId);