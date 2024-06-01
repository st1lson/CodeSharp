using CodeSharp.Core.Docker;
using CodeSharp.Core.Docker.Factories;
using CodeSharp.Core.Docker.Models;
using CodeSharp.Core.Docker.Providers;
using CodeSharp.Core.Executors.Models.Testing;
using CodeSharp.Core.Executors.Strategies;

namespace CodeSharp.Core.Executors;

public class TestExecutor : TestExecutor<TestingResponse>
{
    public TestExecutor(
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

public class TestExecutor<TResponse> : CodeExecutor, ITestExecutor<TResponse>
{
    private readonly IContainerEndpointProvider _containerEndpointProvider;
    private readonly ContainerConfiguration _configuration;
    private readonly IContainerNameProvider _containerNameProvider;
    private readonly IContainerPortProvider _containerPortProvider;
    private readonly IContainerHealthCheckProvider _containerHealthCheckProvider;
    private readonly IDockerClientFactory _dockerClientFactory;
    private readonly ICommunicationStrategy _communicationStrategy;

    public TestExecutor(
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

    public async Task<TResponse> TestAsync(string code, string testsCode, TestingOptions? options = default, CancellationToken cancellationToken = default)
    {
        options ??= TestingOptions.Default;

        using var dockerContainer = new DockerContainer(_configuration, _containerNameProvider, _containerPortProvider, _containerHealthCheckProvider, _dockerClientFactory);
        await dockerContainer.StartAsync(cancellationToken);

        await dockerContainer.EnsureCreatedAsync(cancellationToken);
        var testingUrl = _containerEndpointProvider.GetTestingEndpoint();

        var testingRequest = new TestingRequest
        {
            CodeToTest = code,
            TestsCode = testsCode,
            Options = options,
        };

        return await _communicationStrategy.SendRequestAsync<TestingRequest, TResponse>(testingUrl, testingRequest, cancellationToken);
    }

    public async Task<TResponse> TestFileAsync(string filePath, string testsCode, TestingOptions? options = default, CancellationToken cancellationToken = default)
    {
        const string requiredFileExtension = ".cs";

        var fileExtension = Path.GetExtension(filePath);
        if (!fileExtension.Equals(requiredFileExtension))
        {
            throw new ArgumentException("Wrong file extension");
        }

        var code = await ReadCodeFromFileAsync(filePath, cancellationToken);

        return await TestAsync(code, testsCode, options, cancellationToken);
    }
}
