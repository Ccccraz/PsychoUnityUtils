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
            else if (msg == "over")
            {

                var _ = listener.SendMsgAsync("over");
            }
        }
    }
    private static void PrintData(object sender, DataModel e)
    {
        var msg = Encoding.UTF8.GetString(e.Data);

        Console.WriteLine($"Received msg is: {msg}, the size of msg is: {e.DataSize}");

    }
}