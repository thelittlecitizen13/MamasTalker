using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;

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
                while (true)
                {
                    TcpClient client = _server.AcceptTcpClient();
                    
                    printClientConnection(client);

                    object obj = new object();
                    ThreadPool.QueueUserWorkItem(obj =>
                    {
                        NetworkStream nwStream = client.GetStream();
                        IFormatter formatter = new BinaryFormatter();
                        while (true)
                        {
                            MessageData data = new MessageData(takeScreenShot());
                            formatter.Serialize(nwStream, data);
                            Thread.Sleep(10000);
                        }

                    }, null);

                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                Console.WriteLine("Terminating...");
                _server.Stop();
            }

        }
        private void printClientConnection(TcpClient client)
        {
            Console.WriteLine("Connected to: {0}:{1} ",
                           ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(),
                           ((IPEndPoint)client.Client.RemoteEndPoint).Port.ToString());
        }
        private Bitmap takeScreenShot()
        {
            var bitmap = new Bitmap(1920, 1080);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(0, 0, 0, 0,
                bitmap.Size, CopyPixelOperation.SourceCopy);
            }
            // Important - the path have to be valid to be able to save the image
            bitmap.Save(@"C:\images\serverPrintScreen" + Guid.NewGuid() + ".jpg", ImageFormat.Jpeg);
            return bitmap;
        }




    }
}


