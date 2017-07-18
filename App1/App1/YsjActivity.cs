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

namespace App1
{
    [Activity(Label = "YsjActivity", MainLauncher = true, Theme = "@style/ysjTheme")]
    public class YsjActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.ysj);
            // Create your application here

            var btn = FindViewById<Button>(Resource.Id.button1);
            btn.Click += Btn_Click;
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(TestActivity));
            this.Finish();
            this.OverridePendingTransition(Resource.Animation.fade_in, Resource.Animation.fade_out);
        }
    }
}