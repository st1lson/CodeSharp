using System.Net.Http.Json;
using System.Text.Json;
using CodeSharp.Core.Docker;
using CodeSharp.Core.Docker.Models;
using CodeSharp.Core.Docker.Providers;
using CodeSharp.Core.Services.Models.Testing;

namespace CodeSharp.Core.Services;

public class TestingService : ITestingService
{
    private readonly IContainerEndpointProvider _containerEndpointProvider;
    private readonly ContainerConfiguration _configuration;
    private readonly IContainerNameProvider _containerNameProvider;
    private readonly IContainerPortProvider _containerPortProvider;
    private readonly IContainerHealthCheckProvider _containerHealthCheckProvider;

    public TestingService(
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

    public async Task<TestingResponse> TestAsync(string code, string testsCode, CancellationToken cancellationToken = default)
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
            throw new Exception("Testing failed");
        }

        var json = await result.Content.ReadAsStringAsync(cancellationToken);
        var item = JsonSerializer.Deserialize<TestingResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return item!;
    }

    public async Task<TestingResponse> TestFileAsync(string filePath, string testsCode, CancellationToken cancellationToken = default)
    {
        const string requiredFileExtension = ".cs";

        var fileExtension = Path.GetExtension(filePath);
        if (!fileExtension.Equals(requiredFileExtension))
        {
            throw new Exception("Wrong file extension");
        }

        var code = await File.ReadAllTextAsync(filePath, cancellationToken);

        return await TestAsync(code, testsCode, cancellationToken);
    }
}
