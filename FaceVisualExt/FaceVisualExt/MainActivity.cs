using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Hardware.Display;
using Android.Media;
using System.IO;
using System.Threading.Tasks;
using Android.Net;

namespace FaceVisualExt
{
    [Activity(Label = "FaceVisualExt", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Unspecified)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            this.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            SetContentView(Resource.Layout.front);
            ShowDisplay1();

            var btn = FindViewById<Button>(Resource.Id.MyButton);
            btn.Click += delegate
            {
                debug("are you ok");
            };
        }

        private void ShowDisplay1()
        {
            DisplayManager displayManager;//屏幕管理类
            Display[] displays;//屏幕数组
            displayManager = (DisplayManager)this.GetSystemService(Context.DisplayService);
            displays = displayManager.GetDisplays();
            //主屏
            if (displays.Length >= 2)
            {
                //副屏
                var display1 = displays[1];
                DifferentDislay anotherDisplay = new DifferentDislay(this, display1);
                anotherDisplay.Window.SetType(WindowManagerTypes.SystemAlert);
                anotherDisplay.Show();
            }
            else
            {
                return;
            }
        }

        private void debug(string str)
        {
            Toast.MakeText(this, str, ToastLength.Short).Show();
        }
    }
}

