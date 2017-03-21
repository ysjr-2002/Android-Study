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
        private Activity activity;

        public MySocket(Activity activity)
        {
            this.activity = activity;
        }

        public void Connect(string serverIp)
        {
            ws = new WebSocket(string.Format("ws://{0}:4649/Employee", serverIp));
            ws.OnOpen += Ws_OnOpen;
            ws.OnClose += Ws_OnClose;
            ws.OnError += Ws_OnError;
            ws.OnMessage += Ws_OnMessage;
            ws.Connect();
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
                        Alert("保存成功");
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
            ws.Send(data);
        }

        private void Alert(string msg)
        {
            activity.RunOnUiThread(new Action(() =>
            {
                Toast.MakeText(Application.Context, msg, ToastLength.Short).Show();
            }));
        }

        public void Close()
        {
            ws?.Close();
            ws = null;
        }
    }
}