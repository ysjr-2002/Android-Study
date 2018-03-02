﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FaceVisual
{
    /// <summary>
    /// 开机启动-必须设置IntentFilter
    /// </summary>
    [BroadcastReceiver]
    [IntentFilter(new[] { Intent.ActionBootCompleted }, Categories = new[] { Intent.CategoryDefault })]
    class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == Intent.ActionBootCompleted)
            {
                Intent newIntent = new Intent(context, typeof(SplashActivity));
                //这个标记必须加
                newIntent.AddFlags(ActivityFlags.NewTask);
                context.StartActivity(newIntent);
            }
        }
    }
}