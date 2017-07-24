using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using System.Net;

namespace AppX45
{
    [Activity(Label = "AppX45", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            base.SetContentView(Resource.Layout.Main);
            var btn = FindViewById<Button>(Resource.Id.abc);
            btn.Click += Btn_Click;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.menu1, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.exit)
            {
                Toast.MakeText(this, "exit", ToastLength.Short).Show();

                var root = Android.OS.Environment.ExternalStorageDirectory.Path;
                var downfile = System.IO.Path.Combine(root, "lzl.jpg");
                WebClient web = new WebClient();
                web.DownloadFileAsync(new System.Uri("http://img2.ph.126.net/C6bYWOb6J8ZXPmhWRaF6eA==/2990953102627448678.jpg"), downfile);
                web.DownloadFileCompleted += Web_DownloadFileCompleted;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void Web_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Toast.MakeText(this, "down", ToastLength.Short).Show();
        }

        private void Btn_Click(object sender, System.EventArgs e)
        {
            //this.Window.SetFlags(WindowManagerFlags.TranslucentStatus, WindowManagerFlags.TranslucentStatus);
            //var uiOptions = (int)this.Window.DecorView.WindowSystemUiVisibility;
        }
    }
}

