using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Hardware.Display;

namespace FaceVisualExt
{
    [Activity(Label = "FaceVisualExt", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            this.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            SetContentView(Resource.Layout.Main);
            //GetDisplay();
        }

        private void GetDisplay()
        {
            DisplayManager displayManager;//屏幕管理类
            Display[] displays;//屏幕数组
            displayManager = (DisplayManager)this.GetSystemService(Context.DisplayService);
            displays = displayManager.GetDisplays();
            //主屏
            var display1 = displays[0];
            if (displays.Length >= 2)
            {
                //副屏
                var display2 = displays[1];
            }

            DifferentDislay anotherDisplay = new DifferentDislay(this, display1);
            //mPresentation.getWindow().setType(
            //WindowManager.LayoutParams.TYPE_SYSTEM_ALERT);
            //mPresentation.Window.SetType(WindowManagerLayoutParams)
            anotherDisplay.Window.SetType(WindowManagerTypes.SystemAlert);
            anotherDisplay.Show();

            //AlertDialog.Builder builder = new AlertDialog.Builder(this);
            //builder.SetTitle("你好");
            //builder.SetView(Resource.Layout.Main);
            //var dialog = builder.Create();
            //dialog.Show();
        }
    }
}

