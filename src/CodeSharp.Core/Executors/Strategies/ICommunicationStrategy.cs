namespace CodeSharp.Core.Executors.Strategies;

public interface ICommunicationStrategy
{
    Task<TResponse> SendRequestAsync<TRequest, TResponse>(string endpoint, TRequest request, CancellationToken cancellationToken);
}
