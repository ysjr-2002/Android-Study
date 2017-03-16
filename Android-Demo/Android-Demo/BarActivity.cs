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
            this.Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 31, 31, 31));
            // Create your application here            
            base.SetContentView(Resource.Layout.bar);
            var buttona = FindViewById<Button>(Resource.Id.buttona);
            var buttons = FindViewById<Button>(Resource.Id.buttons);
            var ivSon = FindViewById<ImageView>(Resource.Id.imageView1);
            buttona.Click += delegate
            {
                var animation = AnimationUtils.LoadAnimation(this, Resource.Animation.alpha);
                ivSon.StartAnimation(animation);
            };

            buttons.Click += delegate
            {
                var sa = AnimationUtils.LoadAnimation(this, Resource.Animation.scale);
                sa.SetAnimationListener(this);
                //ScaleAnimation sa = new ScaleAnimation(0, 2, 0, 2, 0.5f, 0.5f);
                //sa.FillAfter = true;
                //sa.Duration = 4000;
                ivSon.StartAnimation(sa);
            };
        }
    }
}