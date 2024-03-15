using CodeSharp.Core.Executors.Exceptions;
using System.Net.Http.Json;
using System.Text.Json;

namespace CodeSharp.Core.Executors.Strategies;

public class HttpCommunicationStrategy : ICommunicationStrategy
{
    private readonly HttpClient _httpClient;

    public HttpCommunicationStrategy(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TResponse> SendRequestAsync<TRequest, TResponse>(string endpoint, TRequest request, CancellationToken cancellationToken)
    {
        var httpResponse = await _httpClient.PostAsJsonAsync(endpoint, request, cancellationToken);
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new CompilationFailedException();
        }

        var json = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<TResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }
}
