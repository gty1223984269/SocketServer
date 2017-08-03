using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
namespace SocketServer
{
  public  class HttpServer
    {
        private static HttpListener listener = null;
        // This example requires the System and System.Net namespaces.
        public static void SimpleListenerExample(string[] prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required,
            // for example "http://127.0.0.1:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            listener = new HttpListener();
            // Add the prefixes.
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            Console.WriteLine("http服务器已经启动Listening...");
            // Note: The GetContext method blocks while waiting for a request. 
            //HttpListenerContext context = listener.GetContext();
            listener.BeginGetContext(new AsyncCallback(GetContextCallBack), listener);



        }

    public  static void GetContextCallBack(IAsyncResult ar)
         {
             try
             {
                 listener = ar.AsyncState as HttpListener;
 
                 HttpListenerContext context = listener.EndGetContext(ar);
                 listener.BeginGetContext(new AsyncCallback(GetContextCallBack), listener);
                 processRequest(context);


             }
             catch { }
             
         }


        public static void processRequest(HttpListenerContext context)
        {

            HttpListenerRequest request = context.Request;
            var Id = request.QueryString["Id"];
            var tempContent = request.QueryString["Content"];
            var method = request.QueryString["method"];
            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            // Construct a response.
            DicSocketAsyncEventArgsPool dp = new DicSocketAsyncEventArgsPool();
            int connectCount = dp.Count;
            switch (method)
            {
                case "Send":
                    dp.Send(Id, "", tempContent);
                    break;
                case "SendAll":
                    dp.SendAll("", tempContent);
                    break;
            }
            string responseString = "<HTML><BODY>" + connectCount + "</BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            output.Close();

        }
    }

   
}
