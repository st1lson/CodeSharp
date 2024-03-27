using CodeSharp.Core.Docker.Exceptions;

namespace CodeSharp.Core.Docker.Providers
{
    public class HttpContainerHealthCheckProvider : IContainerHealthCheckProvider
    {
        private const int Retries = 5;
        private const int DelayMs = 1000;

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

            var healthCheckEndpoint = _containerEndpointProvider.GetHealthCheckEndpoint();
            for (int i = 0; i < Retries; i++)
            {
                try
                {
                    var healthCheckResponse = await httpClient.GetAsync(healthCheckEndpoint, cancellationToken);

                    if (healthCheckResponse.IsSuccessStatusCode)
                    {
                        return;
                    }

                    await Task.Delay(DelayMs, cancellationToken);
                }
                catch (Exception)
                {
                    await Task.Delay(DelayMs, cancellationToken);
                }
            }

            throw new HealthCheckFailedException("Failed to connect to the server");
        }
    }
}
