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
        private WebSocket ws = null;
        private Action _capture;
        private Action<string> _onPassResult;

        public event Action OnCaputure
        {
            add
            {
                _capture = value;
            }
            remove
            {
                _capture -= value;
            }
        }

        public event Action<string> OnPass
        {
            add
            {
                _onPassResult = value;
            }
            remove
            {
                _onPassResult -= value;
            }
        }

        private Activity activity;
        public MySocket(Activity activity)
        {
            this.activity = activity;
        }

        public Task Connect(string serverIp)
        {
            return Task.Factory.StartNew(() =>
            {
                ws = new WebSocket(string.Format("ws://{0}:4649/Employee", serverIp));
                ws.OnOpen += Ws_OnOpen;
                ws.OnClose += Ws_OnClose;
                ws.OnError += Ws_OnError;
                ws.OnMessage += Ws_OnMessage;
                ws.Connect();
                ws.EmitOnPing = true;
            });
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.IsText)
            {
                var message = JsonConvert.DeserializeObject<Message>(e.Data);
                if (message.action == "employee")
                {
                }
            }
        }

        private void Ws_OnError(object sender, ErrorEventArgs e)
        {
            Alert("server error");
            //Reconnect();
        }

        private void Ws_OnClose(object sender, CloseEventArgs e)
        {
            Alert("server close");
            //Reconnect();
        }

        private void Ws_OnOpen(object sender, EventArgs e)
        {
            Alert("websocket ok");
        }

        public void Send(string msg)
        {
            ws.SendAsync(msg, null);
        }

        private void Alert(string msg)
        {
            Toast.MakeText(Application.Context, msg, ToastLength.Short).Show();
        }

        private void Reconnect()
        {
            ws.Connect();
        }
    }
}