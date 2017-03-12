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

namespace CameraApp4
{
    [Activity(Label = "MainActivity", MainLauncher = false)]
    public class MainActivity : Activity, ISurfaceHolderCallback, Camera.IShutterCallback, Camera.IPictureCallback, Camera.IAutoFocusCallback
    {
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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.camera_menu, menu);
            return true;
        }

        private SurfaceView view;
        private ISurfaceHolder holder;
        private Android.Hardware.Camera camera;

        private Button take;
        private Button open;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            RequestWindowFeature(WindowFeatures.NoTitle);
            this.SetContentView(Resource.Layout.Main);
            view = FindViewById<SurfaceView>(Resource.Id.surfaceView1);
            open = FindViewById<Button>(Resource.Id.open);
            take = FindViewById<Button>(Resource.Id.take);
            take.Click += Take_Click;
            open.Click += Open_Click;

            start();
            System.Threading.Tasks.Task t = new System.Threading.Tasks.Task(() =>
            {
                Thread.Sleep(2000);
                this.RunOnUiThread(new Action(() =>
                {
                    camera.TakePicture(this, this, this);
                }));
            });
            t.Start();
        }

        protected override void OnDestroy()
        {
            camera?.StopPreview();
            camera?.Release();
            camera = null;
            base.OnDestroy();
        }

        private void Open_Click(object sender, EventArgs e)
        {
            start();
        }

        private void Take_Click(object sender, EventArgs e)
        {
            //camera.AutoFocus(this);
            camera.TakePicture(this, this, this);
        }

        void Camera.IAutoFocusCallback.OnAutoFocus(bool success, Camera camera)
        {
            camera.TakePicture(this, this, this);
        }

        private void start()
        {
            holder = view.Holder;
            holder.AddCallback(this);

            camera = Android.Hardware.Camera.Open(1);
            Camera.Parameters p = camera.GetParameters();
            p.SetPreviewSize(320, 240);
            //p.SetPictureSize(320, 240);

            camera.SetDisplayOrientation(90);
            camera.SetParameters(p);
            //camera.SetPreviewDisplay(holder);
            MySocket.Current.OnPass += Current_OnPass;
        }

        private void Current_OnPass(string obj)
        {
            this.RunOnUiThread(() =>
            {
                //obj = "杨绍杰";
                var msg = JsonConvert.DeserializeObject<Message>(obj);
                Share.Message = msg;
                Intent intent = new Intent(this, typeof(DisplayActivity));
                //intent.PutExtra("msg", obj);
                StartActivity(intent);
                this.Finish();
            });
        }

        public void OnShutter()
        {
        }

        public void OnPictureTaken(byte[] data, Camera camera)
        {
            if (data != null)
            {
                //Toast.MakeText(this, "来了", ToastLength.Short).Show();
                //var _dir = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "CameraApp4");
                //if (!_dir.Exists())
                //{
                //    _dir.Mkdirs();
                //}

                //var _file = new File(_dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
                //Java.IO.FileOutputStream fs = new FileOutputStream(_file);
                //fs.Write(data);
                //fs.Close();

                //var bitmap = Android.Graphics.BitmapFactory.DecodeByteArray(data, 0, data.Length);
                //imageview.SetImageBitmap(bitmap);
                //bitmap.Dispose();

                camera.StartPreview();
                try
                {
                    var faceimage = Convert.ToBase64String(data);
                    Message msg = new Message
                    {
                        action = "androidface",
                        face = faceimage
                    };
                    var json = JsonConvert.SerializeObject(msg);
                    MySocket.Current.Send(json);
                }
                catch (Exception)
                {
                    Toast.MakeText(this, "出错了", ToastLength.Short).Show();
                }
                //ws.Send("消息太长???");
                //ws.Send(data);
            }
        }


    }

    class Message
    {
        public string action { get; set; }

        public string face { get; set; }

        public string name { get; set; }

        public string type { get; set; }
    }
}