using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkComClient;

public static class Program
{

    private static async Task Main()
    {
        var ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888);

        using Socket client = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        await client.ConnectAsync(ipEndPoint);

        while (true)
        {
            const string message = "Hi friends 👋!<|EOM|>";
            var messageBytes = Encoding.UTF8.GetBytes(message);

            _ = await client.SendAsync(messageBytes, SocketFlags.None);
            
            Console.WriteLine($"Socket client sent message: \"{message}\"");

            var buffer = new byte[1024];
            var received = await client.ReceiveAsync(buffer, SocketFlags.None);
            var response = Encoding.UTF8.GetString(buffer, 0, received);

            if(response == "over"){
                break;
            }
        }
        
        client.Shutdown(SocketShutdown.Both);
    }
}