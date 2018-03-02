using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
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
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/Icon", MainLauncher = false)]
    public class FaceMainActivity : RootActivity
    {
        //威盛的板子 1080*1820
        private System.Timers.Timer timer = null;
        private TextView tvTime = null;
        private TextView tvWeek = null;
        private TextView tvWelcome = null;
        private HttpSocket socketMain = null;

        private const int stayInerval = 2000;

        private View vistor = null;
        private TextView tv;
        private TextView tvName;
        private ImageView ivFace;

        private const int p_width = 900;
        private const int p_height = 1200;

        private Handler handler = null;
        public static int connect_error = 1000;
        public static int connect_ok = 1001;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Config.ReadProfile();
            SetContentView(Resource.Layout.FaceMain);

            SetTimer();
            SetBackground();

            tvWelcome = FindViewById<TextView>(Resource.Id.tvWelcome);
            tvWeek = FindViewById<TextView>(Resource.Id.tvWeek);
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
                //var faceImage = getFaceBitmap("https://pic3.zhimg.com/v2-071d4282fb28c00ba03d418a7889f55e_r.jpg");
                //ShowFace("ysj", faceImage);
            };

            handler = new Handler((msg) =>
            {
                if (msg.What == connect_error)
                {
                    Toast.MakeText(this, "websocket连接失败", ToastLength.Short).Show();
                }
                if (msg.What == connect_ok)
                {
                    Toast.MakeText(this, "websocket连接成功", ToastLength.Short).Show();
                }
            });
        }

        private void SetTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
        }

        private void SetBackground()
        {
            if (string.IsNullOrEmpty(Config.Profile.BgUri) == true)
                return;

            var root = FindViewById<LinearLayout>(Resource.Id.mainroot);
            BitmapDrawable bd = new BitmapDrawable(Config.Profile.BgUri);
            root.Background = bd;
            bd.Dispose();
        }

        private void Sa_AnimationEnd(object sender, Animation.AnimationEndEventArgs e)
        {
            Thread.Sleep(Config.Profile.Delay);
            //forbidden = false;
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

            //Toast.MakeText(this, string.Format("Display:{0}*{1}", width, height), ToastLength.Long).Show();
            Start();
        }

        private async void Start()
        {
            var cfg = Config.Profile;

            tvWelcome.Text = cfg.Welcome1;
            Showtime();
            StartTimer();

            socketMain = new HttpSocket(handler, cfg.ServerIp, cfg.CameraMain, OnRecognizePersonMain);
            await socketMain.Connect();
        }

        protected override void OnPause()
        {
            base.OnPause();
            timer?.Stop();
            socketMain?.Disconnect();
        }

        private void OnRecognizePersonMain(FaceRecognized entity)
        {
            //if (forbidden)
            //    return;

            var avatar = "";
            if (entity.person.avatar.StartsWith("http"))
                avatar = entity.person.avatar;
            else
                avatar = "http://" + Config.Profile.ServerIp + entity.person.avatar;
            var name = entity.person.name;

            var faceImage = getFaceBitmap(avatar);
            ShowFace(name, faceImage);
        }

        //private bool forbidden = false;
        //private void OnRecognizePersonSub(FaceRecognized entity)
        //{
        //    forbidden = true;
        //    RunOnUiThread(() =>
        //    {
        //        var lp = vistor.LayoutParameters;
        //        lp.Width = p_width;
        //        lp.Height = p_height;
        //        vistor.LayoutParameters = lp;
        //        tv.Text = "";
        //        tvName.Text = "请稍等...";
        //        tvName.SetTextColor(Color.Red);
        //        ivFace.SetImageResource(Resource.Drawable.no);
        //        var sa = AnimationUtils.LoadAnimation(this, Resource.Animation.scale);
        //        sa.AnimationEnd += Sa_AnimationEnd;
        //        vistor.StartAnimation(sa);
        //    });
        //}

        private void ShowFace(string name, Bitmap faceImage)
        {
            RunOnUiThread(() =>
            {
                var lp = vistor.LayoutParameters;
                lp.Width = p_width;
                lp.Height = p_height;
                vistor.LayoutParameters = lp;
                tvName.Text = name;
                tvName.SetTextColor(Color.Rgb(255, 106, 00));
                tv.Text = Config.Profile.Welcome2;
                ivFace.SetImageBitmap(faceImage);
                faceImage.Dispose();
                //Picasso.With(this).Load(avatar).Into(ivFace);
                GC.Collect();
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
            timer.Start();
        }

        private void Showtime()
        {
            RunOnUiThread(new Action(() =>
            {
                int dayOfWeek = (int)DateTime.Now.DayOfWeek;
                tvWeek.Text = weeks[dayOfWeek];
                tvTime.Text = DateTime.Now.ToString("HH:mm:ss");
            }));
        }

        string[] weeks = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Showtime();
        }

        protected override void OnDestroy()
        {
            timer?.Stop();
            socketMain?.Disconnect();
            base.OnDestroy();
        }
    }
}