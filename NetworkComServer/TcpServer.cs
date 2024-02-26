using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkComServer
{
    internal class TcpServer
    {
        public delegate void MyEventHandler(object sender, (byte[], int) e);
        public event MyEventHandler? DataReceived;
        private IPEndPoint IpEndPoint { get; }

        private Socket? Handler { get; set; }
        private bool IsListening {get ; set;}

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

            while(IsListening){
                var received = await Handler.ReceiveAsync(buffer, SocketFlags.None);
                
                if(received > 0){
                    DataReceived?.Invoke(this, (buffer, received));
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
            } else {
                throw new InvalidOperationException("Socket handler is not initialized.");
            }
        }

        internal async Task SendMsgAsync(string msg)
        {
            var buffer = Encoding.UTF8.GetBytes(msg);
            await SendAsync(buffer);
        }

        internal void Stop(){
            IsListening = false;
        }
    }
}