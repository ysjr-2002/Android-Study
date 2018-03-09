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
        //��ʢ�İ��� 1080*1820
        private System.Timers.Timer timer = null;
        private TextView tvTime = null;
        private TextView tvWeek = null;
        private TextView tvWelcome = null;
        private HttpSocket socketMain = null;

        private const int stayInerval = 2000;

        private View vistor = null;
        private TextView tvWelcomeEmp;
        private TextView tvName;
        private DE.Hdodenhof.CircleImageView.CircleImageView face;

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

            face = this.FindViewById<DE.Hdodenhof.CircleImageView.CircleImageView>(Resource.Id.abc);
            tvWelcomeEmp = this.FindViewById<TextView>(Resource.Id.tvWecomeEmp);
            tvName = this.FindViewById<TextView>(Resource.Id.tvName);

            var settingTextView = FindViewById<TextView>(Resource.Id.settingTextView);
            settingTextView.Click += delegate
            {
                Intent intent = new Intent(this, typeof(SettingActivity));
                StartActivity(intent);
                this.Finish();
            };

            handler = new Handler((msg) =>
            {
                if (msg.What == connect_error)
                {
                    showToast("websocket����ʧ��");
                }
                if (msg.What == connect_ok)
                {
                    showToast("websocket���ӳɹ�");
                }
            });
        }

        private void showToast(string msg)
        {
            var toast = Toast.MakeText(this, msg, ToastLength.Short);
            toast.SetGravity(GravityFlags.Center, 0, 0);
            toast.Show();
        }

        private void SetTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
        }

        private async void test()
        {
            await Task.Delay(1000);
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
            Thread.Sleep(Config.Profile.Delay * 10);

            this.RunOnUiThread(new Action(() =>
            {
                var sa = AnimationUtils.LoadAnimation(this, Resource.Animation.translate);
                vistor.StartAnimation(sa);
            }));
        }

        protected override void OnStart()
        {
            base.OnStart();

            var a = this.Resources.DisplayMetrics.Density;
            var b = this.Resources.DisplayMetrics.DensityDpi;
            var c = this.Resources.DisplayMetrics.HeightPixels;
            var d = this.Resources.DisplayMetrics.WidthPixels;
            var e = this.Resources.DisplayMetrics.Xdpi;
            var f = this.Resources.DisplayMetrics.Ydpi;

            DisplayMetrics metric = new DisplayMetrics();
            this.WindowManager.DefaultDisplay.GetMetrics(metric);

            int width = metric.WidthPixels;  // ��ȣ�PX��
            int height = metric.HeightPixels;  // �߶ȣ�PX��

            float density = metric.Density;  // �ܶȣ�0.75 / 1.0 / 1.5 / 2.0��
            int densityDpi = (int)metric.DensityDpi;  // �ܶ�DPI��120 / 160 / 240��

            //Toast.MakeText(this, string.Format("Display:{0}*{1}", width, height), ToastLength.Long).Show();
            Start();
        }

        private int dip2px(Context ctx, float dpValue)
        {
            return (int)(dpValue * ctx.Resources.DisplayMetrics.Density + 0.5f);
        }

        private async void Start()
        {
            var cfg = Config.Profile;

            tvWelcome.Text = cfg.Welcome1;
            tvWelcomeEmp.Text = cfg.Welcome2;
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
            var avatar = "";
            if (entity.person.avatar.StartsWith("http"))
                avatar = entity.person.avatar;
            else
                avatar = "http://" + Config.Profile.ServerIp + entity.person.avatar;
            var name = entity.person.name;

            if(entity.person.subject_type == 0)
            {
                //Ա��
            }
            else if( entity.person.subject_type ==1)
            {
                //�ÿ�
            }
            else if( entity.person.subject_type == 2)
            {
                //VIP
            }

            var faceImage = getFaceBitmap(avatar);
            ShowFace(name, faceImage);
        }

        private void ShowFace(string name, Bitmap faceImage)
        {
            RunOnUiThread(() =>
            {
                tvName.Text = name;

                face.SetImageBitmap(faceImage);
                faceImage.Recycle();
                faceImage = null;
                GC.Collect();
                var animation = AnimationUtils.LoadAnimation(this, Resource.Animation.scale);
                animation.AnimationEnd += Sa_AnimationEnd;
                vistor.StartAnimation(animation);
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

        string[] weeks = { "������", "����һ", "���ڶ�", "������", "������", "������", "������" };
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