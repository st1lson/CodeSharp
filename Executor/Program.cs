using Core.Docker;
using Core.Docker.Models;
using Core.Docker.Providers;

IContainerPortProvider portProvider = new ContainerPortProvider();

IDockerConfiguration configuration = new DockerConfiguration(
    Image.CreateImage("codesharp.executor:dev"),
    new RandomContainerNameProvider(),
    portProvider,
    new ContainerEndpointProvider(portProvider));

using IDockerContainer dockerContainer = new DockerContainer(configuration);
await dockerContainer.StartAsync();

await dockerContainer.WaitToBeReadyAsync();

var httpClient = new HttpClient();

var res = await httpClient.GetStringAsync($"http://localhost:{configuration.ContainerPortProvider.CurrentPort}/WeatherForecast");
Console.WriteLine(res);