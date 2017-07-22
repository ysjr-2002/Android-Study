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
using Android.Graphics;
using Android.Graphics.Drawables;

namespace FaceVisual
{
    [Activity(Label = "@string/ApplicationName", Theme = "@style/AppTheme", MainLauncher = true)]
    public class SplashActivity : RootActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.Splash);

            //var path = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "lzl.jpg");
            //var ll = FindViewById<LinearLayout>(Resource.Id.root);
            //BitmapDrawable bd = new BitmapDrawable(path);
            //ll.Background = bd;
            //Config.ReadProfile();
         

            Handler handler = new Handler();
            handler.PostDelayed(() =>
            {
                Intent intent = new Intent(this, typeof(FaceMainActivity));
                StartActivity(intent);
                Finish();
            }, 2000);
        }
    }
}