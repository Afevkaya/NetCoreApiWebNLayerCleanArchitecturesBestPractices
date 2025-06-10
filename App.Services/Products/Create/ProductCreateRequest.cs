namespace App.Services.Products.Create;

public record ProductCreateRequest(string Name, decimal Price, int Quantity);