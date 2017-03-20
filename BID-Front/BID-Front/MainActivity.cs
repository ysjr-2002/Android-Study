using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Provider;
using Android.Graphics;
using Newtonsoft.Json;
using System.IO;

namespace BID_Front
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/Icon")]
    public class MainActivity : Activity
    {
        private Button bt_Employee;
        private Button bt_Setting;
        private Button bt_Take;
        private const int Take_Picture = 1;

        private TextView tv_card;
        private TextView tv_name;
        private TextView tv_email;
        private ImageView iv_photo;
        private Bitmap faceBitmap;

        private TextView tv_Server;

        private AlertDialog alertDialog = null;
        private MySocket socket = null;

        private void DialogCancel()
        {
            var field = alertDialog.Class.Superclass.GetDeclaredField("mShowing");
            field.Accessible = true;
            //设置mShowing值，欺骗android系统  
            field.Set(alertDialog, false);
        }

        private void DialogDismiss()
        {
            var field = alertDialog.Class.Superclass.GetDeclaredField("mShowing");
            field.Accessible = true;
            //设置mShowing值，欺骗android系统  
            field.Set(alertDialog, true);
        }

        private void Dialog(string msg)
        {
            Toast.MakeText(this, msg, ToastLength.Short).Show();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            this.Window.SetStatusBarColor(Color.Black);
            SetContentView(Resource.Layout.Main);

            bt_Employee = this.FindViewById<Button>(Resource.Id.btEmployee);
            bt_Employee.Click += btNewEmployee_Click;

            bt_Setting = this.FindViewById<Button>(Resource.Id.btSetting);
            bt_Setting.Click += btSetting_Click;
        }

        protected override void OnStart()
        {
            base.OnStart();
            connect();
        }

        private async void connect()
        {
            socket = new MySocket(this);
            var task = socket.Connect("192.168.0.4");
            await task;
        }

        private void btSetting_Click(object sender, EventArgs e)
        {
            var view = LayoutInflater.Inflate(Resource.Layout.server, null);
            var builder = new AlertDialog.Builder(this);
            builder.SetTitle("服务器配置");
            builder.SetView(view);
            builder.SetPositiveButton("确定", (a, b) =>
            {
                var serverIp = tv_Server.Text;

                if (string.IsNullOrEmpty(serverIp))
                {
                    Dialog("请填写服务器地址！");
                    DialogCancel();
                    return;
                }
                DialogDismiss();
            });

            builder.SetNegativeButton("取消", (a, b) =>
            {
                DialogDismiss();
            });
            alertDialog = builder.Create();
            alertDialog.Show();

            //tv_Server = view.FindViewById<EditText>(Resource.Id.tvServer);
        }

        private void btTake_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, Take_Picture);
        }

        private void btNewEmployee_Click(object sender, EventArgs e)
        {
            var view = LayoutInflater.Inflate(Resource.Layout.employee, null);
            var builder = new AlertDialog.Builder(this);
            builder.SetTitle("员工录入");
            builder.SetView(view);
            builder.SetPositiveButton("确定", (a, b) =>
            {
                var card = tv_card.Text;
                var name = tv_name.Text;
                var email = tv_email.Text;

                if (faceBitmap == null)
                {
                    Dialog("请拍照！");
                    DialogCancel();
                    return;
                }

                if (string.IsNullOrEmpty(card))
                {
                    Dialog("请填写卡号！");
                    DialogCancel();
                    return;
                }

                Message message = new Message
                {
                    action = "employee",
                    type = "ok",
                    card = card,
                    name = name,
                    email = email,
                };

                MemoryStream ms = new MemoryStream();
                faceBitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                byte[] byteArray = ms.ToArray();
                ms.Close();
                faceBitmap.Dispose();
                var facebase64 = Convert.ToBase64String(byteArray);
                message.face = facebase64;
                var json = JsonConvert.SerializeObject(message);
                socket.Send(json);

                DialogDismiss();
            });

            builder.SetNegativeButton("取消", (a, b) =>
            {
                DialogDismiss();
            });
            alertDialog = builder.Create();
            alertDialog.Show();


            bt_Take = view.FindViewById<Button>(Resource.Id.btntake);
            bt_Take.Click += btTake_Click;

            iv_photo = view.FindViewById<ImageView>(Resource.Id.ivPhoto);
            tv_card = view.FindViewById<TextView>(Resource.Id.tvCard);
            tv_name = view.FindViewById<TextView>(Resource.Id.tvName);
            tv_email = view.FindViewById<TextView>(Resource.Id.tvEmail);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == Take_Picture)
            {
                if (resultCode == Result.Ok)
                {
                    faceBitmap = (Bitmap)data.Extras.Get("data");
                    iv_photo.SetImageBitmap(faceBitmap);
                }
            }
        }
    }
}

