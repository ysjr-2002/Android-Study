using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Java.Lang;
using System.Net.Sockets;

namespace AutoUpgrade
{
    [Activity(Label = "AutoUpgrade", MainLauncher = false, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;
        string localFile = "";
        private Button btn2;
        public const int MESSAGE_STATE_CHANGE = 1;
        public const int MESSAGE_READ = 2;
        public const int MESSAGE_WRITE = 3;
        public const int MESSAGE_DEVICE_NAME = 4;
        public const int MESSAGE_TOAST = 5;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate
            {
                WebClient web = new WebClient();

                string url = "http://192.168.1.131:9872/1.apk";
                Uri uri = new Uri(url);

                string root = Android.OS.Environment.ExternalStorageDirectory.Path;
                string folder = "upgrade";

                var path = System.IO.Path.Combine(root, folder);
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                localFile = System.IO.Path.Combine(path, "ysj.apk");

                if (System.IO.File.Exists(localFile))
                {
                    System.IO.File.Delete(localFile);
                }
                web.DownloadFileAsync(uri, localFile);
                web.DownloadFileCompleted += Web_DownloadFileCompleted;
            };

            btn2 = this.FindViewById<Button>(Resource.Id.MyButton2);
            MyHandler handler = new MyHandler(this);
            btn2.Click += delegate
            {
                //while (true)
                //{
                //    handler.ObtainMessage(MESSAGE_STATE_CHANGE).SendToTarget();
                //    System.Threading.Thread.Sleep(1000);
                //}
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        var data = System.Text.Encoding.UTF8.GetBytes("data->" + DateTime.Now);
                        UdpClient udp = new UdpClient();
                        udp.Send(data, data.Length, new IPEndPoint(IPAddress.Parse("192.168.1.116"), 9872));
                        System.Threading.Thread.Sleep(1000);
                    }
                });
            };
        }

        private class MyHandler : Handler
        {
            private MainActivity _activity;
            public MyHandler(MainActivity activity)
            {
                _activity = activity;
            }

            public override void HandleMessage(Message msg)
            {
                switch (msg.What)
                {
                    case MESSAGE_STATE_CHANGE:
                        _activity.btn2.Text = DateTime.Now.ToString("HH:mm:ss");
                        break;
                    case MESSAGE_READ:
                        break;
                    case MESSAGE_WRITE:
                        break;
                    case MESSAGE_DEVICE_NAME:
                        break;
                }
            }
        }

        private void Web_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Intent intent = new Intent(Intent.ActionView);
            intent.SetFlags(ActivityFlags.NewTask);
            intent.SetDataAndType(Android.Net.Uri.Parse(string.Concat("file://", localFile)), "application/vnd.android.package-archive");
            StartActivity(intent);
            //this.Finish();
        }
    }
}

