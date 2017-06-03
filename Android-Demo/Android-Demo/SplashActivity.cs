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
using System.Threading.Tasks;

namespace Android_Demo
{
    [Activity(Label = "SplashActivity", MainLauncher = true, Theme = "@style/AppTheme")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1);
                Intent intent = new Intent(this, typeof(FrameActivity));
                StartActivity(intent);
                Finish();
            });
        }
    }
}