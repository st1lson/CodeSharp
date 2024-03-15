using CodeSharp.Core.Executors.Exceptions;
using CodeSharp.Core.Executors.Strategies;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Xunit;

namespace CodeSharp.Core.Tests.Executors.Strategies;

public class HttpCommunicationStrategyTests
{
    private readonly HttpClient _httpClient;
    private readonly HttpCommunicationStrategy _strategy;

    public HttpCommunicationStrategyTests()
    {
        _httpClient = Substitute.For<HttpClient>();
        _strategy = new HttpCommunicationStrategy(_httpClient);
    }

    [Fact]
    public async Task SendRequestAsync_WhenSuccess_ReturnsDeserializedResponse()
    {
        // Arrange
        var request = new { Data = "TestData" };
        var response = new { Result = "TestResult" };
        var endpoint = "http://test.com/api";
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(response), System.Text.Encoding.UTF8, "application/json")
        };
        _httpClient.PostAsJsonAsync(endpoint, request, Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(Task.FromResult(httpResponseMessage));

        // Act
        var result = await _strategy.SendRequestAsync<object, dynamic>(endpoint, request, CancellationToken.None);

        // Assert
        var actualResult = result.GetProperty("Result").GetString();
        Assert.Equal(response.Result, actualResult);
    }

    [Fact]
    public async Task SendRequestAsync_WhenFailure_ThrowsCompilationFailedException()
    {
        // Arrange
        var request = new { Data = "TestData" };
        var response = new { Message = "Failed to make the request" };
        var endpoint = "http://test.com/api";
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, "application/json")
        };
        _httpClient.PostAsJsonAsync(endpoint, request, Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(Task.FromResult(httpResponseMessage));

        // Act & Assert
        await Assert.ThrowsAsync<CompilationFailedException>(() =>
            _strategy.SendRequestAsync<object, dynamic>(endpoint, request, CancellationToken.None));
    }
}
