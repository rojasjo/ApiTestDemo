using System.Text.Json;

namespace ApiTestDemo.IntegrationTests.TestSuite;

public static class HttpResponseDeserializer
{
    public static async Task<T?> DeserializeAsync<T>(this HttpResponseMessage httpResponseMessage)
    {
        httpResponseMessage.EnsureSuccessStatusCode();
        
        var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
        
        return JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}