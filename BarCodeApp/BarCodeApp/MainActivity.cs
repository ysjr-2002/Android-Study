using Android.App;
using Android.Widget;
using Android.OS;
using System.Net.Sockets;
using System;
using System.Net;
using System.Text;
using Android.Content;
using System.Timers;

namespace BarCodeApp
{
    [Activity(Label = "BarCodeApp", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/mainTheme")]
    public class MainActivity : Activity
    {
        private TextView time;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            this.SetContentView(Resource.Layout.Main);
            time = FindViewById<TextView>(Resource.Id.time);
        }

        protected override void OnStart()
        {
            base.OnStart();
            Intent intent = new Intent(this, typeof(UdpService));
            StartService(intent);
            StartTimer(TimeSpan.FromSeconds(1), ShowTime);
        }

        private bool ShowTime()
        {
            time.Text = DateTime.Now.ToString("HH:mm:ss");
            return true;
        }

        public void StartTimer(TimeSpan interval, Func<bool> callback)
        {
            var handler = new Handler(Looper.MainLooper);
            handler.PostDelayed(() =>
            {
                if (callback())
                    StartTimer(interval, callback);

                handler.Dispose();
                handler = null;
            }, (long)interval.TotalMilliseconds);
        }

        private void GetIp()
        {
            var address = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (var ip in address)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Toast.MakeText(this, ip.ToString(), ToastLength.Short).Show();
                }
            }
        }

        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / this.Resources.DisplayMetrics.Density);
            return dp;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Intent intent = new Intent(this, typeof(UdpService));
            StopService(intent);
        }
    }
}

