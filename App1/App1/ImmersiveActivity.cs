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
    [Activity(Label = "ImmersiveActivity", MainLauncher = false, Theme = "@android:style/Theme.Material.Light")]
    public class ImmersiveActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            //this.SetContentView(Resource.Layout.immersive);
            //var btn = FindViewById<Button>(Resource.Id.button1);
            //btn.Click += Btn_Click;
            //this.Window.DecorView.SystemUiVisibilityChange += DecorView_SystemUiVisibilityChange;
        }

        private void DecorView_SystemUiVisibilityChange(object sender, View.SystemUiVisibilityChangeEventArgs e)
        {
            var height = this.Window.DecorView.Height;
            Toast.MakeText(this, height.ToString(), ToastLength.Short).Show();
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            ToggleHideBar();
        }

        private void ToggleHideBar()
        {
            var uiOptions = this.Window.DecorView.SystemUiVisibility;
            var newUiOptions = uiOptions;

            //bool isImmersiveEnable = uiOptions | View.SystemUiFlagImmersiveSticky

            if (Build.VERSION.SdkInt == BuildVersionCodes.Lollipop)
            {
                Toast.MakeText(this, "shit", ToastLength.Short).Show();
            }

            if (uiOptions == StatusBarVisibility.Visible)
            {
                this.Window.DecorView.SystemUiVisibility = StatusBarVisibility.Hidden;
            }
            else
            {
                this.Window.DecorView.SystemUiVisibility = StatusBarVisibility.Visible;
            }
        }
    }
}