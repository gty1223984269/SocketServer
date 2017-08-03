using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
           
            IOCPServer server = new IOCPServer(8088,10000);
            server.Start();
            Console.WriteLine("socket服务器已启动....");
            HttpServer.SimpleListenerExample(new[] { "http://127.0.0.1:8080/index/" });
            Console.Read();




        }
    }
}
