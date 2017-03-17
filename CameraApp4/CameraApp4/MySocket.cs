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

        private MySocket()
        {

        }

        private static MySocket _current = new MySocket();
        public static MySocket Current
        {
            get
            {
                return _current;
            }
        }

        public void Init(string serverIp)
        {
            ws = new WebSocket(string.Format("ws://{0}:4649/Echo", serverIp));
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
        }

        private void Ws_OnError(object sender, ErrorEventArgs e)
        {
            Toast.MakeText(Application.Context, "server error", ToastLength.Short).Show();
            Reconnect();
        }

        private void Ws_OnClose(object sender, CloseEventArgs e)
        {
            Toast.MakeText(Application.Context, "server close", ToastLength.Short).Show();
            Reconnect();
        }

        private void Ws_OnOpen(object sender, EventArgs e)
        {
            Toast.MakeText(Application.Context, "server connect", ToastLength.Short).Show();
        }

        public void Send(string msg)
        {
            ws.SendAsync(msg, null);
        }

        private bool bRun = false;
        private bool bReconnect = false;
        private const int connect_interval = 5000;
        private void Reconnect()
        {
            ws.Connect();
            //if (bRun)
            //    return;

            //Task.Factory.StartNew(() =>
            //{
            //    bRun = true;
            //    while (bReconnect)
            //    {
            //        ws.Connect();
            //        Log.Info("����һ��", this.GetType().FullName);
            //        Thread.Sleep(connect_interval);
            //    }
            //});
        }
    }
}