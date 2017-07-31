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
            #region sokcet第一个版本
            //SocketServices SocketServices = new SocketServices();
            //Console.WriteLine("Server is ready!");
            //Console.Read();
            #endregion

            #region socket第二个版本
            IOCPServer server = new IOCPServer(8088,10000);
            server.Start();
            Console.WriteLine("socket服务器已启动....");
            //System.Console.ReadLine();
            #endregion

            #region http服务器
            HttpServer.SimpleListenerExample(new[] { "http://127.0.0.1:8080/index/" });
            Console.Read();
            #endregion



        }
    }
}
