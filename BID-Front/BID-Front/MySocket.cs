using Android.App;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using WebSocketSharp;

namespace BID_Front
{
    class MySocket
    {
        private WebSocket _webSocket = null;
        private Activity _activity = null;
        private Action _saveBack = null;

        public MySocket(Activity activity, Action saveBack)
        {
            this._activity = activity;
            this._saveBack = saveBack;
        }

        public void Connect(string serverIp)
        {
            _webSocket = new WebSocket(string.Format("ws://{0}:4649/Employee", serverIp));
            _webSocket.OnOpen += Ws_OnOpen;
            _webSocket.OnClose += Ws_OnClose;
            _webSocket.OnError += Ws_OnError;
            _webSocket.OnMessage += Ws_OnMessage;
            _webSocket.Connect();
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.IsText)
            {
                var message = JsonConvert.DeserializeObject<Message>(e.Data);
                if (message.action == "employee")
                {
                    if (message.type == "save")
                    {
                        _saveBack.BeginInvoke(null, null);
                    }
                }
            }
        }

        private void Ws_OnError(object sender, ErrorEventArgs e)
        {
            //Alert("server error");
        }

        private void Ws_OnClose(object sender, CloseEventArgs e)
        {
            //Alert("server close");
        }

        private void Ws_OnOpen(object sender, EventArgs e)
        {
            //Alert("websocket ok");
        }

        public void Send(string data)
        {
            _webSocket.Send(data);
        }

        public void Close()
        {
            _webSocket?.Close();
            _webSocket = null;
        }
    }
}