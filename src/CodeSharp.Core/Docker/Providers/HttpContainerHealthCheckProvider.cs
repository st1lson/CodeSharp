using CodeSharp.Core.Docker.Exceptions;

namespace CodeSharp.Core.Docker.Providers
{
    public class HttpContainerHealthCheckProvider : IContainerHealthCheckProvider
    {
        private const int Retries = 5;

        private readonly IContainerEndpointProvider _containerEndpointProvider;
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpContainerHealthCheckProvider(IContainerEndpointProvider endpointProvider, IHttpClientFactory httpClientFactory)
        {
            _containerEndpointProvider = endpointProvider;
            _httpClientFactory = httpClientFactory;
        }

        public async Task EnsureCreatedAsync(CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var delayTask = Task.Delay(1000, cancellationToken);
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

            throw new HealthCheckFailedException("Failed to connect to the server");
        }
    }
}
