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
    /// <summary>
    /// 开机启动-必须设置IntentFilter
    /// </summary>
    [BroadcastReceiver]
    [IntentFilter(new[] { Android.Content.Intent.ActionBootCompleted }, Categories = new[] { Android.Content.Intent.CategoryDefault })]
    class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == Intent.ActionBootCompleted)
            {
                Intent newIntent = new Intent(context, typeof(MainActivity));
                //这个标记必须加
                newIntent.AddFlags(ActivityFlags.NewTask);
                context.StartActivity(newIntent);
            }
        }
    }
}