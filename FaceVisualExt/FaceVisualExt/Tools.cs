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
using System.Net;
using Android.Graphics;

namespace FaceVisualExt
{
    class Tools
    {
        public static byte[] DownImage(string url)
        {
            WebClient webclient = new WebClient();
            return webclient.DownloadData(url);
        }

        public static Bitmap getFaceBitmap(string url)
        {
            var data = DownImage(url);
            var bitmap = BitmapFactory.DecodeByteArray(data, 0, data.Length);
            return bitmap;
        }
    }
}