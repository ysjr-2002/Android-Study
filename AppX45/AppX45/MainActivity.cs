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
using Java.Lang;

namespace AppX45
{
    [Activity(Label = "MainActivity", MainLauncher = false)]
    public class MainActivity : Activity
    {
        ysj[] items;
        ListView lv;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            this.SetContentView(Resource.Layout.Main);
            //lv = FindViewById<ListView>(Resource.Id.deviceList);
            //ActionBar.SetTitle(Resource.String.tt);
            //items = new ysj[100];
            //for (int i = 0; i < items.Length; i++)
            //{
            //    items[i] = new ysj { name = i.ToString(), age = i.ToString("d2") };
            //}
            //var adapter = new HomeScreenAdapter(this, items);
            //lv.Adapter = adapter;
            //lv.ItemClick += Lv_ItemClick;
            //lv.FastScrollEnabled = true;
            //lv.ChoiceMode = ChoiceMode.Single;
        }

        private void Lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var pos = e.Position;
            var item = items[pos];
            Toast.MakeText(this, item.name + " " + item.age, ToastLength.Short).Show();
        }
    }

    public class ysj
    {
        public string name;
        public string age;
    }
}