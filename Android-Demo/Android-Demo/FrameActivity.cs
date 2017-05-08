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
using System.Threading;

namespace Android_Demo
{
    [Activity(Label = "FrameActivity", MainLauncher = false)]
    public class FrameActivity : Activity
    {
        private Handler handler = new Handler();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            this.SetContentView(Resource.Layout.frame);

            var btn = base.FindViewById<Button>(Resource.Id.btnhome);
            btn.Click += delegate
            {
                Intent intent = new Intent();
                intent.SetAction(Intent.ActionMain);
                intent.AddCategory(Intent.CategoryHome);
                StartActivity(intent);
            };

            var btn1 = base.FindViewById<Button>(Resource.Id.btnopenurl);
            btn1.Click += delegate
            {
                Intent intent = new Intent();
                intent.SetAction(Intent.ActionView);
                intent.SetData(Android.Net.Uri.Parse("http://www.baidu.com"));
                StartActivity(intent);
            };

            var btn2 = base.FindViewById<Button>(Resource.Id.btnsetprogress);
            var pb3 = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            btn2.Click += delegate
            {
                Thread thread = new Thread(() =>
                {
                    while (true)
                    {
                        if (pb3.Progress == 100)
                        {
                            handler.Post(() =>
                            {
                                Toast.MakeText(this, "更新完成", ToastLength.Short).Show();
                            });
                            break;
                        }
                        handler.Post(() =>
                        {
                            pb3.Progress++;
                        });
                        Thread.Sleep(100);
                    }
                });
                thread.Start();
            };
        }

        private void Start()
        {
        }
    }
}