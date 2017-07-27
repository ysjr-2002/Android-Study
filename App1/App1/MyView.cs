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
using Android.Graphics.Drawables;
using Android.Content.Res;
using static Android.Graphics.Shader;

namespace App1
{
    public class RoundImageView : ImageView
    {
        /** 
        * 图片的类型，圆形or圆角 
        */
        private int type;
        private static int TYPE_CIRCLE = 0;
        private static int TYPE_ROUND = 1;
        /** 
         * 圆角大小的默认值 
         */
        private static int BODER_RADIUS_DEFAULT = 10;
        /** 
         * 圆角的大小 
         */
        private int mBorderRadius;

        /** 
         * 绘图的Paint 
         */
        private Paint mBitmapPaint;
        /** 
         * 圆角的半径 
         */
        private int mRadius;
        /** 
         * 3x3 矩阵，主要用于缩小放大 
         */
        private Matrix mMatrix;
        /** 
         * 渲染图像，使用图像为绘制图形着色 
         */
        private BitmapShader mBitmapShader;
        /** 
         * view的宽度 
         */
        private int mWidth;
        private RectF mRoundRect;

        public RoundImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            mMatrix = new Matrix();
            mBitmapPaint = new Paint(PaintFlags.AntiAlias);

            //TypedArray a = context.ObtainStyledAttributes(attrs,
            //        Resource.Styleable.RoundImageView);

            //var defValue = (int)TypedValue
            //                .ApplyDimension(ComplexUnitType.Dip,
            //                        BODER_RADIUS_DEFAULT, this.Resources.DisplayMetrics);

            //mBorderRadius = a.GetDimensionPixelSize(
            //        Resource.Styleable.RoundImageView_borderRadius, defValue);// 默认为10dp  

            //type = a.GetInt(Resource.Styleable.RoundImageView_type, TYPE_CIRCLE);// 默认为Circle  


            //a.Recycle();

            var shit = Resource.Styleable.shit;

            TypedArray temp = context.ObtainStyledAttributes(attrs, shit);
            int n = temp.IndexCount;
            for (int i = 0; i < n; i++)
            {
                //int attr = a.getIndex(i);
                //switch (attr)
                //{
                //    case R.styleable.CustomTitleView_titleText:
                //        mTitleText = a.getString(attr);
                //        break;
                //    case R.styleable.CustomTitleView_titleTextColor:
                //        // 默认颜色设置为黑色  
                //        mTitleTextColor = a.getColor(attr, Color.BLACK);
                //        break;
                //    case R.styleable.CustomTitleView_titleTextSize:
                //        // 默认设置为16sp，TypeValue也可以把sp转化为px  
                //        mTitleTextSize = a.getDimensionPixelSize(attr, (int)TypedValue.applyDimension(
                //                TypedValue.COMPLEX_UNIT_SP, 16, getResources().getDisplayMetrics()));
                //        break;
                //}
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            if (type == TYPE_CIRCLE)
            {
                mWidth = Math.Min(MeasuredWidth, MeasuredHeight);
                mRadius = mWidth / 2;
                SetMeasuredDimension(mWidth, mWidth);
            }
        }

        private void setUpShader()
        {
            Drawable drawable = Drawable;
            if (drawable == null)
            {
                return;
            }

            Bitmap bmp = getBitmap(drawable);
            // 将bmp作为着色器，就是在指定区域内绘制bmp  
            mBitmapShader = new BitmapShader(bmp, TileMode.Clamp, TileMode.Clamp);
            float scale = 1.0f;
            if (type == TYPE_CIRCLE)
            {
                // 拿到bitmap宽或高的小值  
                int bSize = Math.Min(bmp.Width, bmp.Height);
                scale = mWidth * 1.0f / bSize;

            }
            else if (type == TYPE_ROUND)
            {
                // 如果图片的宽或者高与view的宽高不匹配，计算出需要缩放的比例；缩放后的图片的宽高，一定要大于我们view的宽高；所以我们这里取大值；  
                scale = Math.Max(Width * 1.0f / bmp.Width, Height
                        * 1.0f / bmp.Height);
            }
            // shader的变换矩阵，我们这里主要用于放大或者缩小  
            mMatrix.SetScale(scale, scale);
            // 设置变换矩阵  
            mBitmapShader.SetLocalMatrix(mMatrix);
            // 设置shader  
            mBitmapPaint.SetShader(mBitmapShader);
        }

        protected override void OnDraw(Canvas canvas)
        {
            if (Drawable == null)
            {
                return;
            }
            setUpShader();
            if (type == TYPE_ROUND)
            {
                canvas.DrawRoundRect(mRoundRect, mBorderRadius, mBorderRadius,
                        mBitmapPaint);
            }
            else
            {
                canvas.DrawCircle(mRadius, mRadius, mRadius, mBitmapPaint);
            }
            //base.OnDraw(canvas);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            // 圆角图片的范围  
            if (type == TYPE_ROUND)
                mRoundRect = new RectF(0, 0, Width, Height);
        }

        private Bitmap getBitmap(Drawable drawable)
        {
            if (drawable is BitmapDrawable)
            {
                return ((BitmapDrawable)drawable).Bitmap;
            }
            else
            {
                return null;
            }
        }
    }
}