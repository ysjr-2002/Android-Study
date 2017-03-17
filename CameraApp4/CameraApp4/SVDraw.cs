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
using static Android.Graphics.Paint;

namespace CameraApp4
{
    class SVDraw : SurfaceView, ISurfaceHolderCallback
    {
        private ISurfaceHolder sh;
        private int width;
        private int height;
        private int middle;
        private Canvas canvas = null;
        private int rectWidthhalf = 240;
        private int rectHeight = 460;
        private const int top = 300;
        private const int len = 50;
        private const int textRectHeight = 80;
        private const string tip = "请将面部置于区域内";

        private static Color txtColor = Color.Goldenrod;
        private static Color rectColor = Color.Goldenrod;
        public SVDraw(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            sh = Holder;
            sh.AddCallback(this);
            SetZOrderOnTop(true);
            Holder.SetFormat(Format.Transparent);
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {
            this.width = width;
            this.height = height;
            this.middle = this.width / 2;
            drawLine();
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            canvas = holder.LockCanvas();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
        }

        void clearDraw()
        {
            Canvas canvas = sh.LockCanvas();
            canvas.DrawColor(Color.Blue);
            sh.UnlockCanvasAndPost(canvas);
        }

        public void drawLine()
        {
            if (canvas != null)
            {
                //canvas.DrawColor(Color.Transparent);
                canvas.DrawColor(Color.Argb(160, 166, 166, 166));

                Rect targetRect = new Rect(0, top - textRectHeight, width, textRectHeight);
                Paint paint = new Paint(PaintFlags.AntiAlias);
                paint.StrokeWidth = 2;
                paint.TextSize = 36;
                //canvas.DrawRect(targetRect, paint);
                paint.Color = txtColor;
                FontMetricsInt fontMetrics = paint.GetFontMetricsInt();
                int baseline = (targetRect.Bottom + targetRect.Top - fontMetrics.Bottom - fontMetrics.Top) / 2 + 80;
                paint.TextAlign = Align.Center;
                canvas.DrawText(tip, targetRect.CenterX(), baseline, paint);

                Paint linePaint = new Paint();
                linePaint.AntiAlias = true;
                linePaint.Color = rectColor;
                linePaint.StrokeWidth = 5;
                linePaint.SetStyle(Style.Stroke);
                //左上
                int lefttop_x = middle - rectWidthhalf;
                canvas.DrawLine(lefttop_x, top, lefttop_x + len, top, linePaint);
                canvas.DrawLine(lefttop_x, top, lefttop_x, top + len, linePaint);

                //右上
                int righttop_x = middle + rectWidthhalf;
                canvas.DrawLine(righttop_x, top, righttop_x - len, top, linePaint);
                canvas.DrawLine(righttop_x, top, righttop_x, top + len, linePaint);

                //左下
                int leftbottom_x = lefttop_x;
                int leftbottom_y = top + rectHeight;
                canvas.DrawLine(leftbottom_x, leftbottom_y, leftbottom_x + len, leftbottom_y, linePaint);
                canvas.DrawLine(leftbottom_x, leftbottom_y, leftbottom_x, leftbottom_y - len, linePaint);

                //右下
                int rightbottom_x = middle + rectWidthhalf;
                int rightbottom_y = top + rectHeight;
                canvas.DrawLine(rightbottom_x, rightbottom_y, rightbottom_x - len, rightbottom_y, linePaint);
                canvas.DrawLine(rightbottom_x, rightbottom_y, rightbottom_x, rightbottom_y - len, linePaint);

                //清屏
                Paint clearPaint = new Paint();
                clearPaint.AntiAlias = true;
                clearPaint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.Clear));
                canvas.DrawRect(new Rect { Left = lefttop_x, Top = top, Right = rightbottom_x, Bottom = rightbottom_y }, clearPaint);

                sh.UnlockCanvasAndPost(canvas);
            }
        }
    }
}