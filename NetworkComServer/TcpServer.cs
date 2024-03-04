using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace NetworkComServer
{
    internal class TcpServer
    {
        public delegate void MyEventHandler(object sender, DataModel e);
        public event MyEventHandler? DataReceived;
        private IPEndPoint IpEndPoint { get; }

        private Socket? Handler { get; set; }
        private bool IsListening { get; set; }

        internal TcpServer(string hostName = "127.0.0.1", int port = 8888)
        {
            IpEndPoint = new IPEndPoint(IPAddress.Parse(hostName), port);
        }

        internal async Task ListenAsync()
        {
            using Socket listener = new(IpEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            var buffer = new byte[1024];

            listener.Bind(IpEndPoint);
            listener.Listen(100);

            Handler = await listener.AcceptAsync();

            IsListening = true;

            var heart = HeartBeat();
            while (IsListening)
            {
                var received = await Handler.ReceiveAsync(buffer, SocketFlags.None);

                if (received > 0)
                {
                    DataReceived?.Invoke(this, new DataModel() { DataSize = received, Data = buffer });
                }
            }

            Handler.Shutdown(SocketShutdown.Both);
            Handler.Close();
        }

        internal async Task SendAsync(byte[] buffer)
        {
            if (Handler != null)
            {
                await Handler.SendAsync(buffer, SocketFlags.None);
            }
            else
            {
                throw new InvalidOperationException("Socket handler is not initialized.");
            }
        }

        internal async Task SendMsgAsync(string msg)
        {
            await SendAsync(Encoding.UTF8.GetBytes(msg));
        }

        internal void Stop()
        {
            IsListening = false;
        }

        internal async Task HeartBeat()
        {
            while (IsListening)
            {
                try
                {
                    await SendAsync(Encoding.UTF8.GetBytes("heartbeat"));
                }
                catch (System.Exception e)
                {
                    if (e is SocketException socketException)
                    {
                        if (socketException.SocketErrorCode == SocketError.ConnectionAborted)
                        {
                            IsListening = false;
                            Console.WriteLine("SocketException 10053 (ConnectionAborted) occurred. Closeing the conection...");
                        }
                        else
                        {
                            Console.WriteLine($"SocketException occurred: {socketException.SocketErrorCode}");
                        }
                    }
                    else
                    {
                        Console.WriteLine(e);
                    }
                }

                await Task.Delay(5000);
            }
        }
    }

    internal struct DataModel()
    {
        internal int DataSize { get; set; }
        internal byte[] Data { get; set; }
    }
}