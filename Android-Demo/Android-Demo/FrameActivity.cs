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

namespace Android_Demo
{
    [Activity(Label = "FrameActivity", MainLauncher = false)]
    public class FrameActivity : Activity
    {
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
                //intent.AddCategory(Intent.CategoryHome);
                intent.SetData(Android.Net.Uri.Parse("http://www.baidu.com"));
                StartActivity(intent);
            };
        }

        private void Start()
        {
        }
    }
}