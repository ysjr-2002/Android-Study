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

namespace CameraApp4
{
    [Activity(Label = "InitActivity", MainLauncher = false)]
    public class InitActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.init);
        }

        private void Current_OnCaputure()
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            this.Finish();
        }
    }
}