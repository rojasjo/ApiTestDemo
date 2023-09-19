namespace ApiTestDemo.IntegrationTests.TestSuite;

public static class HttpClientFactory
{
    public static HttpClient Create(WebAppFactory factory)
    {
        var httpClient = factory.CreateClient();
        httpClient.BaseAddress = new Uri("https://localhost/");

        return httpClient;
    }
}