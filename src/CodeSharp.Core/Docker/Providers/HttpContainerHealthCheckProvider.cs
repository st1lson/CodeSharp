using Core.Docker.Exceptions;

namespace Core.Docker.Providers;

public class HttpContainerHealthCheckProvider : IContainerHealthCheckProvider
{
    private const int Retries = 5;

    private readonly IContainerEndpointProvider _containerEndpointProvider;

    public HttpContainerHealthCheckProvider(IContainerEndpointProvider endpointProvider)
    {
        _containerEndpointProvider = endpointProvider;
    }

    public async Task EnsureCreatedAsync(CancellationToken cancellationToken)
    {
        var delayTask = Task.Delay(1000, cancellationToken);
        using var httpClient = new HttpClient();
        for (int i = 0; i < Retries; i++)
        {
            try
            {
                var healthCheckEndpoint = _containerEndpointProvider.GetHealthCheckEndpoint();

                var healthCheckResponse = await httpClient.GetAsync(healthCheckEndpoint, cancellationToken);

                if (healthCheckResponse.IsSuccessStatusCode)
                {
                    return;
                }

                await delayTask;
            }
            catch (Exception)
            {
                await delayTask;
            }
        }

        throw new HealthcheckFailedException("Failed to connect to the server");
    }
}
