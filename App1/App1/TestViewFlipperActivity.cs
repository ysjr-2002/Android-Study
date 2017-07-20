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
    [Activity(Label = "TestViewFlipperActivity", MainLauncher = false, Theme = "@android:style/Theme.Material.Light.NoActionBar.Fullscreen")]
    public class TestViewFlipperActivity : Activity
    {
        int[] resources = new int[3] { Resource.Drawable.son1, Resource.Drawable.son2, Resource.Drawable.son3 };
        ViewFlipper viewFlipper = null;
        GestureDetector gestureDetector = null;
        CustomGestureDetector customGestureDetector = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.flipper);
            // Create your application here
            viewFlipper = FindViewById<ViewFlipper>(Resource.Id.viewFlipper1);
            customGestureDetector = new CustomGestureDetector(viewFlipper);
            gestureDetector = new GestureDetector(customGestureDetector);
            for (int i = 0; i < resources.Length; i++)
            {
                ImageView imageView = new ImageView(this);
                imageView.SetImageResource(resources[i]);
                viewFlipper.AddView(imageView);
            }
            //viewFlipper.StartFlipping();
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            gestureDetector.OnTouchEvent(e);
            return base.OnTouchEvent(e);
        }
    }
}