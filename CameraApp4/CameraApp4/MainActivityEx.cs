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
using Android.Hardware;
using WebSocketSharp;
using System.Json;
using System.IO;
using Newtonsoft.Json;
using System.Threading;
using Java.IO;
using System.Threading.Tasks;

namespace CameraApp4
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = false, Icon = "@drawable/Icon")]
    public class MainActivityEx : Activity, ISurfaceHolderCallback, Camera.IShutterCallback, Camera.IPictureCallback, Camera.IPreviewCallback
    {
        private SurfaceView view;
        private SVDraw viewtop;
        private ISurfaceHolder holder;
        private Android.Hardware.Camera camera;

        private bool bCapture = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            SetContentView(Resource.Layout.mainnew);

            Config.ReadProfile();
            if (string.IsNullOrEmpty(Config.Profile.ServerIp))
            {
                StartActivity(typeof(ConfigActivity));
                this.Finish();
                return;
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            connect();
        }

        private void connect()
        {
            MySocket.Current.Init(Config.Profile.ServerIp);
            MySocket.Current.OnCaputure += Current_OnCaputure;
            MySocket.Current.OnPass += Current_OnPass;
            view = FindViewById<SurfaceView>(Resource.Id.sv_camera);
            viewtop = FindViewById<SVDraw>(Resource.Id.sv_top);
            start();
        }

        private void start()
        {
            holder = view.Holder;
            holder.AddCallback(this);

            camera = Android.Hardware.Camera.Open(1);
            Android.Hardware.Camera.Parameters p = camera.GetParameters();
            p.SetPreviewSize(320, 240);

            camera.SetDisplayOrientation(90);
            camera.SetParameters(p);
            camera.SetPreviewCallback(this);
        }

        private void Current_OnCaputure()
        {
            bCapture = true;
        }

        private void Current_OnPass(string obj)
        {
            this.RunOnUiThread(() =>
            {
                var message = JsonConvert.DeserializeObject<Message>(obj);
                var view = LinearLayout.Inflate(this, Resource.Layout.visitor, null);

                var ivFace = view.FindViewById<ImageView>(Resource.Id.iv_face);
                var tvWeclome = view.FindViewById<TextView>(Resource.Id.tv_weclome);
                var tvName = view.FindViewById<TextView>(Resource.Id.tv_name);

                var data = Convert.FromBase64String(message.face);
                var faceImage = Android.Graphics.BitmapFactory.DecodeByteArray(data, 0, data.Length);

                tvName.Text = message.name;
                if (message.type == "ok")
                    tvWeclome.Text = "欢迎光临";
                else
                {
                    tvWeclome.Text = "比对失败，请重新刷卡";
                }

                ivFace.SetImageBitmap(faceImage);
                faceImage.Dispose();

                var builder = new AlertDialog.Builder(this);
                builder.SetView(view);
                var dialog = builder.Create();

                dialog.Window.SetGravity(GravityFlags.Top);
                dialog.Show();
                dialog.Window.SetLayout(360, 500);

                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(Config.Profile.Delay);
                    faceImage?.Dispose();
                    dialog.Dismiss();
                });
            });
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Android.Graphics.Format format, int width, int height)
        {
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            camera.SetPreviewDisplay(holder);
            camera.StartPreview();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
        }

        public void OnShutter()
        {
        }

        public void OnPictureTaken(byte[] data, Camera camera)
        {
            #region TakePicture
            //if (data != null)
            //{
            //    camera.StartPreview();
            //    try
            //    {
            //        var faceimage = Convert.ToBase64String(data);
            //        Message msg = new Message
            //        {
            //            action = "androidface",
            //            face = faceimage
            //        };
            //        var json = JsonConvert.SerializeObject(msg);
            //        MySocket.Current.Send(json);
            //    }
            //    catch (Exception)
            //    {
            //        Toast.MakeText(this, "出错了", ToastLength.Short).Show();
            //    }
            //} 
            #endregion
        }

        public void OnPreviewFrame(byte[] data, Android.Hardware.Camera camera)
        {
            Camera.Size size = camera.GetParameters().PreviewSize;
            if (!bCapture)
                return;

            Android.Graphics.YuvImage image = new Android.Graphics.YuvImage(data, Android.Graphics.ImageFormatType.Nv21, size.Width, size.Height, null);
            if (image != null)
            {
                MemoryStream stream = new MemoryStream();
                image.CompressToJpeg(new Android.Graphics.Rect(0, 0, size.Width, size.Height), 80, stream);
                Android.Graphics.Bitmap bmp = Android.Graphics.BitmapFactory.DecodeByteArray(stream.ToArray(), 0, (int)stream.Length);
                stream.Close();

                //因为图片会放生旋转，因此要对图片进行旋转到和手机在一个方向上
                var newbmp = rotateMyBitmap(bmp);

                MemoryStream rotateStream = new MemoryStream();
                newbmp.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 90, rotateStream);
                byte[] bitmapData = rotateStream.ToArray();

                var faceimage = Convert.ToBase64String(bitmapData);
                Message msg = new Message
                {
                    action = "androidface",
                    face = faceimage
                };
                var json = JsonConvert.SerializeObject(msg);
                MySocket.Current.Send(json);
            }
            else
            {
                Toast.MakeText(this, "抓图失败", ToastLength.Short).Show();
            }
            bCapture = false;
        }

        public Android.Graphics.Bitmap rotateMyBitmap(Android.Graphics.Bitmap bmp)
        {
            //*****旋转一下
            Android.Graphics.Matrix matrix = new Android.Graphics.Matrix();
            matrix.PostRotate(270);
            Android.Graphics.Bitmap nbmp2 = Android.Graphics.Bitmap.CreateBitmap(bmp, 0, 0, bmp.Width, bmp.Height, matrix, true);
            bmp.Dispose();
            return nbmp2;
        }


    }
}