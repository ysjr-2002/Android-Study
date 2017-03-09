using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WebServer_WPF.server
{
    class Echo : WebSocketBehavior
    {
        Action<byte[]> callback = null;
        public Echo(Action<byte[]> callback)
        {
            this.callback = callback;
            Console.WriteLine("我被初始化了");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.IsText)
            {
                var data = e.Data;
                var buffer = Convert.FromBase64String(data);
                var fs = File.Create("d:\\" + DateTime.Now.ToString("HHmmss") + ".jpg");
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();
                callback(buffer);
            }
            else if (e.IsBinary)
            {
                var data = e.RawData;
                //var buffer = Convert.FromBase64String(data);
                var fs = File.Create("d:\\android.jpg");
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
        }
    }
}
