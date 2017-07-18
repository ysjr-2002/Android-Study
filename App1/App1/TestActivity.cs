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
using Android.Graphics;
using static Android.Widget.ImageView;
using System.Net;
using Android.Preferences;

namespace App1
{
    //Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen"
    [Activity(Label = "TestActivity", MainLauncher = false)]
    public class TestActivity : Activity
    {
        TextView abc;
        ImageView img;
        Button btncopy;
        Button button3;
        Button button4;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.Test);
            // Create your application here
            var button = FindViewById<Button>(Resource.Id.button1);
            button.Click += Button_Click;
            //abc = FindViewById<TextView>(Resource.Id.textView1);
            img = FindViewById<ImageView>(Resource.Id.img);

            btncopy = FindViewById<Button>(Resource.Id.button2);
            btncopy.Click += Btncopy_Click;

            button3 = FindViewById<Button>(Resource.Id.button3);
            button3.Click += Button3_Click;

            button4 = FindViewById<Button>(Resource.Id.button4);
            button4.Click += Button4_Click;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            AppPreferences app = new App1.AppPreferences(this);
            var content = app.getAccessKey();
            Toast.MakeText(this, content, ToastLength.Short).Show();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            //AppPreferences app = new App1.AppPreferences(this);
            //app.saveAccessKey("杨绍杰");
            Intent intent = new Intent(this, typeof(ResultActivity));
            StartActivity(intent);
        }

        private void Btncopy_Click(object sender, EventArgs e)
        {
            //var root = Android.OS.Environment.ExternalStorageDirectory;
            //var source = root + "/lzl.jpg";
            //var density = root + "/pictures/obria/lzl.jpg";
            //System.IO.File.Copy(source, density);
            //Toast.MakeText(this, "复制成功", ToastLength.Short).Show();

            StartActivity(typeof(Sub1Activity));
        }

        int mycode = 11;
        private void Button_Click(object sender, EventArgs e)
        {
            var root = Android.OS.Environment.ExternalStorageDirectory;
            var path = System.IO.Path.Combine(root.Path, "pictures");
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            path = System.IO.Path.Combine(path, "obria");
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            //path = System.IO.Path.Combine(root.Path, "lzl.jpg");
            WebClient web = new WebClient();
            web.DownloadFileCompleted += Web_DownloadFileCompleted;
            web.DownloadFileAsync(new Uri("http://ent.sun0769.com/star/bg/W020100520427180311645.jpg"), path);

            //Intent intent = new Intent();
            //intent.SetAction(Intent.ActionGetContent);
            //intent.SetType("image/*");
            //StartActivityForResult(intent, mycode);

            //var sa = AnimationUtils.LoadAnimation(this, Resource.Animation.anim_slide_in_left);
            //abc.StartAnimation(sa);

            //var drawable = this.GetDrawable(Resource.Drawable.on);
            //img.SetImageDrawable(drawable);

            //this.GetDrawable()

            //BitmapFactory.Options options = new BitmapFactory.Options();
            //options.InJustDecodeBounds = true;
            //options.InSampleSize = 2;
            //options.InJustDecodeBounds = false;

            //img.SetScaleType(ScaleType.Center);
            //var temp = BitmapFactory.DecodeResource(this.Resources, Resource.Drawable.on, options);

            //var w = temp.Width;
            //var h = temp.Height;

            //var lp = img.LayoutParameters;
            //var imgw = img.MeasuredWidth;
            //var imgh = img.MeasuredHeight;

            //img.SetImageBitmap(temp);
            //if (temp != null && !temp.IsRecycled)
            //{
            //    temp.Dispose();
            //    //temp.Dispose();
            //    temp = null;
            //    GC.Collect();
            //}

            //img.SetImageResource(Resource.Drawable.on);
        }

        private void Web_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Toast.MakeText(this, "下载结束", ToastLength.Short).Show();
        }

        public override void StartActivityForResult(Intent intent, int requestCode)
        {
            if (requestCode != mycode)
            {
                return;
            }

            var data = intent.Data;
            img.SetImageURI(data);
        }
    }
}