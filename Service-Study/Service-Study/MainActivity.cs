using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Service_Study
{
    [Activity(Label = "Service_Study", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        bool isServiceRunning = false;
        Button startButton;
        Button stopButton;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            startButton = FindViewById<Button>(Resource.Id.startService);
            startButton.Click += Start_Click;

            stopButton = FindViewById<Button>(Resource.Id.stopService);
            stopButton.Click += Stop_Click;
            stopButton.Enabled = false;
        }

        protected override void OnPause()
        {
            // Clean up: shut down the service when the Activity is no longer visible.
            StopService(new Intent(this, typeof(SimpleStartedService)));
            base.OnPause();
        }

        void Start_Click(object sender, System.EventArgs e)
        {
            StartService(new Intent(this, typeof(SimpleStartedService)));
            isServiceRunning = true;
            startButton.Enabled = false;
            stopButton.Enabled = true;
        }


        void Stop_Click(object sender, System.EventArgs e)
        {
            StopService(new Intent(this, typeof(SimpleStartedService)));
            isServiceRunning = false;
            startButton.Enabled = true;
            stopButton.Enabled = false;
        }
    }
}

