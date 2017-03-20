using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using FaceVisual.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Android.Animation.Animator;

namespace FaceVisual
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = false, Icon = "@drawable/Icon")]
    public class FaceMainActivity : RootActivity
    {
        private System.Timers.Timer timer = null;
        private TextView tvTime = null;
        private TextView tvWelcome = null;
        private HttpSocket socketMain = null;
        private HttpSocket socketSub = null;

        private const int stayInerval = 2000;

        private View vistor = null;
        private TextView tv;
        private TextView tvName;
        private ImageView ivFace;

        private const int p_width = 500;
        private const int p_height = 700;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Config.ReadProfile();
            if (string.IsNullOrEmpty(Config.Profile.ServerIp))
            {
                StartActivity(typeof(SettingActivity));
                this.Finish();
                return;
            }
            SetContentView(Resource.Layout.FaceMain);
            tvWelcome = FindViewById<TextView>(Resource.Id.tvWelcome);
            tvTime = FindViewById<TextView>(Resource.Id.tvTime);
            vistor = this.FindViewById<LinearLayout>(Resource.Id.alter);

            ivFace = this.FindViewById<ImageView>(Resource.Id.faceImage);
            tv = this.FindViewById<TextView>(Resource.Id.tvWecomeEmp);
            tvName = this.FindViewById<TextView>(Resource.Id.tvName);

            var settingTextView = FindViewById<TextView>(Resource.Id.settingTextView);
            settingTextView.Click += delegate
            {
                Intent intent = new Intent(this, typeof(SettingActivity));
                StartActivity(intent);

                //var lp = vistor.LayoutParameters;
                //lp.Width = p_width;
                //lp.Height = p_height;
                //vistor.LayoutParameters = lp;
                //tvName.Text = "杨绍杰";
                //tv.Text = Config.Profile.Welcome2;
                //ivFace.SetImageResource(Resource.Drawable.no);
                //var sa = AnimationUtils.LoadAnimation(this, Resource.Animation.scale);
                //sa.AnimationEnd += Sa_AnimationEnd;
                //vistor.StartAnimation(sa);
            };
        }

        private void Sa_AnimationEnd(object sender, Animation.AnimationEndEventArgs e)
        {
            Thread.Sleep(Config.Profile.Delay);
            forbidden = false;
            var sa = AnimationUtils.LoadAnimation(this, Resource.Animation.translate);
            vistor.StartAnimation(sa);
        }

        protected override void OnStart()
        {
            base.OnStart();

            DisplayMetrics metric = new DisplayMetrics();
            this.WindowManager.DefaultDisplay.GetMetrics(metric);

            int width = metric.WidthPixels;  // 宽度（PX）
            int height = metric.HeightPixels;  // 高度（PX）

            float density = metric.Density;  // 密度（0.75 / 1.0 / 1.5）
            int densityDpi = (int)metric.DensityDpi;  // 密度DPI（120 / 160 / 240）

            //Toast.MakeText(this, width + " " + height + " " + density + " " + densityDpi, ToastLength.Long).Show();
            Start();
        }

        private async void Start()
        {
            var cfg = Config.Profile;

            tvWelcome.Text = cfg.Welcome1;
            Showtime();
            StartTimer();

            socketMain = new HttpSocket(this);
            socketMain.SetCallback(OnRecognizePersonMain);
            var main = socketMain.Connect(cfg.ServerIp, cfg.CameraMain);
            await main;

            socketSub = new HttpSocket(this);
            socketSub.SetCallback(OnRecognizePersonSub);
            var sub = socketSub.Connect(cfg.ServerIp, cfg.CameraSub);
            await sub;
        }

        private void OnRecognizePersonMain(FaceRecognized entity)
        {
            if (forbidden)
                return;

            var url = "";
            if (entity.person.avatar.StartsWith("http"))
                url = entity.person.avatar;
            else
                url = "http://" + Config.Profile.ServerIp + entity.person.avatar;
            var name = entity.person.name;
            var faceImage = getFaceBitmap(url);
            ShowFace(name, faceImage);
        }

        private bool forbidden = false;
        private void OnRecognizePersonSub(FaceRecognized entity)
        {
            forbidden = true;
            RunOnUiThread(() =>
            {
                //var view = LinearLayout.Inflate(this, Resource.Layout.no, null);
                //var builder = new AlertDialog.Builder(this);
                //builder.SetView(view);
                //var dialog = builder.Create();
                //dialog.Show();
                //Task.Factory.StartNew(() =>
                //{
                //    Thread.Sleep(Config.Profile.Delay);
                //    dialog.Dismiss();
                //    forbidden = false;
                //});

                var lp = vistor.LayoutParameters;
                lp.Width = p_width;
                lp.Height = p_height;
                vistor.LayoutParameters = lp;
                tv.Text = "";
                tvName.Text = "请稍等...";
                tvName.SetTextColor(Color.Red);
                ivFace.SetImageResource(Resource.Drawable.no);
                var sa = AnimationUtils.LoadAnimation(this, Resource.Animation.scale);
                sa.AnimationEnd += Sa_AnimationEnd;
                vistor.StartAnimation(sa);
            });
        }

        private void ShowFace(string name, Bitmap faceImage)
        {
            RunOnUiThread(() =>
            {
                #region 旧方式
                //var view = LinearLayout.Inflate(this, Resource.Layout.visitor, null);
                //var ivFace = view.FindViewById<ImageView>(Resource.Id.faceImage);
                //var tv = view.FindViewById<TextView>(Resource.Id.tvWecomeEmp);
                //var tvName = view.FindViewById<TextView>(Resource.Id.tvName);
                //tvName.Text = name;
                //tv.Text = Config.Profile.Welcome2;
                //ivFace.SetImageBitmap(faceImage);
                //faceImage.Dispose();
                //var builder = new AlertDialog.Builder(this);
                //builder.SetView(view);
                //var dialog = builder.Create();
                //dialog.Show();
                //Task.Factory.StartNew(() =>
                //{
                //    Thread.Sleep(stayInerval);
                //    faceImage?.Dispose();
                //    dialog.Dismiss();
                //}); 
                #endregion

                var lp = vistor.LayoutParameters;
                lp.Width = p_width;
                lp.Height = p_height;
                vistor.LayoutParameters = lp;
                tvName.Text = name;
                tvName.SetTextColor(Color.Rgb(255, 106, 00));
                tv.Text = Config.Profile.Welcome2;
                ivFace.SetImageBitmap(faceImage);
                faceImage.Dispose();
                var sa = AnimationUtils.LoadAnimation(this, Resource.Animation.scale);
                sa.AnimationEnd += Sa_AnimationEnd;
                vistor.StartAnimation(sa);
            });
        }

        private byte[] DownImage(string url)
        {
            WebClient webclient = new WebClient();
            return webclient.DownloadData(url);
        }

        private Bitmap getFaceBitmap(string url)
        {
            var data = DownImage(url);
            var bitmap = BitmapFactory.DecodeByteArray(data, 0, data.Length);
            return bitmap;
        }

        private void StartTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Start();
            timer.Elapsed += Timer_Elapsed;
        }

        private void Showtime()
        {
            RunOnUiThread(new Action(() =>
            {
                tvTime.Text = DateTime.Now.ToString("HH:mm:ss");
            }));
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Showtime();
        }

        protected override void OnDestroy()
        {
            timer?.Stop();
            socketMain?.Close();
            socketSub?.Close();
            base.OnDestroy();
        }
    }
}