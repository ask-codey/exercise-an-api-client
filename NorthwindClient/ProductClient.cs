namespace NorthwindClient;

// Calls the products API from Exercise: Northwind Web API.
//
// Requirements:
// - The client is given its HttpClient, with the API's base address already set. It never makes its own.
// - GetAllAsync(categoryId) sends GET api/products, or GET api/products?categoryId=1 when a category is
//   given, and returns the products.
// - GetByIdAsync(productId) sends GET api/products/{productId}, and returns the product, or null when the
//   API answers 404.
// - AddAsync(product) sends POST api/products with the product as JSON, and returns the product the API
//   sends back (with its new id).
// - UpdateAsync(product) sends PUT api/products/{the product's id} with the product as JSON, and returns
//   true, or false when the API answers 404.
// - DeleteAsync(productId) sends DELETE api/products/{productId}, and returns true, or false when the API
//   answers 404.
// - Any other unsuccessful answer, such as 400 or 500, throws an HttpRequestException.
// - LowStockAsync(threshold) returns the products with fewer than threshold units in stock (a product with
//   no stock figure counts as 0), fewest first, and products with the same stock in order of name.
public class ProductClient
{
    private readonly HttpClient client;

    public ProductClient(HttpClient client)
    {
        this.client = client;
    }

    public async Task<List<Product>> GetAllAsync(int? categoryId)
    {
        return new List<Product>();
    }

    public async Task<Product?> GetByIdAsync(int productId)
    {
        return null;
    }

    public async Task<Product> AddAsync(Product product)
    {
        return product;
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        return false;
    }

    public async Task<bool> DeleteAsync(int productId)
    {
        return false;
    }

    public async Task<List<Product>> LowStockAsync(int threshold)
    {
        return new List<Product>();
    }
}
