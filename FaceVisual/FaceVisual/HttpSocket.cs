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
using System.Threading;
using System.Web;

namespace FaceVisual
{
    class HttpSocket
    {
        private string _koalaIp = "";
        private string _cameraIp = "";
        private bool _open = false;
        private WebSocket _socket = null;
        private Action<FaceRecognized> _callback = null;
        private Handler _handler = null;
        private const int sleep = 10 * 1000;
        public HttpSocket(Handler handler, string koalaIp, string cameraIp, Action<FaceRecognized> callback)
        {
            _handler = handler;
            _koalaIp = koalaIp;
            _cameraIp = cameraIp;
            _callback = callback;
        }

        public Task<bool> Connect()
        {
            return Task.Factory.StartNew(() =>
            {
                Dispose();

                var url = string.Format("ws://{0}:9000/video", _koalaIp.Trim());
                //C2
                var rtsp = string.Format("rtsp://{0}/user=admin&password=&channel=1&stream=0.sdp?", _cameraIp.Trim());
                //¾ÞÁú
                //var rtsp = string.Format("rtsp://{0}:554/media/live/1/1", _cameraIp.Trim());
                rtsp = HttpUtility.UrlEncode(rtsp);
                var all = string.Concat(url, "?url=", rtsp);

                _socket = new WebSocket(all);
                _socket.OnOpen += _socket_OnOpen;
                _socket.OnError += _socket_OnError;
                _socket.OnClose += _socket_OnClose;
                _socket.OnMessage += _socket_OnMessage;
                _socket.Connect();
                return _open;
            });
        }

        private void _socket_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.IsText)
            {
                var entity = JsonConvert.DeserializeObject<FaceRecognized>(e.Data);
                if (entity.type == RecognizeState.recognized.ToString())
                {
                    _callback.Invoke(entity);
                }
            }
        }

        private void _socket_OnClose(object sender, CloseEventArgs e)
        {
            _open = false;
            _handler.SendEmptyMessage(FaceMainActivity.connect_error);
            if (!_appclose)
            {
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(sleep);
                    Connect();
                });
            }
        }

        private void _socket_OnError(object sender, ErrorEventArgs e)
        {
            _open = false;
        }

        private void _socket_OnOpen(object sender, EventArgs e)
        {
            _open = true;
            _handler.SendEmptyMessage(FaceMainActivity.connect_ok);
        }

        private void Dispose()
        {
            if (_socket == null)
                return;

            if (_socket.ReadyState == WebSocketState.Open)
                _socket.Close();

            _socket.OnOpen -= _socket_OnOpen;
            _socket.OnClose -= _socket_OnClose;
            _socket.OnError -= _socket_OnError;
            _socket.OnMessage -= _socket_OnMessage;
            _socket = null;
        }

        private bool _appclose = false;
        public void Disconnect()
        {
            _appclose = true;
            _socket?.CloseAsync();
            _socket = null;
        }
    }
}