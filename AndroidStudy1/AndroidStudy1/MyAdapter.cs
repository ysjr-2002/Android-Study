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
    class MyAdapter : ArrayAdapter<Chat>
    {
        Chat[] list = null;
        Activity context;
        public MyAdapter(Activity context, int resourcId, Chat[] list) : base(context, resourcId, list)
        {
            this.context = context;
            this.list = list;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Chat chat = list[position];
            View temp = this.context.LayoutInflater.Inflate(Resource.Layout.msg_item, null);
            var left = temp.FindViewById<LinearLayout>(Resource.Id.left_layout);
            var left_msg = temp.FindViewById<TextView>(Resource.Id.left_msg);
            var right = temp.FindViewById<LinearLayout>(Resource.Id.right_layout);
            var right_msg = temp.FindViewById<TextView>(Resource.Id.right_msg);
            if (chat.Type == Chat.RECEIVE)
            {
                left.Visibility = ViewStates.Visible;
                right.Visibility = ViewStates.Gone;
                left_msg.Text = chat.Content;
            }
            else
            {
                left.Visibility = ViewStates.Gone;
                right.Visibility = ViewStates.Visible;
                right_msg.Text = chat.Content;
            }
            convertView = temp;
            return temp;
        }
    }
}