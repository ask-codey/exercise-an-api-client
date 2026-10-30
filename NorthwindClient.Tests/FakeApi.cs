using System.Net;
using System.Text;

namespace NorthwindClient.Tests;

// Stands in for the products API: it records each request, and answers with the status and JSON it was given.
public class FakeApi : HttpMessageHandler
{
    public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
    public string Json { get; set; } = "";

    public HttpMethod? Method { get; private set; }
    public string? PathAndQuery { get; private set; }
    public string? Body { get; private set; }

    // An HttpClient that sends its requests here.
    public HttpClient CreateClient() => new HttpClient(this) { BaseAddress = new Uri("http://api.test/") };

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Method = request.Method;
        PathAndQuery = request.RequestUri?.PathAndQuery;
        Body = request.Content == null ? null : await request.Content.ReadAsStringAsync(cancellationToken);

        HttpResponseMessage response = new HttpResponseMessage(Status);
        if (Json != "")
        {
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
        }
        return response;
    }
}
