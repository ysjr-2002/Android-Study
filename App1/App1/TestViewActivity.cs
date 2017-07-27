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
    [Activity(Label = "TestViewActivity", MainLauncher = true)]
    public class TestViewActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TestView);
        }

        protected override void OnStart()
        {
            base.OnStart();

            var width = this.Resources.DisplayMetrics.WidthPixels;
            var height = this.Resources.DisplayMetrics.HeightPixels;
            var xhdpi = this.Resources.DisplayMetrics.Xdpi;
            var yhdpi = this.Resources.DisplayMetrics.Ydpi;
            Toast.MakeText(this, width + " " + height + " " + xhdpi + " " + yhdpi, ToastLength.Short).Show();


            var displayMetrics = new DisplayMetrics();
            this.WindowManager.DefaultDisplay.GetMetrics(displayMetrics);

            if (displayMetrics == this.Resources.DisplayMetrics)
            {

            }
        }
    }
}