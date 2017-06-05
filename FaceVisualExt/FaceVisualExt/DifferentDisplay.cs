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

namespace FaceVisualExt
{
    public class DifferentDislay : Presentation
    {
        public DifferentDislay(Context outerContext, Display display) : base(outerContext, display)
        {
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.back);
            var btn = FindViewById<Button>(Resource.Id.good);
            btn.Click += delegate
            {
                Toast.MakeText(this.Context, "∏±∆¡œ‘ æ", ToastLength.Short).Show();
            };
        }
    }
}