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

namespace CameraApp4
{
    [Activity(Label = "@string/setting")]
    public class ConfigActivity : Activity
    {
        private EditText tvserver;
        private EditText tvdelay;
        private Button button;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.config);

            tvserver = FindViewById<EditText>(Resource.Id.tvServer);
            tvdelay = FindViewById<EditText>(Resource.Id.tvDelay);
            button = FindViewById<Button>(Resource.Id.btSave);
            button.Click += delegate
            {
                Save();
            };

            tvserver.Text = Config.Profile.ServerIp;
            tvdelay.Text = Config.Profile.Delay.ToString();
        }

        private void Save()
        {
            var server = tvserver.Text;
            var delay = tvdelay.Text;

            if (string.IsNullOrEmpty(server))
            {
                var hint = Resources.GetString(Resource.String.serverhint);
                toast(hint);
                return;
            }

            var n = getNumber(delay);

            if (n < 1000 || n > 5000)
            {
                toast("Please input valid time range[1000~5000]");
                return;
            }

            Config.Profile.ServerIp = server;
            Config.Profile.Delay = n;
            Config.SaveProfile();

            StartActivity(typeof(MainActivityEx));
            Finish();
        }

        private void toast(string msg)
        {
            var toast = Toast.MakeText(this, msg, ToastLength.Short);
            toast.SetGravity(GravityFlags.CenterHorizontal, 0, 0);
            toast.Show();
        }

        private int getNumber(string s)
        {
            int i = 0;
            Int32.TryParse(s, out i);
            return i;
        }
    }
}