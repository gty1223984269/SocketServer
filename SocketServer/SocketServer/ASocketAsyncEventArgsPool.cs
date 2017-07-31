using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace SocketServer
{
   public abstract class ASocketAsyncEventArgsPool
    {
        public virtual void Push(SocketAsyncEventArgs item) { }
        public virtual void Push(Socket item,string Id) { }
        public virtual SocketAsyncEventArgs Pop() { return null; }
        public abstract int Count { get; }
        public virtual void Push(SocketAsyncEventArgs item, string Id) { }
        public virtual void Send(string to, string from, string content) { }
        public virtual void SendAll(string from, string content) { }
    }

   
}
