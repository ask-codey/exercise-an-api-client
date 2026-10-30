using System.Net;

namespace NorthwindClient.Tests;

// These tests give ProductClient an HttpClient that talks to a FakeApi instead of a real server,
// so they need nothing running. You don't need to change this file.
[TestClass]
public class ProductClientTests
{
    private const string ThreeProducts = """
        [
          { "productId": 1, "productName": "Chai", "categoryId": 1, "unitPrice": 18.0, "unitsInStock": 39, "discontinued": false },
          { "productId": 2, "productName": "Chang", "categoryId": 1, "unitPrice": 19.0, "unitsInStock": 3, "discontinued": false },
          { "productId": 3, "productName": "Aniseed Syrup", "categoryId": 2, "unitPrice": 10.0, "unitsInStock": 3, "discontinued": false }
        ]
        """;

    private const string Chai = """
        { "productId": 1, "productName": "Chai", "categoryId": 1, "unitPrice": 18.0, "unitsInStock": 39, "discontinued": false }
        """;

    private FakeApi api = null!;
    private ProductClient client = null!;

    [TestInitialize]
    public void MakeTheClient()
    {
        api = new FakeApi();
        client = new ProductClient(api.CreateClient());
    }

    [TestMethod]
    public async Task GetAllAsync_NoCategory_GetsEveryProduct()
    {
        api.Json = ThreeProducts;

        List<Product> products = await client.GetAllAsync(null);

        Assert.AreEqual(HttpMethod.Get, api.Method);
        Assert.AreEqual("/api/products", api.PathAndQuery);
        Assert.HasCount(3, products);
        Assert.AreEqual("Chang", products[1].ProductName);
    }

    [TestMethod]
    public async Task GetAllAsync_WithCategory_AsksForThatCategory()
    {
        api.Json = ThreeProducts;

        await client.GetAllAsync(1);

        Assert.AreEqual("/api/products?categoryId=1", api.PathAndQuery);
    }

    [TestMethod]
    public async Task GetByIdAsync_Found_ReturnsTheProduct()
    {
        api.Json = Chai;

        Product? product = await client.GetByIdAsync(1);

        Assert.AreEqual("/api/products/1", api.PathAndQuery);
        Assert.AreEqual("Chai", product?.ProductName);
        Assert.AreEqual(18.0m, product?.UnitPrice);
    }

    [TestMethod]
    public async Task GetByIdAsync_404_ReturnsNull()
    {
        api.Status = HttpStatusCode.NotFound;

        Product? product = await client.GetByIdAsync(999);

        Assert.AreEqual("/api/products/999", api.PathAndQuery, "It should still ask the API.");
        Assert.IsNull(product);
    }

    [TestMethod]
    public async Task GetByIdAsync_500_Throws()
    {
        api.Status = HttpStatusCode.InternalServerError;

        await Assert.ThrowsExactlyAsync<HttpRequestException>(() => client.GetByIdAsync(1));
    }

    [TestMethod]
    public async Task AddAsync_NewProduct_PostsItAsJsonAndReturnsTheAnswer()
    {
        api.Status = HttpStatusCode.Created;
        api.Json = """{ "productId": 78, "productName": "Trail Mix", "categoryId": 3, "unitPrice": 4.5, "unitsInStock": 20, "discontinued": false }""";

        Product added = await client.AddAsync(new Product { ProductName = "Trail Mix", CategoryId = 3, UnitPrice = 4.5m, UnitsInStock = 20 });

        Assert.AreEqual(HttpMethod.Post, api.Method);
        Assert.AreEqual("/api/products", api.PathAndQuery);
        Assert.Contains("\"productName\":\"Trail Mix\"", api.Body ?? "");
        Assert.AreEqual(78, added.ProductId);
    }

    [TestMethod]
    public async Task AddAsync_400_Throws()
    {
        api.Status = HttpStatusCode.BadRequest;

        await Assert.ThrowsExactlyAsync<HttpRequestException>(() => client.AddAsync(new Product()));
    }

    [TestMethod]
    public async Task UpdateAsync_204_PutsTheProductAndReturnsTrue()
    {
        api.Status = HttpStatusCode.NoContent;

        bool updated = await client.UpdateAsync(new Product { ProductId = 7, ProductName = "Uncle Bob's Organic Dried Pears" });

        Assert.AreEqual(HttpMethod.Put, api.Method);
        Assert.AreEqual("/api/products/7", api.PathAndQuery);
        Assert.Contains("Dried Pears", api.Body ?? "");
        Assert.IsTrue(updated);
    }

    [TestMethod]
    public async Task UpdateAsync_404_ReturnsFalse()
    {
        api.Status = HttpStatusCode.NotFound;

        bool updated = await client.UpdateAsync(new Product { ProductId = 999, ProductName = "Nothing" });

        Assert.AreEqual(HttpMethod.Put, api.Method, "It should still ask the API.");
        Assert.IsFalse(updated);
    }

    [TestMethod]
    public async Task DeleteAsync_204_ReturnsTrue()
    {
        api.Status = HttpStatusCode.NoContent;

        bool deleted = await client.DeleteAsync(7);

        Assert.AreEqual(HttpMethod.Delete, api.Method);
        Assert.AreEqual("/api/products/7", api.PathAndQuery);
        Assert.IsTrue(deleted);
    }

    [TestMethod]
    public async Task DeleteAsync_404_ReturnsFalse()
    {
        api.Status = HttpStatusCode.NotFound;

        bool deleted = await client.DeleteAsync(999);

        Assert.AreEqual(HttpMethod.Delete, api.Method, "It should still ask the API.");
        Assert.IsFalse(deleted);
    }

    [TestMethod]
    public async Task DeleteAsync_409_Throws()
    {
        api.Status = HttpStatusCode.Conflict;

        await Assert.ThrowsExactlyAsync<HttpRequestException>(() => client.DeleteAsync(1));
    }

    [TestMethod]
    public async Task LowStockAsync_Threshold_FewestFirstThenByName()
    {
        api.Json = ThreeProducts;

        List<Product> low = await client.LowStockAsync(10);

        CollectionAssert.AreEqual(new[] { "Aniseed Syrup", "Chang" }, low.Select(p => p.ProductName).ToArray());
    }
}
