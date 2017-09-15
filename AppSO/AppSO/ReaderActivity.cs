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
using com.pci.pca.readcard;
using Com.Handheld.UHFLonger;

namespace AppSO
{
    [Activity(Label = "身份证", MainLauncher = true, Icon = "@drawable/icon")]
    public class ReaderActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            base.SetContentView(Resource.Layout.Reader);

            var btnOpen = FindViewById<Button>(Resource.Id.btnOpen);
            var btnClose = FindViewById<Button>(Resource.Id.btnClose);

            btnOpen.Click += BtnOpen_Click;
            btnClose.Click += BtnClose_Click;
        }

        SerialPort serial = null;
        private int seriaPort = 14;
        private int baudrate = 115200;

        UHFLongerManager manager = null;
        private void BtnOpen_Click(object sender, EventArgs e)
        {
            serial = new SerialPort();
            serial.psam_poweron();
            serial = new SerialPort(seriaPort, baudrate, 0);

            //manager = UHFLongerManager.Instance;
            //var set = manager.SetOutPower(500);

        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
        }
    }
}