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
using WebSocketSharp;

namespace App5.Resources.layout
{
    [Activity(Label = "wsClientActivity", MainLauncher = true)]
    public class wsClientActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.wsclient);

            var button1 = FindViewById<Button>(Resource.Id.button1);
            button1.Click += Button_Click;

            var button2 = FindViewById<Button>(Resource.Id.button2);
            button2.Click += button2_click;
        }

        private WebSocket ws = null;
        private void Button_Click(object sender, EventArgs e)
        {
            ws = new WebSocket("ws://192.168.0.4:4649/Echo");
            ws.OnOpen += Ws_OnOpen;
            ws.OnClose += Ws_OnClose;
            ws.OnError += Ws_OnError;
            ws.OnMessage += Ws_OnMessage;
            ws.Connect();
        }

        void print(string msg)
        {
            Console.WriteLine("client:" + msg);
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            print("message");
            if (e.IsText)
            {
                print(e.Data);
            }
        }

        private void Ws_OnError(object sender, ErrorEventArgs e)
        {
            print("error");
        }

        private void Ws_OnClose(object sender, CloseEventArgs e)
        {
            print("close");
        }

        private void Ws_OnOpen(object sender, EventArgs e)
        {
            print("open");
        }

        private void button2_click(object sender, EventArgs e)
        {
            ws.Send("ni hao");
        }
    }
}