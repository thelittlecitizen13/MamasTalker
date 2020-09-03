using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MamasTalker.Server
{
    public class MamasServer
    {
        private IPAddress _localAddress;
        private TcpListener _server;
        
        public MamasServer(int port)
        {
            _localAddress = IPAddress.Parse("127.0.0.1");
            _server = new TcpListener(_localAddress, port);
        }
        public void Run()
        {
            
            _server.Start();
            Console.WriteLine($"Listening at {_server.LocalEndpoint}. Waiting for connections.");
            // Data Buffers
            Byte[] bytes = new byte[256];
            string data;
            while(true)
            {
                try
                {
                    var client = _server.AcceptTcpClient();
                    Console.WriteLine($"{client.Client.RemoteEndPoint} connected!");
                    data = null;
                    NetworkStream stream = client.GetStream();
                    int i;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);
                    }

                    // Shutdown and end connection
                    client.Close();
                }

                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
                finally
                {
                    // Stop listening for new clients.
                    Console.WriteLine("Terminating...");
                    _server.Stop();
                }
            }
        }
    }
}
