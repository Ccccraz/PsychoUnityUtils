using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkComServer;

public static class Program
{
    private static async Task Main()
    {
        var ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888);

        using Socket listener = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        
        listener.Bind(ipEndPoint);
        listener.Listen(100);

        var handler = await listener.AcceptAsync();

        while (true)
        {
            var buffer = new byte[1024];
            var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
            var response = Encoding.UTF8.GetString(buffer, 0, received);

            const string eom = "<|EOM|>";
            if (response.IndexOf(eom) > -1)
            {
                Console.WriteLine($"Socket server received message: \"{response.Replace(eom, "")}\"");

                var ackMessage = "<|ACK|>";
                var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
                await handler.SendAsync(echoBytes, 0);
                
                Console.WriteLine($"Socket server sent acknowledgment: \"{ackMessage}\"");
                
                break;
            }
        }
    }
}