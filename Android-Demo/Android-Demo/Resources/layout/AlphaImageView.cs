using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Android_Demo.Resources.layout
{
    class AlphaImageView : ImageView
    {
        public AlphaImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        protected override void OnDraw(Canvas canvas)
        {
            //base.OnDraw(canvas);
            //canvas.DrawColor(Color.Yellow);
            //Paint p = new Paint
            //{
            //    TextSize = 40,
            //    AntiAlias = true,
            //    Color = Color.Blue
            //};
            //canvas.DrawText("—Ó…‹Ω‹", 1, 32, p);
        }
    }
}