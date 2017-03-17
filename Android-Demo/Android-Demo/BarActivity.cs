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

        private LinearLayout vistor = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 0x33, 0xcc, 0xff));
            // Create your application here            
            base.SetContentView(Resource.Layout.bar);
            var buttona = FindViewById<Button>(Resource.Id.buttona);
            var buttons = FindViewById<Button>(Resource.Id.buttons);
            var buttond = FindViewById<Button>(Resource.Id.buttonalter);
            vistor = this.FindViewById<LinearLayout>(Resource.Id.alter);


            //var ivSon = FindViewById<ImageView>(Resource.Id.imageView1);
            buttona.Click += delegate
            {
                //var ah = AnimationUtils.LoadAnimation(this, Resource.Animation.alpha);
                var ah = new AlphaAnimation(0, 1.0f);
                //ivSon.StartAnimation(ah);
            };

            buttons.Click += delegate
            {
                var sa = AnimationUtils.LoadAnimation(this, Resource.Animation.scale);
                //ScaleAnimation sa = new ScaleAnimation(0, 2, 0, 2, Dimension.Absolute, ivSon.Width / 2f, Dimension.Absolute, ivSon.Height / 2f);
                //sa.Interpolator = new AccelerateInterpolator();
                //sa.FillAfter = true;
                //sa.Duration = 1000;
                //ivSon.StartAnimation(sa);
            };

            buttond.Click += delegate
            {
                var lp = vistor.LayoutParameters; //= new ViewGroup.LayoutParams(200, 500);
                lp.Width = 200;
                lp.Height = 500;
                vistor.LayoutParameters = lp;
                //LinearLayout.Inflate(this, Resource.Id.alter, null);
                var ivFace = this.FindViewById<ImageView>(Resource.Id.faceImage);
                var tv = this.FindViewById<TextView>(Resource.Id.tvWecomeEmp);
                var tvName = this.FindViewById<TextView>(Resource.Id.tvName);

                tvName.Text = "—Ó…‹Ω‹";
                tv.Text = "ª∂”≠π‚¡Ÿ";
                ivFace.SetBackgroundResource(Resource.Drawable.son);

                var sa = AnimationUtils.LoadAnimation(this, Resource.Animation.scale);
                vistor.StartAnimation(sa);
                //var x = this.FindViewById<RelativeLayout>(Resource.Id.rl);
                //var newp = x.LayoutParameters;
                //newp.Width = 200;
                //newp.Height = 200;
                //x.LayoutParameters = newp;
            };
        }
    }
}