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
                NetworkStream nwStream = client.GetStream();
                //ToDo: Figure the best way to send and recieve repeatedly (not to stop after one send);
                //ToDo: Test if the server can send data to the client, without recieving data from client first
                while (true)
                {

                    //string textToSend = "a";
                    //byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

                    ////---send the text---
                    //Console.WriteLine("Sending : " + textToSend);
                    //nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                    //---read  the text---
                    IFormatter formatter = new BinaryFormatter();
                    while (true)
                    {
                        MessageData data = (MessageData)formatter.Deserialize(nwStream);
                        data.bitmap.Save(@"C:\Users\thelittlecitizen13\Desktop\Images\client\" + Guid.NewGuid() + ".jpg", ImageFormat.Jpeg);
                        Console.WriteLine("Image Recieved");
                    }


                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                //ToDo: Figure out a way to terminate client
                client.Close();
            }
        }
    }
}
