using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WebSocketSharp;
using System.IO;
using System.Json;
using System.Threading;
using System.Threading.Tasks;
using Android.Util;

namespace CameraApp4
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

        private Activity activity = null;
        public MySocket(Activity activity)
        {
            this.activity = activity;
        }

        public Task Init(string serverIp)
        {
            return Task.Factory.StartNew(() =>
            {
                ws = new WebSocket(string.Format("ws://{0}:4649/Echo", serverIp));
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
                StringReader sr = new StringReader(e.Data);
                JsonValue jv = JsonObject.Load(sr);
                var cmd = jv["action"];
                if (cmd == "capture")
                {
                    _capture();
                }
                else if (cmd == "pass")
                {
                    _onPassResult?.Invoke(e.Data);
                }
            }
            else if (e.IsPing)
            {
                Log.Info("DEBUG", DateTime.Now.ToString("HH:mm:ss") + " Ping");
            }
        }

        private void Ws_OnError(object sender, ErrorEventArgs e)
        {
            Config.Log("WebSocket Error");
            Reconnect();
        }

        private void Ws_OnClose(object sender, CloseEventArgs e)
        {
            Config.Log("WebSocket close");
        }

        private void Ws_OnOpen(object sender, EventArgs e)
        {
            Alert("server connect");
        }

        public void Send(string msg)
        {
            ws.Send(msg);
        }

        private void Reconnect()
        {
            Close();
            Init(Config.Profile.ServerIp);
        }

        public void Close()
        {
            ws?.Close();
            ws = null;
        }

        private void Alert(string msg)
        {
            activity.RunOnUiThread(new Action(() =>
            {
                Toast.MakeText(Application.Context, msg, ToastLength.Short).Show();
            }));
        }
    }
}