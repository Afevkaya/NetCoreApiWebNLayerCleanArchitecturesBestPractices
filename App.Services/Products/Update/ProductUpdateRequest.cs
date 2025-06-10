namespace App.Services.Products.Update;

public record ProductUpdateRequest(string Name, decimal Price, int Quantity);