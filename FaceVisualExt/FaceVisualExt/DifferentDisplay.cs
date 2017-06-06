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
        private const int p_width = 500;
        private const int p_height = 700;
        private Handler handler = new Handler();

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
        }


        public void ShowFace(string name, Bitmap faceImage)
        {
            handler.Post(() =>
            {
                var lp = vistor.LayoutParameters;
                lp.Width = p_width;
                lp.Height = p_height;
                vistor.LayoutParameters = lp;
                tvName.Text = name;
                tvName.SetTextColor(this.Resources.GetColor(Resource.Color.face));
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
                lp.Width = p_width;
                lp.Height = p_height;
                vistor.LayoutParameters = lp;
                tvName.Text = "请稍等";
                tvName.SetTextColor(Color.Rgb(255, 106, 00));
                tv.Text = Config.Profile.Welcome2;
                ivFace.SetImageResource(Resource.Drawable.no);
                StartShow();
            });
        }

        private void StartShow()
        {
            var sa = AnimationUtils.LoadAnimation(this.Context, Resource.Animation.scale);
            //sa.AnimationEnd += Sa_AnimationEnd;
            vistor.StartAnimation(sa);
        }
    }
}