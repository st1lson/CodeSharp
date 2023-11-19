using Core.Docker;
using Core.Docker.Providers;
using Core.Models;

IDockerConfiguration configuration = new DockerConfiguration(
    Image.CreateImage("codesharp.executor:dev"),
    new RandomDockerContainerNameProvider());

using (IDockerContainer dockerContainer = new DockerContainer(configuration))
{
    try
    {
        await dockerContainer.StartAsync();

        await Task.Delay(5000);
        
        var httpClient = new HttpClient();

        var res = await httpClient.GetStringAsync("http://localhost:8080/WeatherForecast");
        Console.WriteLine(res);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }

}