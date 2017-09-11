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
using System.Net.Sockets;
using System.Net;

namespace BarCodeApp
{
    [Service]
    class UdpService : Service
    {
        UdpClient udp = null;
        const int port = 9876;
        //本虚拟机Ip为192.168.66.101
        public override void OnCreate()
        {
            base.OnCreate();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            udp = new UdpClient(port);
            udp.BeginReceive(BeginRecieve, null);
            return base.OnStartCommand(intent, flags, startId);
        }

        Handler handler = new Handler();
        private void BeginRecieve(IAsyncResult ir)
        {
            IPEndPoint remoteIp = null;
            var buffer = udp.EndReceive(ir, ref remoteIp);
            if (buffer.Length > 0)
            {
                var barcode = Encoding.UTF8.GetString(buffer);
                handler.Post(() =>
                {
                    var toast = Toast.MakeText(this, barcode, ToastLength.Short);
                    toast.SetGravity(Android.Views.GravityFlags.Bottom, 0, 46);
                    toast.Show();
                });
                udp.BeginReceive(BeginRecieve, null);
            }
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}