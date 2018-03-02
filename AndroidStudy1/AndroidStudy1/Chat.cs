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

namespace AndroidStudy1
{
    class Chat
    {
        public int Type { get; set; }
        public string Content { get; set; }

        public const int RECEIVE = 2;
        public const int SEND = 1;

        public Chat(string msg, int type)
        {
            this.Content = msg;
            this.Type = type;
        }
    }
}