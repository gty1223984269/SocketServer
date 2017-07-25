using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;
namespace SocketServer
{
    public class SocketServices
    {
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
            socketClient.Add(DateTime.Now.ToString(), client);
            //给客户端发送一个欢迎消息
            client.Send(Encoding.Unicode.GetBytes("Hi there, I accept you request at " + DateTime.Now.ToString()));
            //准备接受下一个客户端请求
            //addClient(DateTime.Now.ToString(), client);
            socket.BeginAccept(new AsyncCallback(ClientAccepted), socket);
        }
        public static void ReceiveMessage(IAsyncResult ar)
        {
            var buffer = new byte[1024];
            try
            {
                var socket = ar.AsyncState as Socket;

                //方法参考：http://msdn.microsoft.com/zh-cn/library/system.net.sockets.socket.endreceive.aspx
                var length = socket.EndReceive(ar);
                //读取出来消息内容
                var message = Encoding.Unicode.GetString(buffer, 0, length);          //显示消息
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
