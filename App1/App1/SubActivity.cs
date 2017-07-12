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
using Android.Util;

namespace App1
{
    [Activity(Label = "SubActivity")]
    public class SubActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.sub);

            var btn1 = FindViewById<Button>(Resource.Id.button1);
            var btn2 = FindViewById<Button>(Resource.Id.button2);
            btn1.Click += delegate
            {
                StartActivity(typeof(MainActivity));
                this.Finish();
            };
            btn2.Click += delegate
            {
                StartActivity(typeof(MainActivity));
            };
            Console.WriteLine("onCreate " + typeof(SubActivity).FullName);            
        }

        protected override void OnStart()
        {
            base.OnStart();
            Console.WriteLine("OnStart " + typeof(SubActivity).FullName);
        }

        protected override void OnResume()
        {
            base.OnResume();
            Console.WriteLine("OnResume " + typeof(SubActivity).FullName);
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            Console.WriteLine("OnRestart " + typeof(SubActivity).FullName);
        }

        protected override void OnPause()
        {
            base.OnPause();
            Console.WriteLine("OnPause " + typeof(SubActivity).FullName);
        }

        protected override void OnStop()
        {
            base.OnStop();
            Console.WriteLine("OnStop " + typeof(SubActivity).FullName);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Console.WriteLine("OnDestroy " + typeof(SubActivity).FullName);
        }
    }
}