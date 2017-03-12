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
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using Android.Graphics;

namespace CameraApp4
{
    [Activity(Label = "DisplayActivity")]
    public class DisplayActivity : Activity
    {
        private Matrix matrix = new Matrix();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            // Create your application here
            SetContentView(Resource.Layout.display);
            //var  json = Intent.GetStringExtra("msg");
            //var msg = JsonConvert.DeserializeObject<Message>(json);

            var msg = Share.Message;
            var name = FindViewById<TextView>(Resource.Id.personName);
            name.Text = msg.name;

            var imageview = FindViewById<ImageView>(Resource.Id.personFace);
            var data = Convert.FromBase64String(msg.face);
            //var bitmap = Android.Graphics.BitmapFactory.DecodeByteArray(data, 0, data.Length);
            //matrix.SetRotate(90);
            var bitmap = LoadAndResizeBitmap(data, 400, 300);
            //bitmap = scaleBitmap(bitmap, 90);
            imageview.SetImageBitmap(bitmap);
            bitmap.Dispose();

            Task task = new Task(() =>
            {
                Thread.Sleep(2000);
                this.RunOnUiThread(new Action(() =>
                {
                    Intent t = new Intent(this, typeof(InitActivity));
                    StartActivity(t);
                    this.Finish();
                }));
            });
            task.Start();
        }

        public static Bitmap LoadAndResizeBitmap(byte[] data, int width, int height)
        {
            // First we get the the dimensions of the file on disk
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeByteArray(data, 0, data.Length, options);

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                                   ? outHeight / height
                                   : outWidth / width;
            }

            // Now we will load the image and have BitmapFactory resize it for us.
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Bitmap resizedBitmap = BitmapFactory.DecodeByteArray(data, 0, data.Length, options);
            Matrix m = new Matrix();
            m.SetRotate(270);
            var newm = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, m, true);
            resizedBitmap.Dispose();
            return newm;
        }
    }
}