using CodeSharp.Core.Docker;
using CodeSharp.Core.Docker.Factories;
using CodeSharp.Core.Docker.Models;
using CodeSharp.Core.Docker.Providers;
using CodeSharp.Core.Executors.Models.Compilation;
using CodeSharp.Core.Executors.Strategies;

namespace CodeSharp.Core.Executors;

public class CompileExecutor : CompileExecutor<CompilationResponse>
{
    public CompileExecutor(
        ContainerConfiguration configuration,
        IContainerNameProvider containerNameProvider,
        IContainerPortProvider containerPortProvider,
        IContainerHealthCheckProvider containerHealthCheckProvider,
        IContainerEndpointProvider containerEndpointProvider,
        IDockerClientFactory dockerClientFactory,
        ICommunicationStrategy communicationStrategy) : base(configuration, containerNameProvider, containerPortProvider, containerHealthCheckProvider, containerEndpointProvider, dockerClientFactory, communicationStrategy)
    {
    }
}

public class CompileExecutor<TResponse> : CodeExecutor, ICompileExecutor<TResponse>
{
    private readonly IContainerEndpointProvider _containerEndpointProvider;
    private readonly ContainerConfiguration _configuration;
    private readonly IContainerNameProvider _containerNameProvider;
    private readonly IContainerPortProvider _containerPortProvider;
    private readonly IContainerHealthCheckProvider _containerHealthCheckProvider;
    private readonly IDockerClientFactory _dockerClientFactory;
    private readonly ICommunicationStrategy _communicationStrategy;

    public CompileExecutor(
        ContainerConfiguration configuration,
        IContainerNameProvider containerNameProvider,
        IContainerPortProvider containerPortProvider,
        IContainerHealthCheckProvider containerHealthCheckProvider,
        IContainerEndpointProvider containerEndpointProvider,
        IDockerClientFactory dockerClientFactory,
        ICommunicationStrategy communicationStrategy)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _containerNameProvider = containerNameProvider ?? throw new ArgumentNullException(nameof(containerNameProvider));
        _containerPortProvider = containerPortProvider ?? throw new ArgumentNullException(nameof(containerPortProvider));
        _containerHealthCheckProvider = containerHealthCheckProvider ?? throw new ArgumentNullException(nameof(containerHealthCheckProvider));
        _containerEndpointProvider = containerEndpointProvider ?? throw new ArgumentNullException(nameof(containerEndpointProvider));
        _dockerClientFactory = dockerClientFactory ?? throw new ArgumentNullException(nameof(dockerClientFactory));
        _communicationStrategy = communicationStrategy ?? throw new ArgumentNullException(nameof(communicationStrategy));
    }

    public async Task<TResponse> CompileAsync(string code, bool run, CancellationToken cancellationToken)
    {
        using var dockerContainer = new DockerContainer(_configuration, _containerNameProvider, _containerPortProvider, _containerHealthCheckProvider, _dockerClientFactory);
        await dockerContainer.StartAsync(cancellationToken);

        await dockerContainer.EnsureCreatedAsync(cancellationToken);
        var compileUrl = _containerEndpointProvider.GetCompileEndpoint();

        var compilationRequest = new CompilationRequest
        {
            Code = code,
            Run = run
        };

        return await _communicationStrategy.SendRequestAsync<CompilationRequest, TResponse>(compileUrl, compilationRequest, cancellationToken);
    }

    public async Task<TResponse> CompileFileAsync(string filePath, bool run, CancellationToken cancellationToken = default)
    {
        var code = await ReadCodeFromFileAsync(filePath, cancellationToken);

        return await CompileAsync(code, run, cancellationToken);
    }
}
