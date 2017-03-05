using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace CameraApp6
{
    [Activity(Label = "CameraApp6", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //RequestWindowFeature(WindowFeatures.NoTitle);
            //// Set our view from the "main" layout resource
            //SetContentView(Resource.Layout.Main);

            //// Get our button from the layout resource,
            //// and attach an event to it
            //Button button = FindViewById<Button>(Resource.Id.MyButton);
            //button.Click += delegate
            //{
            //    CheckSDCard();
            //};

            ActionBar.Hide();
            SetContentView(Resource.Layout.activity_camera);
            if (bundle == null)
            {
                FragmentManager.BeginTransaction().Replace(Resource.Id.container, Camera2BasicFragment.NewInstance()).Commit();
            }
        }


        private bool CheckSDCard()
        {
            var path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.MediaMounted);
            var a = Android.OS.Environment.GetExternalStorageState(path);
            var b = Android.OS.Environment.MediaMounted;
            if (a == b)
            {
                Toast.MakeText(this, "SD卡存在", ToastLength.Short).Show();
                return true;
            }
            else
            {
                Toast.MakeText(this, "SD不卡存在", ToastLength.Short).Show();
                return false;
            }
        }
    }
}

