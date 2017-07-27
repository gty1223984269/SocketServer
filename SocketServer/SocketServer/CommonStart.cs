using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketServer;
using System.Net.Sockets;
namespace SocketServer
{
 public class CommonStart
    {
        static object locker = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>泛型
        /// <param name="Uid"></param>登录的Id
        /// <param name="client"></param>socket实例
        public static void Registe(string Uid, Socket client)
        {
            //线程同步
            lock (locker)
            {
                try
                {
                    Socket vSocket;
                    SocketServer.SocketServices.socketClient.TryGetValue(Uid,out vSocket);
                    if (vSocket == null)
                    {
                        SocketServer.SocketServices.socketClient.Add(Uid, client);
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
        public static void Send(string to,string from,string content)
        {
            lock (locker)
            {
                try
                {
                    Socket vSocket;
                    SocketServer.SocketServices.socketClient.TryGetValue(to, out vSocket);
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
        public static void SendAll(string from,string content)
        {
            lock (locker)
            {
                try
                {
                    foreach (var socket in SocketServer.SocketServices.socketClient)
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
