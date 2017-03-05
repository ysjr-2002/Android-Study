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

namespace App5
{
    [Activity(Label = "SubMain")]
    public class SubMainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SubMain);


            var msg = Intent.GetStringExtra("My Data");
            var textView = FindViewById<TextView>(Resource.Id.textView1);
            textView.Text = msg;
        }

        protected override void OnPause()
        {
            base.OnPause();
            Console.WriteLine("被暂停了");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Console.WriteLine("被销毁了");
        }
    }
}