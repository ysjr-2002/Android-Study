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
    /// <summary>
    /// 自定义手势
    /// </summary>
    class CustomGestureDetector : GestureDetector.SimpleOnGestureListener
    {
        ViewFlipper viewFlipper = null;
        public CustomGestureDetector(ViewFlipper viewFlipper)
        {
            this.viewFlipper = viewFlipper;
        }

        public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            if (e1.GetX() > e2.GetX())
            {
                //如果初始触点的X坐标比最终触点的X坐标大表示向左滑动     
                viewFlipper.SetInAnimation(viewFlipper.Context, Resource.Animation.anim_slide_in_left);
                viewFlipper.SetOutAnimation(viewFlipper.Context, Resource.Animation.anim_slide_out_left);
                viewFlipper.ShowNext();
            }

            if (e1.GetX() < e2.GetX())
            {
                //如果初始触点的X坐标比最终触点的X坐标小表示向右滑动
                viewFlipper.SetInAnimation(viewFlipper.Context, Resource.Animation.anim_slide_in_right);
                viewFlipper.SetOutAnimation(viewFlipper.Context, Resource.Animation.anim_slide_out_right);
                viewFlipper.ShowPrevious();
            }

            return base.OnFling(e1, e2, velocityX, velocityY);
        }
    }
}