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

namespace BID_Front
{
    class Message
    {
        public string action { get; set; }

        public string face { get; set; }

        public string name { get; set; }

        public string type { get; set; }

        public string card { get; set; }

        public string email { get; set; }

        public string photo { get; set; }
    }
}