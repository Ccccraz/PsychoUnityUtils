using System.Text;

namespace NetworkComServer;

public static class Program
{
    private static void Main()
    {
        var listener = new TcpServer();

        var listenAsync = listener.ListenAsync();

        listener.DataReceived += PrintData;

        while (true)
        {
            var msg = Console.ReadLine();

            if (msg == "stop")
            {
                break;
            }
        }
    }
    private static void PrintData(object sender, (byte[], int) e)
    {
        var msg = Encoding.UTF8.GetString(e.Item1);
        
        Console.WriteLine($"Received msg is: {msg}, the size of msg is: {e.Item2}");
    }
}