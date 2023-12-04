using Core.Docker;
using Core.Docker.Models;
using Core.Docker.Providers;

IContainerPortProvider portProvider = new ContainerPortProvider();
IContainerEndpointProvider endpointProvider = new ContainerEndpointProvider(portProvider);
IContainerHealthCheckProvider healthCheckProvider = new HttpContainerHealthCheckProvider(endpointProvider);

IDockerConfiguration configuration = new DockerConfiguration(
    Image.CreateImage("codesharp.executor:latest"),
    new RandomContainerNameProvider(),
    portProvider,
    endpointProvider,
    healthCheckProvider);

using IDockerContainer dockerContainer = new DockerContainer(configuration);
await dockerContainer.StartAsync();

await dockerContainer.EnsureCreatedAsync();

var httpClient = new HttpClient();

var res = await httpClient.GetStringAsync($"http://localhost:{configuration.ContainerPortProvider.CurrentPort}/WeatherForecast");
Console.WriteLine(res);