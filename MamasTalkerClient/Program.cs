using System;

namespace MamasTalkerClient
{
    class Program
    {
        static void Main(string[] args)
        {
            MamasClient client = new MamasClient("127.0.0.1", 8844);
            client.Run();
        }
    }
}
