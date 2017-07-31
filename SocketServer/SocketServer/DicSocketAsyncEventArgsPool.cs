using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
namespace SocketServer
{
    public class DicSocketAsyncEventArgsPool : ASocketAsyncEventArgsPool
    {
        public static Dictionary<string, Socket> socketClient = new Dictionary<string, Socket>();
        public override int Count
        {
            get
            {
                return socketClient.Count;
            }
        }
        public override void Push(Socket item, string Id)
        {
            lock (socketClient)
            {
                try
                {
                    Socket vSocket;
                    socketClient.TryGetValue(Id, out vSocket);
                    if (vSocket == null)
                    {
                        socketClient.Add(Id,item);
                    }
                    else { return; }
                }
                catch (Exception e)
                {
                    Console.WriteLine("IOException source: {0}", e.Source);
                }

            }


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>给谁发送消息
        /// <param name="from"></param>谁发的消息
        /// <param name="content"></param>发送的内容
        public override void Send(string to, string from, string content)
        {
            lock (socketClient)
            {
                try
                {
                    Socket vSocket;
                    socketClient.TryGetValue(to, out vSocket);
                    if (vSocket != null)
                    {
                        vSocket.Send(Encoding.UTF8.GetBytes(content));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("IOException source: {0}", e.Source);
                }

            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>给谁发送消息
        /// <param name="content"></param>发送的内容
        public override void SendAll(string from, string content)
        {
            lock (socketClient)
            {
                try
                {
                    foreach (var socket in socketClient)
                    {

                        socket.Value.Send(Encoding.UTF8.GetBytes(content));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("IOException source: {0}", e.Source);
                }

            }

        }
    }
}
