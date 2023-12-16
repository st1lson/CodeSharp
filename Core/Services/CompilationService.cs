using Core.Docker;
using Core.Docker.Models;
using Core.Docker.Providers;
using Core.Services.Models;
using System.Net.Http.Json;
using System.Text.Json;
using Core.Services.Models.Compilation;

namespace Core.Services;

public class CompilationService : ICompilationService
{
    private readonly IContainerEndpointProvider _containerEndpointProvider;
    private readonly ContainerConfiguration _configuration;
    private readonly IContainerNameProvider _containerNameProvider;
    private readonly IContainerPortProvider _containerPortProvider;
    private readonly IContainerHealthCheckProvider _containerHealthCheckProvider;

    public CompilationService(
        ContainerConfiguration configuration,
        IContainerNameProvider containerNameProvider,
        IContainerPortProvider containerPortProvider,
        IContainerHealthCheckProvider containerHealthCheckProvider,
        IContainerEndpointProvider containerEndpointProvider)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _containerNameProvider = containerNameProvider ?? throw new ArgumentNullException(nameof(containerNameProvider));
        _containerPortProvider = containerPortProvider ?? throw new ArgumentNullException(nameof(containerPortProvider));
        _containerHealthCheckProvider = containerHealthCheckProvider ?? throw new ArgumentNullException(nameof(containerHealthCheckProvider));
        _containerEndpointProvider = containerEndpointProvider ?? throw new ArgumentNullException(nameof(containerEndpointProvider));
    }

    public async Task<CompilationResponse> CompileAsync(string code, CancellationToken cancellationToken)
    {
        using var dockerContainer = new DockerContainer(_configuration, _containerNameProvider, _containerPortProvider, _containerHealthCheckProvider);
        await dockerContainer.StartAsync(cancellationToken);

        await dockerContainer.EnsureCreatedAsync(cancellationToken);
        var compileUrl = _containerEndpointProvider.GetCompileEndpoint();
        using var httpClient = new HttpClient();

        var compilationRequest = new CompilationRequest
        {
            Code = code
        };
        var result = await httpClient.PostAsJsonAsync(compileUrl, compilationRequest, cancellationToken);
        if (!result.IsSuccessStatusCode)
        {
            throw new Exception("Compilation failed");
        }

        var json = await result.Content.ReadAsStringAsync(cancellationToken);
        var item = JsonSerializer.Deserialize<CompilationResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return item!;
    }

    public async Task<CompilationResponse> CompileFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        const string requiredFileExtension = ".cs";
        
        var fileExtension = Path.GetExtension(filePath);
        if (!fileExtension.Equals(requiredFileExtension))
        {
            throw new Exception("Wrong file extension");
        }
        
        var code = await File.ReadAllTextAsync(filePath, cancellationToken);

        return await CompileAsync(code, cancellationToken);
    }
}
