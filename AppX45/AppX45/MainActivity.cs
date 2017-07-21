using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;

namespace AppX45
{
    [Activity(Label = "AppX45", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.Window.ClearFlags(WindowManagerFlags.TranslucentStatus | WindowManagerFlags.TranslucentNavigation);
            this.Window.RequestFeature(WindowFeatures.NoTitle);
            this.Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            this.Window.SetStatusBarColor(Color.Transparent);
            this.Window.SetNavigationBarColor(Color.Transparent);
            // Set our view from the "main" layout resource
            base.SetContentView(Resource.Layout.Main);
            var uiOptions = this.Window.DecorView.WindowSystemUiVisibility;

            if ((uiOptions & SystemUiFlags.Visible) == SystemUiFlags.Visible)
            {

            }

            if ((uiOptions & SystemUiFlags.LowProfile) == SystemUiFlags.LowProfile)
            {

            }
            var btn = FindViewById<Button>(Resource.Id.abc);
            btn.Click += Btn_Click;
        }

        private void Btn_Click(object sender, System.EventArgs e)
        {
            //this.Window.SetFlags(WindowManagerFlags.TranslucentStatus, WindowManagerFlags.TranslucentStatus);
            //var uiOptions = (int)this.Window.DecorView.WindowSystemUiVisibility;

          
        }
    }
}

