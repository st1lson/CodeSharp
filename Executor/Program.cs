using Core.Docker;
using Core.Docker.Models;
using Core.Docker.Providers;

IContainerPortProvider portProvider = new ContainerPortProvider();
IContainerEndpointProvider endpointProvider = new ContainerEndpointProvider(portProvider);
IContainerHealthCheckProvider healthCheckProvider = new HttpContainerHealthCheckProvider(endpointProvider);

ContainerConfiguration configuration = new ContainerConfiguration
{
    Image = Image.CreateImage("codesharp.executor:latest")
};

using IDockerContainer dockerContainer = new DockerContainer(configuration,
    new RandomContainerNameProvider(),
    portProvider,
    healthCheckProvider);
await dockerContainer.StartAsync();

await dockerContainer.EnsureCreatedAsync();

var httpClient = new HttpClient();

var res = await httpClient.GetStringAsync($"http://localhost:{portProvider.CurrentPort}/WeatherForecast");
Console.WriteLine(res);