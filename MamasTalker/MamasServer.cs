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

                    Console.WriteLine("Connected to: {0}:{1} ",
                        ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(),
                        ((IPEndPoint)client.Client.RemoteEndPoint).Port.ToString());
                    object obj = new object();
                    ThreadPool.QueueUserWorkItem(obj =>
                    {
                        NetworkStream nwStream = client.GetStream();
                        MessageData data = new MessageData(takeScreenShot());
                        IFormatter formatter = new BinaryFormatter();
                        while (true)
                        {
                            formatter.Serialize(nwStream, data);
                            Thread.Sleep(1000);
                            data.bitmap = takeScreenShot();
                            Thread.Sleep(10000);
                        }
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
                Console.WriteLine("Terminating...");
                _server.Stop();
            }

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
            bitmap.Save(@"C:\serverPrintScreen" + Guid.NewGuid() + ".jpg", ImageFormat.Jpeg);
            return bitmap;
        }




    }
}


