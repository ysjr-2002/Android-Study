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

        protected void onCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.back);
        }
    }
}