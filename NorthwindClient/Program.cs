using NorthwindClient;

// The products API from Exercise: Northwind Web API must be running. Use its address here.
using HttpClient http = new HttpClient { BaseAddress = new Uri("http://localhost:5179/") };
ProductClient products = new ProductClient(http);

try
{
    Console.WriteLine("Beverages:");
    foreach (Product product in await products.GetAllAsync(1))
    {
        Console.WriteLine($"  {product.ProductName} {product.UnitPrice:C}");
    }

    Console.WriteLine("Running low (fewer than 5 in stock):");
    foreach (Product product in await products.LowStockAsync(5))
    {
        Console.WriteLine($"  {product.ProductName}: {product.UnitsInStock ?? 0}");
    }

    Product? missing = await products.GetByIdAsync(999);
    Console.WriteLine(missing == null ? "There's no product 999" : missing.ProductName);
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"The products API had a problem: {ex.Message}");
}
