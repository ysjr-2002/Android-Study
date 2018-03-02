using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using System;
using Android.Content.Res;
using System.Collections.Generic;

namespace AndroidStudy1
{
    [Activity(Label = "AndroidStudy1", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, AdapterView.IOnItemClickListener
    {
        Button button;
        string[] data = new string[] { "1", "2", "3", "4" };
        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            Toast.MakeText(ApplicationContext, data[position], ToastLength.Short).Show();
        }

        private void showToast(string content)
        {
            Toast.MakeText(this, content, ToastLength.Short).Show();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            button = FindViewById<Button>(Resource.Id.button);

            float xdpi = this.Resources.DisplayMetrics.Xdpi;
            float ydpi = this.Resources.DisplayMetrics.Ydpi;
            float density = this.Resources.DisplayMetrics.Density;
            //float scaleDensity = this.Resources.DisplayMetrics.DensityDpi;


            //ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, data);
            List<Chat> list = new List<Chat>();
            list.Add(new Chat("欢迎你", Chat.RECEIVE));
            list.Add(new Chat("你来自哪里", Chat.RECEIVE));
            list.Add(new Chat("你做什么工作", Chat.RECEIVE));
            list.Add(new Chat("你喜欢吃什么", Chat.RECEIVE));
            list.Add(new Chat("你好", Chat.SEND));
            list.Add(new Chat("我们都好", Chat.SEND));
            MyAdapter adapter = new MyAdapter(this, 0, list.ToArray());

            ListView listview = FindViewById<ListView>(Resource.Id.listview); ;
            listview.Adapter = adapter;

            listview.OnItemClickListener = this;

            Configuration configuration = this.Resources.Configuration;
            if (configuration.Orientation == Android.Content.Res.Orientation.Portrait)
            {
                showToast("竖屏");
            }
            else
            {
                showToast("横屏");
            }
        }
    }
}

