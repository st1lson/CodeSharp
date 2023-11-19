using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace Core.Docker.Providers;

public class DockerContainerPortProvider : IDockerContainerPortProvider
{
    private static readonly HashSet<string> Ports = new();
    
    public string CurrentPort { get; }
    
    public DockerContainerPortProvider()
    {
        var freePort= FindFreePort();
        if (freePort == 0)
        {
            throw new Exception("All ports are mapped");
        }

        var currentPort = freePort.ToString();
        lock (Ports)
        {
            Ports.Add(currentPort);   
        }

        CurrentPort = currentPort;
    }

    public void ReleasePort()
    {
        lock (Ports)
        {
            Ports.Remove(CurrentPort);
        }
    }

    private static int FindFreePort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;

        lock (Ports)
        {
            if (Ports.TryGetValue(port.ToString(), out _))
            {
                return default;
            }
        }
        
        listener.Stop();
        return port;
    }
}