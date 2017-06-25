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
using FaceVisualExt.Code;
using Android.Graphics;
using Android.Views.Animations;
using Android.Util;

namespace FaceVisualExt
{
    /// <summary>
    /// 副屏显示内容
    /// </summary>
    public class DifferentDislay : Presentation
    {
        private TextView tv;
        private View vistor;
        private TextView tvName;
        private TextView tvTime;
        private TextView tvWelcome;
        private ImageView ivFace;
        private const int POPUP_DIALOG_WIDTH = 500;
        private const int POPUP_DIALOG_HEIGHT = 700;
        private Handler handler = new Handler();
        private Color fontcolor = Color.White;
        public DifferentDislay(Context outerContext, Display display) : base(outerContext, display)
        {
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.front);

            tvWelcome = FindViewById<TextView>(Resource.Id.tvWelcome);
            tvTime = FindViewById<TextView>(Resource.Id.tvTime);
            vistor = this.FindViewById<LinearLayout>(Resource.Id.alter);

            ivFace = this.FindViewById<ImageView>(Resource.Id.faceImage);
            tv = this.FindViewById<TextView>(Resource.Id.tvWecomeEmp);
            tvName = this.FindViewById<TextView>(Resource.Id.tvName);

            fontcolor = this.Resources.GetColor(Resource.Color.face);
            tvName.SetTextColor(fontcolor);
        }

        protected override void OnStart()
        {
            base.OnStart();
            tvWelcome.Text = Config.Profile.Welcome2;
            DisplayMetrics dm = new DisplayMetrics();
            this.Window.WindowManager.DefaultDisplay.GetMetrics(dm);
            Toast.MakeText(this.Context, "Sub display=" + dm.WidthPixels + " " + dm.HeightPixels + " " + dm.DensityDpi, ToastLength.Long).Show();
        }

        public void UpdateTimer(string hms)
        {
            tvTime.Text = hms;
        }

        public void ShowFace(string name, Bitmap faceImage)
        {
            handler.Post(() =>
            {
                var lp = vistor.LayoutParameters;
                lp.Width = POPUP_DIALOG_WIDTH;
                lp.Height = POPUP_DIALOG_HEIGHT;
                vistor.LayoutParameters = lp;
                tvName.Text = name;
                tv.Text = Config.Profile.Welcome2;
                ivFace.SetImageBitmap(faceImage);
                faceImage.Dispose();
                StartShow();
            });
        }

        public void Forbidden()
        {
            handler.Post(() =>
            {
                var lp = vistor.LayoutParameters;
                lp.Width = POPUP_DIALOG_WIDTH;
                lp.Height = POPUP_DIALOG_HEIGHT;
                vistor.LayoutParameters = lp;
                tv.Text = string.Empty;
                tvName.Text = "请稍等";
                ivFace.SetImageResource(Resource.Drawable.no);
                StartShow();
            });
        }

        private void StartShow()
        {
            var sa = AnimationUtils.LoadAnimation(this.Context, Resource.Animation.scale);
            sa.AnimationEnd += Sa_AnimationEnd;
            vistor.StartAnimation(sa);
        }

        private void Sa_AnimationEnd(object sender, Animation.AnimationEndEventArgs e)
        {
            System.Threading.Thread.Sleep(Config.Profile.Delay);
            var sa = AnimationUtils.LoadAnimation(this.Context, Resource.Animation.translate);
            vistor.StartAnimation(sa);
        }
    }
}