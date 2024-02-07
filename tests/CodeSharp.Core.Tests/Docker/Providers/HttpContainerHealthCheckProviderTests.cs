using CodeSharp.Core.Docker.Exceptions;
using CodeSharp.Core.Docker.Providers;
using NSubstitute;
using System.Net;
using Xunit;

namespace CodeSharp.Core.Tests.Docker.Providers;

public class HttpContainerHealthCheckProviderTests
{
    private readonly IContainerEndpointProvider _endpointProvider;
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpContainerHealthCheckProviderTests()
    {
        _endpointProvider = Substitute.For<IContainerEndpointProvider>();
        _endpointProvider.GetHealthCheckEndpoint().Returns("http://localhost/healthz");
        _httpClientFactory = Substitute.For<IHttpClientFactory>();
        var messageHandler = new MockHttpMessageHandler();

        var client = new HttpClient(messageHandler);
        _httpClientFactory.CreateClient().Returns(client);
    }

    [Fact]
    public async Task EnsureCreatedAsync_WhenSuccessful_Returns()
    {
        // Arrange
        var provider = new HttpContainerHealthCheckProvider(_endpointProvider, _httpClientFactory);
        MockHttpMessageHandler.SetResponse(HttpStatusCode.OK);

        // Act & Assert
        await provider.EnsureCreatedAsync(CancellationToken.None);
    }

    [Fact]
    public async Task EnsureCreatedAsync_WhenFailed_ThrowsHealthCheckFailedException()
    {
        // Arrange
        var provider = new HttpContainerHealthCheckProvider(_endpointProvider, _httpClientFactory);
        MockHttpMessageHandler.SetResponse(HttpStatusCode.InternalServerError);

        // Act & Assert
        await Assert.ThrowsAsync<HealthCheckFailedException>(() => provider.EnsureCreatedAsync(CancellationToken.None));
    }

    private class MockHttpMessageHandler : HttpMessageHandler
    {
        private static HttpResponseMessage? _responseMessage;

        public static void SetResponse(HttpStatusCode statusCode)
        {
            _responseMessage = new HttpResponseMessage(statusCode);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_responseMessage == null)
            {
                throw new InvalidOperationException("Response message not set.");
            }

            return Task.FromResult(_responseMessage);
        }
    }

}
