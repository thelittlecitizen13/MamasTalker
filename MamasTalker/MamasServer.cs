using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

            try
            {
                // ToDo: Figure a way to accept client connections async at the best way.
                while (true)
                {
                    //---incoming client connected---
                    TcpClient client = _server.AcceptTcpClient();

                    Console.WriteLine("Connected to: {0}:{1} ",
                        ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(),
                        ((IPEndPoint)client.Client.RemoteEndPoint).Port.ToString());


                    object obj = new object();
                    ThreadPool.QueueUserWorkItem(obj =>
                    {
                        //---get the incoming data through a network stream---
                        NetworkStream nwStream = client.GetStream();
                        byte[] buffer = new byte[client.ReceiveBufferSize];

                        //---read incoming stream---
                        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                        //---convert the data received into a string---
                        string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        Console.WriteLine("Received : " + dataReceived);

                        //---write back the text to the client---
                        Console.WriteLine("Sending back : " + dataReceived);
                        nwStream.Write(buffer, 0, bytesRead);
                        client.Close();
                    }, null);
                }
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


