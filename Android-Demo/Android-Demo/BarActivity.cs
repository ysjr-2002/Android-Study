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
using Android.Graphics;
using Android.Views.Animations;
using static Android.Views.Animations.Animation;
using System.Threading.Tasks;
using System.Threading;
using Android.Util;

namespace Android_Demo
{
    [Activity(Label = "AnimActivity", MainLauncher = true)]
    public class BarActivity : Activity, IAnimationListener
    {
        public void OnAnimationEnd(Animation animation)
        {
        }

        public void OnAnimationRepeat(Animation animation)
        {
        }

        public void OnAnimationStart(Animation animation)
        {
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 0x33, 0xcc, 0xff));
            // Create your application here            
            base.SetContentView(Resource.Layout.bar);
            var buttona = FindViewById<Button>(Resource.Id.buttona);
            var buttons = FindViewById<Button>(Resource.Id.buttons);
            var buttond = FindViewById<Button>(Resource.Id.buttonalter);
            var ivSon = FindViewById<ImageView>(Resource.Id.imageView1);
            buttona.Click += delegate
            {
                //var ah = AnimationUtils.LoadAnimation(this, Resource.Animation.alpha);
                var ah = new AlphaAnimation(0, 1.0f);
                ivSon.StartAnimation(ah);
            };

            buttons.Click += delegate
            {
                var sa = AnimationUtils.LoadAnimation(this, Resource.Animation.scale);
                //ScaleAnimation sa = new ScaleAnimation(0, 2, 0, 2, 50f, 50f);
                //sa.Interpolator = new AccelerateInterpolator();
                //sa.FillAfter = true;
                //sa.Duration = 1000;
                ivSon.StartAnimation(sa);
            };

            buttond.Click += delegate
            {
                var view = LinearLayout.Inflate(this, Resource.Layout.visitor, null);
                var ivFace = view.FindViewById<ImageView>(Resource.Id.faceImage);
                var tv = view.FindViewById<TextView>(Resource.Id.tvWecomeEmp);
                var tvName = view.FindViewById<TextView>(Resource.Id.tvName);

                tvName.Text = "ÑîÉÜ½Ü";
                tv.Text = "»¶Ó­¹âÁÙ";
                ivFace.SetBackgroundResource(Resource.Drawable.son);

                //var builder = new AlertDialog.Builder(this);
                //builder.SetView(view);
                //var dialog = builder.Create();
                //dialog.Show();
                //dialog.Window.SetLayout(400, 600);

                //DisplayMetrics dm = new DisplayMetrics();
                //this.WindowManager.DefaultDisplay.GetRealMetrics(dm);
                //Toast.MakeText(this, "w=" + dm.WidthPixels + " h=" + dm.HeightPixels, ToastLength.Long).Show();
                //Task.Factory.StartNew(() =>
                //{
                //    Thread.Sleep(3000);
                //    dialog.Dismiss();
                //});
                ScaleAnimation sa = new ScaleAnimation(0, 2, 0, 2, 50f, 50f);
                sa.Interpolator = new AccelerateInterpolator();
                sa.FillAfter = true;
                sa.Duration = 1000;
                view.StartAnimation(sa);
            };
        }
    }
}