using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Hardware.Display;
using Android.Media;
using System.IO;
using System.Threading.Tasks;
using Android.Net;
using FaceVisualExt.Code;
using Android.Graphics;
using System.Timers;
using Android.Views.Animations;

namespace FaceVisualExt
{
    [Activity(Label = "FaceVisualExt", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private TextView tvTime = null;
        private TextView tvWelcome = null;
        private HttpSocket socketMain = null;
        private HttpSocket socketSub = null;

        private View vistor = null;
        private TextView tv;
        private TextView tvName;
        private ImageView ivFace;

        private Timer timer = null;

        private bool forbidden = false;
        private DifferentDislay subDisplay = null;
        private Handler handler = new Handler();

        private const int POPUP_DIALOG_WIDTH = 500;
        private const int POPUP_DIALOG_HEIGHT = 700;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            this.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            SetContentView(Resource.Layout.front);

            tvWelcome = FindViewById<TextView>(Resource.Id.tvWelcome);
            tvTime = FindViewById<TextView>(Resource.Id.tvTime);
            vistor = this.FindViewById<LinearLayout>(Resource.Id.alter);

            ivFace = this.FindViewById<ImageView>(Resource.Id.faceImage);
            tv = this.FindViewById<TextView>(Resource.Id.tvWecomeEmp);
            tvName = this.FindViewById<TextView>(Resource.Id.tvName);

            ShowDisplay1();
        }

        private void ShowDisplay1()
        {
            DisplayManager displayManager;//屏幕管理类
            Display[] displays;//屏幕数组
            displayManager = (DisplayManager)this.GetSystemService(Context.DisplayService);
            displays = displayManager.GetDisplays();
            //主屏
            if (displays.Length >= 2)
            {
                //副屏
                var display1 = displays[1];
                subDisplay = new DifferentDislay(this, display1);
                subDisplay.Window.SetType(WindowManagerTypes.SystemAlert);
                subDisplay.Show();
            }
            else
            {
                return;
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            Config.ReadProfile();
            Start();
            Emit();
        }

        private async void Start()
        {
            var cfg = Config.Profile;

            tvWelcome.Text = cfg.Welcome1;
            Showtime();
            StartTimer();

            //socketMain = new HttpSocket(this);
            //socketMain.SetCallback(OnRecognizePersonMain);
            //var main = socketMain.Connect(cfg.ServerIp, cfg.CameraMain);

            //socketSub = new HttpSocket(this);
            //socketSub.SetCallback(OnRecognizePersonSub);
            //var sub = socketSub.Connect(cfg.ServerIp, cfg.CameraSub);
            //await Task.WhenAll(main, sub);
        }

        //前摄像机
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
            var faceImage = Tools.getFaceBitmap(url);
            ShowFace(name, faceImage);
            subDisplay?.Forbidden();
        }

        //副摄像机
        private void OnRecognizePersonSub(FaceRecognized entity)
        {
            forbidden = true;
        }

        private void ShowFace(string name, Bitmap faceImage)
        {
            handler.Post(() =>
            {
                var lp = vistor.LayoutParameters;
                lp.Width = POPUP_DIALOG_WIDTH;
                lp.Height = POPUP_DIALOG_HEIGHT;
                vistor.LayoutParameters = lp;
                tvName.Text = name;
                var color = this.Resources.GetColor(Resource.Color.face);
                tvName.SetTextColor(color);
                tv.Text = "欢迎光临阿里中心";
                ivFace.SetImageBitmap(faceImage);
                faceImage.Dispose();
                var sa = AnimationUtils.LoadAnimation(this, Resource.Animation.scale);
                sa.AnimationEnd += Sa_AnimationEnd;
                vistor.StartAnimation(sa);
            });
        }

        private void Sa_AnimationEnd(object sender, Animation.AnimationEndEventArgs e)
        {
            System.Threading.Thread.Sleep(Config.Profile.Delay);
            forbidden = false;
            var sa = AnimationUtils.LoadAnimation(this, Resource.Animation.translate);
            vistor.StartAnimation(sa);
        }

        private void StartTimer()
        {
            timer = new Timer();
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

        private void Emit()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    FaceRecognized fr = new FaceRecognized
                    {
                        person = new Employee
                        {
                            name = "朱某某",
                            avatar = "https://o7rv4xhdy.qnssl.com/@/static/upload/avatar/2017-03-30/fc993aacdaf43e3542ed0498eb2f8b24b7745034.jpg"
                        },
                    };
                    OnRecognizePersonMain(fr);
                    System.Threading.Thread.Sleep(10000);
                }
            });
        }
    }
}

