using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using System.Reflection;
namespace SocketServer
{
    public class SocketServices
    {
     public    static byte[] buffer = new byte[1024];
        public static Dictionary<string, Socket> socketClient = new Dictionary<string, Socket>();
        private Socket socket;
        public SocketServices()
        {
            //创建一个新的Socket,这里我们使用最常用的基于TCP的Stream Socket（流式套接字）
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //将该socket绑定到主机上面的某个端口
            //方法参考：http://msdn.microsoft.com/zh-cn/library/system.net.sockets.socket.bind.aspx
            socket.Bind(new IPEndPoint(IPAddress.Any, 4530));

            //启动监听，并且设置一个最大的队列长度
            //方法参考：http://msdn.microsoft.com/zh-cn/library/system.net.sockets.socket.listen(v=VS.100).aspx
            socket.Listen(4);
            socket.BeginAccept(new AsyncCallback(ClientAccepted), socket);
        }

        public static void ClientAccepted(IAsyncResult ar)
        {

            var socket = ar.AsyncState as Socket;
            //这就是客户端的Socket实例，我们后续可以将其保存起来
            var client = socket.EndAccept(ar);
            //接受客户端的消息
            client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), client);
            //给客户端发送一个欢迎消息
            client.Send(Encoding.UTF8.GetBytes("5555"));
            //准备接受下一个客户端请求
            //addClient(DateTime.Now.ToString(), client);
            socket.BeginAccept(new AsyncCallback(ClientAccepted), socket);
        }
       
        public static void ReceiveMessage(IAsyncResult ar)
        {
            try
            {
                Assembly ass = Assembly.LoadFile(@"F:\github\project\SocketServer\SocketServer\SocketServer\SocketServer\bin\Debug\SocketServer.exe");//我们要调用的dll或exe文件路径 
                var action = "";
                var StrArray ="";
                var socket = ar.AsyncState as Socket;
                //方法参考：http://msdn.microsoft.com/zh-cn/library/system.net.sockets.socket.endreceive.aspx
                var length = socket.EndReceive(ar);
                //读取出来消息内容
                var message = Encoding.Default.GetString(buffer, 0, length);
                var arrayStr=message.Split(':');
                action = arrayStr[0];
                StrArray = arrayStr[1];
               var mStr=StrArray.Split('&');
                Type tp = ass.GetType("SocketServer.CommonStart");  //获取类名，必须 命名空间+类名  
                Object obj = Activator.CreateInstance(tp);  //建立实例
                MethodInfo meth = tp.GetMethod(action);  //获取要调用的方法   
                switch (action)
                {
                    case"Registe": meth.Invoke(obj, new Object[] { mStr[0], socket }); break;
                    case "Send":   meth.Invoke(obj, new Object[] { mStr[0],mStr[1], mStr[2] }); break;
                    case "SendAll": meth.Invoke(obj, new Object[] {"", mStr[0]}); break;
                }
                  
                Console.WriteLine(message);

                //接收下一个消息(因为这是一个递归的调用，所以这样就可以一直接收消息了）
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), socket);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
