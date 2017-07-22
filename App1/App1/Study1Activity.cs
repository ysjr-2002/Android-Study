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
using Android.Graphics;
using System.Threading.Tasks;
using Android.Content.Res;

namespace App1
{
    [Activity(Label = "Study1Activity", MainLauncher = true)]
    public class Study1Activity : Activity
    {
        //https://github.com/avenwu/IndexImageView
        TextView _originalDimensions = null;
        ImageView iv = null;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            this.SetContentView(Resource.Layout.study1);

            _originalDimensions = FindViewById<TextView>(Resource.Id.size);
            iv = FindViewById<ImageView>(Resource.Id.imageView1);

            //BitmapFactory.Options options = new BitmapFactory.Options
            //{
            //    InJustDecodeBounds = true
            //};
            //options.InJustDecodeBounds = true;
            //options.InSampleSize = 4;
            //options.InJustDecodeBounds = false;

            //BitmapFactory.Options options = await GetBitmapOptionsOfImageAsync();
            //Bitmap bitmapToDisplay = await LoadScaledDownBitmapForDisplayAsync(Resources, options, 100, 100);
            //iv.SetImageBitmap(bitmapToDisplay);

            iv.SetImageURI(Android.Net.Uri.Parse("content://media/external/images/media/756"));

            var btn = FindViewById<Button>(Resource.Id.path);
            btn.Click += Btn_Click;
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(
                Intent.CreateChooser(imageIntent, "Select photo"), 0);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                var temp = data;
                iv.SetImageURI(data.Data);
            }
        }

        public async Task<Bitmap> LoadScaledDownBitmapForDisplayAsync(Resources res, BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Calculate inSampleSize
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

            // Decode bitmap with inSampleSize set
            options.InJustDecodeBounds = false;

            return await BitmapFactory.DecodeResourceAsync(res, Resource.Drawable.two, options);
        }

        async Task<BitmapFactory.Options> GetBitmapOptionsOfImageAsync()
        {
            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };

            // The result will be null because InJustDecodeBounds == true.
            Bitmap result = await BitmapFactory.DecodeResourceAsync(Resources, Resource.Drawable.two, options);

            int imageHeight = options.OutHeight;
            int imageWidth = options.OutWidth;

            _originalDimensions.Text = string.Format("Original Size= {0}x{1}", imageWidth, imageHeight);

            return options;
        }

        public static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Raw height and width of image
            float height = options.OutHeight;
            float width = options.OutWidth;
            double inSampleSize = 1d;

            if (height > reqHeight || width > reqWidth)
            {
                int halfHeight = (int)(height / 2);
                int halfWidth = (int)(width / 2);

                // Calculate a inSampleSize that is a power of 2 - the decoder will use a value that is a power of two anyway.
                while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth)
                {
                    inSampleSize *= 2;
                }
            }

            return (int)inSampleSize;
        }
    }
}