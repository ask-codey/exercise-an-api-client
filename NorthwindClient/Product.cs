namespace NorthwindClient;

// A product, shaped like the products API's JSON. It's finished.
public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public int? CategoryId { get; set; }
    public decimal? UnitPrice { get; set; }
    public short? UnitsInStock { get; set; }
    public bool Discontinued { get; set; }
}
