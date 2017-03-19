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

namespace Android_Demo
{
    //, Theme = "@style/AppTheme"
    [Activity(Label = "ImageActivity", MainLauncher = false )]
    public class ImageActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.image);
            // Create your application here
        }
    }
}