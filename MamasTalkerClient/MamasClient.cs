using System;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using Common;

namespace MamasTalkerClient
{
    public class MamasClient
    {
        private IPAddress _serverAddress;
        private int _port;

        public MamasClient(string address, int port)
        {
            _serverAddress = IPAddress.Parse(address);
            _port = port;
        }
        public void Run()
        {
            TcpClient client = new TcpClient(_serverAddress.ToString(), _port);
            try
            {
                using (NetworkStream nwStream = client.GetStream())
                {
                    while (true)
                    {
                        receiveImage(nwStream);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                client.Close();
            }
        }
        private void receiveImage(NetworkStream nwStream)
        {
            IFormatter formatter = new BinaryFormatter();
            MessageData data = (MessageData)formatter.Deserialize(nwStream);
            // Important - the path have to be valid to be able to save the image
            data.bitmap.Save(@"C:\Users\thelittlecitizen13\Desktop\Images\client\" + Guid.NewGuid() + ".jpg", ImageFormat.Jpeg);
            Console.WriteLine("Image Recieved");
        }
    }
}
