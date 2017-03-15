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
using Newtonsoft.Json;
using FaceVisual.Code;
using FaceVisual;
using System.Threading.Tasks;

namespace FaceVisual
{
    class HttpSocket
    {
        private string koalaIp = "";
        private WebSocket socket = null;
        private Action<FaceRecognized> callback = null;

        public Task Connect(string koalaIp, string cameraIp)
        {
            return Task.Factory.StartNew(() =>
            {
                this.koalaIp = koalaIp;
                var wsUrl = string.Format("ws://{0}:9000", koalaIp);
                var rtspUrl = string.Format("rtsp://{0}/user=admin&password=&channel=1&stream=0.sdp?", cameraIp);
                var url = string.Concat(wsUrl, "?url=", rtspUrl.UrlEncode());
                socket = new WebSocket(url);
                socket.OnOpen += Socket_OnOpen;
                socket.OnError += Socket_OnError;
                socket.OnClose += Socket_OnClose;
                socket.OnMessage += Socket_OnMessage;
                socket.Connect();
            });
        }

        public void Close()
        {
            socket?.Close();
        }

        public void SetCallback(Action<FaceRecognized> callback)
        {
            this.callback = callback;
        }

        private void Socket_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.IsText)
            {
                var entity = JsonConvert.DeserializeObject<FaceRecognized>(e.Data);
                if (entity.type == RecognizeState.recognized.ToString())
                {
                    callback?.Invoke(entity);
                }
            }
        }

        private void Socket_OnError(object sender, ErrorEventArgs e)
        {
            Config.Log(koalaIp + " Websocket error");
            Toast.MakeText(Application.Context, "WebSocket connection error", ToastLength.Short).Show();
        }

        private void Socket_OnClose(object sender, CloseEventArgs e)
        {
            Config.Log(koalaIp + " Websocket close");
            Toast.MakeText(Application.Context, "WebSocket connection close", ToastLength.Short).Show();
        }

        private void Socket_OnOpen(object sender, EventArgs e)
        {
            Toast.MakeText(Application.Context, "WebSocket connect ok", ToastLength.Short).Show();
        }
    }
}