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

namespace AutoUpgrade
{
    class SimpleTabFragement : Fragment
    {
        private string content = "";
        public SimpleTabFragement(string content)
        {
            this.content = content;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Tab, container, false);
            var textview = view.FindViewById<TextView>(Resource.Id.sampleTextView);
            textview.Text = content;

            return view;
        }
    }
}