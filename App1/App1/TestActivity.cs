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

namespace App1
{
    [Activity(Label = "TestActivity", MainLauncher = true, Theme = "@style/testTheme")]
    public class TestActivity : Activity
    {
        TextView abc;
        ImageView img;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.Test);
            // Create your application here
            //var button = FindViewById<Button>(Resource.Id.button1);
            //button.Click += Button_Click;
            ////abc = FindViewById<TextView>(Resource.Id.textView1);
            //img = FindViewById<ImageView>(Resource.Id.img);

        }

        private void Button_Click(object sender, EventArgs e)
        {
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

            var lp = img.LayoutParameters;
            var imgw = img.MeasuredWidth;
            var imgh = img.MeasuredHeight;

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
    }
}