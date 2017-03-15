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
    [Activity(Label = "AnimActivity", MainLauncher = true)]
    public class BarActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Window.SetStatusBarColor(Android.Graphics.Color.Black);
            // Create your application here            
            SetContentView(Resource.Layout.bar);
        }
    }
}