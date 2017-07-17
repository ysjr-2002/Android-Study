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
    [Activity(Label = "ResultActivity")]
    public class ResultActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            base.SetContentView(Resource.Layout.result);
            // Create your application here

            var button1 = FindViewById<Button>(Resource.Id.button1);
            var txt = FindViewById<TextView>(Resource.Id.textview1);
            button1.Click += delegate
            {
                Intent intent = new Intent();
                intent.PutExtra("ysj", txt.Text);
                this.SetResult(Result.Ok, intent);
                this.Finish();
            };
        }
    }
}