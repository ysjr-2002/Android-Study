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
using System.Threading.Tasks;
using System.Threading;
using Uri = Android.Net.Uri;
using Android.Media;

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

        private void Alert(string msg)
        {
            this.RunOnUiThread(new Action(() =>
            {
                Toast.MakeText(Application.Context, msg, ToastLength.Short).Show();
            }));
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
            Config.ReadProfile();
        }

        private void ConnectSocket()
        {
            socket = new MySocket(this, SaveBack);
            socket.Connect(Config.Profile.ServerIp);
        }

        private void ConnectDispose()
        {
            socket?.Close();
            socket = null;
        }

        private void SaveBack()
        {
            ConnectDispose();
            Alert("Save Success");
        }

        private void btSetting_Click(object sender, EventArgs e)
        {
            var view = LayoutInflater.Inflate(Resource.Layout.server, null);
            var builder = new AlertDialog.Builder(this);
            builder.SetTitle("Server configrations");
            builder.SetView(view);
            builder.SetPositiveButton("Confirm", (a, b) =>
            {
                var serverIp = tv_Server.Text;
                if (string.IsNullOrEmpty(serverIp))
                {
                    Dialog("Please input server IP address");
                    DialogCancel();
                    return;
                }
                Config.Profile.ServerIp = serverIp;
                Config.SaveProfile();
                Dialog("Server configuration save success");
                DialogDismiss();
            });

            builder.SetNegativeButton("Cancel", (a, b) =>
            {
                DialogDismiss();
            });
            alertDialog = builder.Create();
            alertDialog.Show();

            tv_Server = view.FindViewById<EditText>(Resource.Id.tvServer);
            tv_Server.Text = Config.Profile.ServerIp;
        }

        private void btNewEmployee_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Config.Profile.ServerIp))
            {
                Dialog("Please config server IP addess");
                return;
            }

            var view = LayoutInflater.Inflate(Resource.Layout.employee, null);
            var builder = new AlertDialog.Builder(this);
            builder.SetTitle("Employee Input");
            builder.SetView(view);
            builder.SetPositiveButton("Confirm", (a, b) =>
            {
                var card = tv_card.Text;
                var name = tv_name.Text;
                var email = tv_email.Text;

                if (faceBitmap == null)
                {
                    Dialog("Please take photo");
                    DialogCancel();
                    return;
                }

                if (string.IsNullOrEmpty(card))
                {
                    Dialog("Please input cardno");
                    DialogCancel();
                    return;
                }

                ConnectSocket();

                Message message = new Message
                {
                    action = "employee",
                    type = "save",
                    card = card,
                    name = name,
                    email = email,
                };

                MemoryStream ms = new MemoryStream();
                faceBitmap.Compress(Bitmap.CompressFormat.Png, 80, ms);
                byte[] byteArray = ms.ToArray();
                ms.Close();
                faceBitmap.Dispose();
                var facebase64 = Convert.ToBase64String(byteArray);
                message.face = facebase64;
                var json = JsonConvert.SerializeObject(message);
                socket.Send(json);

                DialogDismiss();
            });

            builder.SetNegativeButton("Cancel", (a, b) =>
            {
                DialogDismiss();
            });
            alertDialog = builder.Create();
            alertDialog.Show();


            bt_Take = view.FindViewById<Button>(Resource.Id.btntake);
            bt_Take.Click += btTake_Click;
            bt_Take.RequestFocus();

            iv_photo = view.FindViewById<ImageView>(Resource.Id.ivPhoto);
            tv_card = view.FindViewById<TextView>(Resource.Id.tvCard);
            tv_name = view.FindViewById<TextView>(Resource.Id.tvName);
            tv_email = view.FindViewById<TextView>(Resource.Id.tvEmail);
        }

        string photoPath = "";
        private void btTake_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            var photoName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            photoPath = System.IO.Path.Combine(GetDirectory(), photoName);
            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(new Java.IO.File(photoPath)));
            StartActivityForResult(intent, Take_Picture);
        }

        private string GetDirectory()
        {
            var root = Android.OS.Environment.ExternalStorageDirectory + "/pictures/";
            if (System.IO.Directory.Exists(root) == false)
            {
                System.IO.Directory.CreateDirectory(root);
            }
            root = System.IO.Path.Combine(root, "obria");
            if (System.IO.Directory.Exists(root) == false)
            {
                System.IO.Directory.CreateDirectory(root);
            }
            return root;
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == Take_Picture)
            {
                if (resultCode == Result.Ok)
                {
                    //var thumbnail = (Bitmap)data.Extras.Get("data");
                    faceBitmap = LoadAndResizeBitmap(photoPath, 600, 800);
                    iv_photo.SetImageBitmap(faceBitmap);
                }
            }
        }

        private Android.Graphics.Bitmap rotateMyBitmap(Android.Graphics.Bitmap bmp)
        {
            //*****旋转一下
            Android.Graphics.Matrix matrix = new Android.Graphics.Matrix();
            matrix.PostRotate(270);
            Android.Graphics.Bitmap nbmp2 = Android.Graphics.Bitmap.CreateBitmap(bmp, 0, 0, bmp.Width, bmp.Height, matrix, true);
            bmp.Dispose();
            return nbmp2;
        }

        private Bitmap LoadAndResizeBitmap(string fileName, int width, int height)
        {
            // First we get the the dimensions of the file on disk
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(fileName, options);

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                                   ? outHeight / height
                                   : outWidth / width;
            }

            // Now we will load the image and have BitmapFactory resize it for us.
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            var resizedBitmap = BitmapFactory.DecodeFile(fileName, options);
            var rotatedBitmap = rotateMyBitmap(resizedBitmap);
            return rotatedBitmap;
        }
    }
}

