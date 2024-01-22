using System.Net.Http.Json;
using System.Text.Json;
using CodeSharp.Core.Docker;
using CodeSharp.Core.Docker.Models;
using CodeSharp.Core.Docker.Providers;
using CodeSharp.Core.Executors.Exceptions;
using CodeSharp.Core.Executors.Models.Testing;

namespace CodeSharp.Core.Executors;

public class TestExecutor : TestExecutor<TestingResponse>
{
    public TestExecutor(
        ContainerConfiguration configuration,
        IContainerNameProvider containerNameProvider,
        IContainerPortProvider containerPortProvider,
        IContainerHealthCheckProvider containerHealthCheckProvider,
        IContainerEndpointProvider containerEndpointProvider) : base(configuration, containerNameProvider, containerPortProvider, containerHealthCheckProvider, containerEndpointProvider)
    {
    }
}

public class TestExecutor<TResponse> : CodeExecutor, ITestExecutor<TResponse> where TResponse : TestingResponse
{
    private readonly IContainerEndpointProvider _containerEndpointProvider;
    private readonly ContainerConfiguration _configuration;
    private readonly IContainerNameProvider _containerNameProvider;
    private readonly IContainerPortProvider _containerPortProvider;
    private readonly IContainerHealthCheckProvider _containerHealthCheckProvider;

    public TestExecutor(
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

    public async Task<TResponse> TestAsync(string code, string testsCode, CancellationToken cancellationToken = default)
    {
        using var dockerContainer = new DockerContainer(_configuration, _containerNameProvider, _containerPortProvider, _containerHealthCheckProvider);
        await dockerContainer.StartAsync(cancellationToken);

        await dockerContainer.EnsureCreatedAsync(cancellationToken);
        var testingUrl = _containerEndpointProvider.GetTestingEndpoint();
        using var httpClient = new HttpClient();

        var testingRequest = new TestingRequest
        {
            CodeToTest = code,
            TestsCode = testsCode,
        };
        var result = await httpClient.PostAsJsonAsync(testingUrl, testingRequest, cancellationToken);
        if (!result.IsSuccessStatusCode)
        {
            throw new TestingFailedException();
        }

        var json = await result.Content.ReadAsStringAsync(cancellationToken);
        var item = JsonSerializer.Deserialize<TResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return item!;
    }

    public async Task<TResponse> TestFileAsync(string filePath, string testsCode, CancellationToken cancellationToken = default)
    {
        const string requiredFileExtension = ".cs";

        var fileExtension = Path.GetExtension(filePath);
        if (!fileExtension.Equals(requiredFileExtension))
        {
            throw new ArgumentException("Wrong file extension");
        }

        var code = await ReadCodeFromFileAsync(filePath, cancellationToken);

        return await TestAsync(code, testsCode, cancellationToken);
    }
}
