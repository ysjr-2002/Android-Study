using Android.App;
using Android.Widget;
using Android.OS;
using System.Runtime.InteropServices;

namespace AppSO
{
    [Activity(Label = "AppSO", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            var btn = FindViewById<Button>(Resource.Id.button1);
            btn.Click += Btn_Click;
        }

        SerialPort sp = null;
        private void Btn_Click(object sender, System.EventArgs e)
        {
            sp = new SerialPort(14, 115200);
        }

        public static Java.Lang.String Bytes2HexString(byte[] b, int size)
        {
            Java.Lang.String ret = "";
            for (int i = 0; i < size; i++)
            {
                Java.Lang.String hex = Java.Lang.Integer.ToHexString(b[i] & 0xFF);
                if (hex.Length() == 1)
                {
                    hex = "0" + hex;
                }
                ret += hex.ToUpperCase();
            }
            return ret;
        }
    }
}

