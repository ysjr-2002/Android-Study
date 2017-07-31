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

namespace App1
{
    [BroadcastReceiver]
    [IntentFilter(new[] { Intent.ActionMain }, Categories = new[] { Android.Content.Intent.CategoryDefault })]
    class Boot : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            //Intent newIntent = new Intent(context, typeof(TestActivity));
            //newIntent.SetFlags(ActivityFlags.NewTask);
            //context.StartActivity(newIntent);
        }
    }
}