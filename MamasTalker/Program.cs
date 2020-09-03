using System;
using System.Threading;

namespace MamasTalker.Server
{
    class Program
    {
        static void Main(string[] args)
        {

            MamasServer server = new MamasServer(8844);
            RunServer(server);
        }
        static void RunServer(MamasServer server)
        {
            server.Run();
        }
    }
}
