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
using Java.IO;

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

                var _dir = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "CameraApp4");
                if (!_dir.Exists())
                {
                    _dir.Mkdirs();
                }

                var _file = new File(_dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
                //_file.CreateNewFile();
                Java.IO.FileOutputStream fs = new FileOutputStream(_file);
                fs.Write(data);
                fs.Close();

                var bitmap = Android.Graphics.BitmapFactory.DecodeByteArray(data, 0, data.Length);
                imageview.SetImageBitmap(bitmap);
            }
        }
    }
}