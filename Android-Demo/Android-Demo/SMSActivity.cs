using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Telephony;

namespace Android_Demo
{
    [Activity(Label = "Android_Demo", MainLauncher = false, Icon = "@drawable/icon")]
    public class SMSActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate
            {
                Intent intent = new Intent(Intent.ActionCall, Android.Net.Uri.Parse("tel:13760129591"));
                StartActivity(intent);
            };

            Button sendButton1 = FindViewById<Button>(Resource.Id.btnSendSMS1);
            sendButton1.Click += (s, e) =>
            {
                sendSMS(string.Empty);
            };

            Button sendButton2 = FindViewById<Button>(Resource.Id.btnSendSMS2);
            sendButton2.Click += (s, e) =>
            {
                sendSMS();
            };
        }

        private void sendSMS(string msg)
        {
            //PendingIntent pi = PendingIntent.GetActivity(this, 1, new Intent(Intent.ActionCall), PendingIntentFlags.)
            SmsManager.Default.SendTextMessage("13760129591", null, "Hello from Xamarin.Android", null, null);
        }

        private void sendSMS()
        {
            var smsUri = Android.Net.Uri.Parse("smsto:13760129591");
            var smsIntent = new Intent(Intent.ActionSendto, smsUri);
            smsIntent.PutExtra("sms_body", "Hello from Xamarin.Android");
            StartActivity(smsIntent);
        }
    }
}

