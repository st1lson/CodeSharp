using System.Net.Http.Json;
using System.Text.Json;
using CodeSharp.Core.Docker;
using CodeSharp.Core.Docker.Models;
using CodeSharp.Core.Docker.Providers;
using CodeSharp.Core.Executors.Exceptions;
using CodeSharp.Core.Executors.Models.Compilation;

namespace CodeSharp.Core.Executors;

public class CompileExecutor : CompileExecutor<CompilationResponse>
{
    public CompileExecutor(
        ContainerConfiguration configuration,
        IContainerNameProvider containerNameProvider,
        IContainerPortProvider containerPortProvider,
        IContainerHealthCheckProvider containerHealthCheckProvider,
        IContainerEndpointProvider containerEndpointProvider) : base(configuration, containerNameProvider, containerPortProvider, containerHealthCheckProvider, containerEndpointProvider)
    {
    }
}

public class CompileExecutor<TResponse> : CodeExecutor, ICompileExecutor<TResponse> where TResponse : CompilationResponse
{
    private readonly IContainerEndpointProvider _containerEndpointProvider;
    private readonly ContainerConfiguration _configuration;
    private readonly IContainerNameProvider _containerNameProvider;
    private readonly IContainerPortProvider _containerPortProvider;
    private readonly IContainerHealthCheckProvider _containerHealthCheckProvider;

    public CompileExecutor(
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

    public async Task<TResponse> CompileAsync(string code, bool run, CancellationToken cancellationToken)
    {
        using var dockerContainer = new DockerContainer(_configuration, _containerNameProvider, _containerPortProvider, _containerHealthCheckProvider);
        await dockerContainer.StartAsync(cancellationToken);

        await dockerContainer.EnsureCreatedAsync(cancellationToken);
        var compileUrl = _containerEndpointProvider.GetCompileEndpoint();
        using var httpClient = new HttpClient();

        var compilationRequest = new CompilationRequest
        {
            Code = code,
            Run = run
        };
        var result = await httpClient.PostAsJsonAsync(compileUrl, compilationRequest, cancellationToken);
        if (!result.IsSuccessStatusCode)
        {
            throw new CompilationFailedException();
        }

        var json = await result.Content.ReadAsStringAsync(cancellationToken);
        var item = JsonSerializer.Deserialize<TResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return item!;
    }

    public async Task<TResponse> CompileFileAsync(string filePath, bool run, CancellationToken cancellationToken = default)
    {
        var code = await ReadCodeFromFileAsync(filePath, cancellationToken);

        return await CompileAsync(code, run, cancellationToken);
    }
}
