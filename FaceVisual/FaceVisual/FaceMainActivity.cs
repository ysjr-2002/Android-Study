using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
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
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/exit")]
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
                //lp.Width = 360;
                //lp.Height = 500;
                //vistor.LayoutParameters = lp;
                //tvName.Text = "ÑîÉÜ½Ü";
                //tv.Text = "»¶Ó­¹âÁÙ";
                //ivFace.SetImageResource(Resource.Drawable.face_ysj);
                //var sa = AnimationUtils.LoadAnimation(this, Resource.Animation.scale);
                //sa.AnimationEnd += Sa_AnimationEnd;
                //vistor.StartAnimation(sa);
            };
        }

        private void Sa_AnimationEnd(object sender, Animation.AnimationEndEventArgs e)
        {
            //Task.Factory.StartNew(() =>
            //{
            Thread.Sleep(1500);
            forbidden = false;
            //this.RunOnUiThread(new Action(() =>
            //{
            //    var lp = vistor.LayoutParameters;
            //    lp.Width = 0;
            //    lp.Height = 0;
            //    vistor.LayoutParameters = lp;
            //}));
            var sa = AnimationUtils.LoadAnimation(this, Resource.Animation.translate);
                vistor.StartAnimation(sa);
            //});
        }

        protected override void OnStart()
        {
            base.OnStart();
            Start();
        }

        private async void Start()
        {
            var cfg = Config.Profile;

            tvWelcome.Text = cfg.Welcome1;
            Showtime();
            StartTimer();

            socketMain = new HttpSocket();
            socketMain.SetCallback(OnRecognizePersonMain);
            var main = socketMain.Connect(cfg.ServerIp, cfg.CameraMain);
            await main;

            socketSub = new HttpSocket();
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
                lp.Width = 360;
                lp.Height = 500;
                vistor.LayoutParameters = lp;
                tv.Text = "";
                tvName.Text = "ÇëÉÔµÈ...";
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
                #region ¾É·½Ê½
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
                lp.Width = 360;
                lp.Height = 500;
                vistor.LayoutParameters = lp;
                tvName.Text = name;
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