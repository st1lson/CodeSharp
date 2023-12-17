using System.Net;
using System.Net.Sockets;
using CodeSharp.Core.Docker.Exceptions;

namespace CodeSharp.Core.Docker.Providers;

public class ContainerPortProvider : IContainerPortProvider
{
    private static readonly HashSet<int> Ports = new();
    
    public int CurrentPort { get; private set; }

    public void AcquirePort()
    {
        var freePort= FindFreePort();
        if (freePort == 0)
        {
            throw new DockerContainerException("All ports are mapped");
        }
        
        lock (Ports)
        {
            Ports.Add(freePort);   
        }

        CurrentPort = freePort;
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
            if (Ports.TryGetValue(port, out _))
            {
                return default;
            }
        }
        
        listener.Stop();
        return port;
    }
}