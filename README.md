# Practice: an API Client

Practice code for the lesson **Practice: an API Client** in the C# Full-Stack Bootcamp on [ask-codey.com](https://www.ask-codey.com).

Your products API from *Practice: Northwind Web API* needs a client: a C# class that other programs can use to read and change products without knowing anything about HTTP. You'll write `ProductClient`.

## What's here

- `NorthwindClient/Product.cs`: a product, shaped like the API's JSON. Ready to use.
- `NorthwindClient/ProductClient.cs`: for you to finish. The requirements are in the comment at the top.
- `NorthwindClient/Program.cs`: a small program that uses the client against the running API.
- `NorthwindClient.Tests`: tests that give the client a fake API, so they need nothing running. You don't need to change these.

## What to do

Finish each method of `ProductClient`: `GetAllAsync`, `GetByIdAsync`, `AddAsync`, `UpdateAsync`, `DeleteAsync` and `LowStockAsync`.

Run the tests with **Test > Run All Tests** (or `dotnet test`). Then start your products API from the last practice, check the address at the top of `Program.cs`, and run this program with **Ctrl+F5**.

## Tips

- `GetFromJsonAsync`, `PostAsJsonAsync`, `PutAsJsonAsync` and `ReadFromJsonAsync` come from `using System.Net.Http.Json;`. `HttpStatusCode` comes from `using System.Net;`.
- For methods where 404 is a normal answer, use `GetAsync`, `PutAsJsonAsync` or `DeleteAsync`, check for `HttpStatusCode.NotFound` first, then call `response.EnsureSuccessStatusCode()` to throw for anything else that isn't a success.
- `LowStockAsync` can use your own `GetAllAsync`, then LINQ. `p.UnitsInStock ?? 0` treats a missing stock figure as 0.
