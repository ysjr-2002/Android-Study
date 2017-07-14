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
using Android.Views.Animations;

namespace App1
{
    [Activity(Label = "TestActivity", MainLauncher = true)]
    public class TestActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.Test);
            // Create your application here

            var button = FindViewById<Button>(Resource.Id.button1);
            button.Click += Button_Click;

            abc = FindViewById<TextView>(Resource.Id.textView1);
        }

        TextView abc;
        private void Button_Click(object sender, EventArgs e)
        {
            var sa = AnimationUtils.LoadAnimation(this, Resource.Animation.anim_slide_in_left);
            abc.StartAnimation(sa);
        }
    }
}