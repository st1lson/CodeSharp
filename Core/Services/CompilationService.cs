using Core.Docker.Providers;
using Core.Services.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Core.Services;

public class CompilationService : ICompilationService
{
    private readonly IContainerEndpointProvider _containerEndpointProvider;

    public CompilationService(IContainerEndpointProvider containerEndpointProvider)
    {
        _containerEndpointProvider = containerEndpointProvider;
    }

    public async Task<CompilationResult> CompileAsync(string code, CancellationToken cancellationToken)
    {
        var compileUrl = _containerEndpointProvider.GetCompileEndpoint();
        using var httpClient = new HttpClient();
        var result = await httpClient.PostAsJsonAsync(compileUrl, code, cancellationToken);
        if (!result.IsSuccessStatusCode)
        {
            return new CompilationResult("asd");
        }

        var json = await result.Content.ReadAsStringAsync(cancellationToken);
        var item = JsonSerializer.Deserialize<CompilationResult>(json);
        return new CompilationResult("asdas");
    }
}
