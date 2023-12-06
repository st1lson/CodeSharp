using Core.Docker;
using Core.Docker.Models;
using Core.Docker.Providers;
using Core.Services;

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

ICompilationService compilationService = new CompilationService(endpointProvider);
var compilationResult = await compilationService.CompileAsync(@"
using System;

class Program
{
    static void Main()
    {
        Console.WriteLine(""Hello, World!"");
    }
}
");

Console.WriteLine(compilationResult);