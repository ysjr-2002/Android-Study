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

namespace CameraApp4
{
    [Activity(Label = "MainActivity", MainLauncher = true)]
    public class MainActivity : Activity, ISurfaceHolderCallback, Camera.IShutterCallback, Camera.IPictureCallback
    {
        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Android.Graphics.Format format, int width, int height)
        {
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            //camera.SetPreviewDisplay(holder);
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
        private ImageView imageview;

        private WebSocket ws = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            this.SetContentView(Resource.Layout.Main);
            view = FindViewById<SurfaceView>(Resource.Id.surfaceView1);
            var btn = FindViewById<Button>(Resource.Id.MyButton);
            take = FindViewById<Button>(Resource.Id.take);
            imageview = FindViewById<ImageView>(Resource.Id.imageView1);
            btn.Click += Start;
            take.Click += Take_Click;

            ws = new WebSocket("ws://192.168.0.4:4649/Echo");
            ws.OnOpen += Ws_OnOpen;
            ws.OnClose += Ws_OnClose;
            ws.OnError += Ws_OnError;
            ws.OnMessage += Ws_OnMessage;
            ws.Connect();
        }

        void print(string msg)
        {
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            print("message");
            if (e.IsText)
            {
                print(e.Data);
                StringReader sr = new StringReader(e.Data);
                JsonValue jv = JsonObject.Load(sr);
                var cmd = jv["action"];
                if (cmd == "capture")
                {
                    this.RunOnUiThread(new Action(() =>
                   {
                       camera.TakePicture(this, this, this);
                   }));
                }
            }
        }

        private void Ws_OnError(object sender, ErrorEventArgs e)
        {
            print("error");
        }

        private void Ws_OnClose(object sender, CloseEventArgs e)
        {
            print("close");
        }

        private void Ws_OnOpen(object sender, EventArgs e)
        {
            print("open");
            Toast.MakeText(this, "webscoket connect", ToastLength.Short).Show();
        }

        private void button2_click(object sender, EventArgs e)
        {
            ws.Send("ni hao");
        }

        private void Take_Click(object sender, EventArgs e)
        {
            camera.TakePicture(this, this, this);
        }

        private void Camera_FaceDetection(object sender, Camera.FaceDetectionEventArgs e)
        {
            Toast.MakeText(this, "检测出人脸", ToastLength.Short).Show();
        }

        private void Start(object sender, EventArgs e)
        {
            holder = view.Holder;
            holder.AddCallback(this);

            camera = Android.Hardware.Camera.Open(1);

            Camera.Parameters p = camera.GetParameters();
            p.SetPreviewSize(320, 240);
            //p.SetPictureSize(320, 240);

            camera.SetDisplayOrientation(90);
            camera.SetParameters(p);
            camera.SetPreviewDisplay(holder);

            camera.StartPreview();
            camera.FaceDetection += Camera_FaceDetection;
        }

        public void OnShutter()
        {
        }

        public void OnPictureTaken(byte[] data, Camera camera)
        {
            if (data != null)
            {
                Toast.MakeText(this, "来了", ToastLength.Short).Show();
                //var _dir = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "CameraApp4");
                //if (!_dir.Exists())
                //{
                //    _dir.Mkdirs();
                //}

                //var _file = new File(_dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
                //Java.IO.FileOutputStream fs = new FileOutputStream(_file);
                //fs.Write(data);
                //fs.Close();

                var bitmap = Android.Graphics.BitmapFactory.DecodeByteArray(data, 0, data.Length);
                imageview.SetImageBitmap(bitmap);
                bitmap.Dispose();

                camera.StartPreview();
                try
                {
                    var str = Convert.ToBase64String(data);
                    ws.Send(str);
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
}