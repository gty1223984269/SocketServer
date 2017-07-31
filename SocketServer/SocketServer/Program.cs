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
        {    //version1
            //SocketServices SocketServices = new SocketServices();
            //Console.WriteLine("Server is ready!");
            //Console.Read();
            //verson2
            IOCPServer server = new IOCPServer(8088, 1);
            server.Start();
            Console.WriteLine("服务器已启动....");
            System.Console.ReadLine();  


        }
    }
}
