using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Java.Lang;
using System.Net.Sockets;
using System.Xml;

namespace AutoUpgrade
{
    [Activity(Label = "AutoUpgrade", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        string localFile = "";
        private Button btn2;
        public const int MESSAGE_STATE_CHANGE = 1;
        public const int MESSAGE_READ = 2;
        public const int MESSAGE_WRITE = 3;
        public const int MESSAGE_DEVICE_NAME = 4;
        public const int MESSAGE_TOAST = 5;

        const int setup = 6543;
        const int update = 6544;
        private Handler handler = null;

        EditText edittext_write;
        EditText edittext_read;

        Button button_write;
        Button button_read;
        Button button_version;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            Button button = FindViewById<Button>(Resource.Id.MyButton);
            button_version = FindViewById<Button>(Resource.Id.button_version);
            edittext_write = FindViewById<EditText>(Resource.Id.edittext_write);
            edittext_read = FindViewById<EditText>(Resource.Id.edittext_read);

            button.Click += Button_Click;
            btn2 = this.FindViewById<Button>(Resource.Id.MyButton2);
            btn2.Click += delegate
            {
                var stream = this.OpenFileOutput("shit", FileCreationMode.Private);
                var sw = new StreamWriter(stream);
                var txt = edittext_write.Text;
                sw.Write(txt);
                sw.Close();
            };

            button_read = FindViewById<Button>(Resource.Id.MyButton3);
            button_read.Click += Button_read_Click;

            //test = new Handler((msg) =>
            //{
            //    if (msg.What == setup)
            //    {
            //        var bundle1 = msg.Data;
            //        var setupPath = bundle1.GetString("path");
            //        SetupApk(setupPath);
            //    }
            //});

            handler = new Handler(new MyHandler(this));

            button_version.Click += Button_version_Click;
        }

        private async void Button_version_Click(object sender, EventArgs e)
        {
            await Task.Factory.StartNew(() =>
            {
                string uri = "http://192.168.2.131:9872/version.xml";
                var request = WebRequest.Create(uri);
                var response = request.GetResponse();
                var stream = response.GetResponseStream();
                //var sw = new StreamReader(stream);
                //var content = sw.ReadToEnd();

                XmlDocument doc = new XmlDocument();
                doc.Load(stream);

                stream.Close();

                var node_version = doc.SelectSingleNode("update/version");
                var node_name = doc.SelectSingleNode("update/name");
                var node_url = doc.SelectSingleNode("/update/url");

                var version = node_version.InnerXml;
                var name = node_name.InnerXml;
                var url = node_url.InnerXml;


                var msg = handler.ObtainMessage();
                msg.What = update;
                var bundle = new Bundle();
                bundle.PutString("version", version);
                bundle.PutString("name", name);
                bundle.PutString("url", url);
                msg.Data = bundle;

                handler.SendMessage(msg);
            });
        }

        private void Button_read_Click(object sender, EventArgs e)
        {
            var stream = this.OpenFileInput("shit");
            var sw = new StreamReader(stream);

            var txt = sw.ReadToEnd();
            sw.Close();

            edittext_read.Text = txt;
        }

        class MyHandler : Java.Lang.Object, Handler.ICallback
        {
            MainActivity main;
            AlertDialog dialog = null;
            public MyHandler(MainActivity main)
            {
                this.main = main;
            }

            public bool HandleMessage(Message msg)
            {
                if (msg.What == setup)
                {
                    var bundle = msg.Data;
                    var setupPath = bundle.GetString("path");
                    main.SetupApk(setupPath);
                }
                if (msg.What == update)
                {
                    var bundle = msg.Data;
                    var name = bundle.GetString("name");
                    var version = bundle.GetString("version");
                    var url = bundle.GetString("url");

                    //Toast.MakeText(main, name + " " + version + " " + url, ToastLength.Short).Show();
                    AlertDialog.Builder builder = new AlertDialog.Builder(main);
                    builder.SetTitle("升级提示");
                    builder.SetMessage("确认升级吗?");

                    builder.SetPositiveButton("确定", (d, which) =>
                    {
                        //main.showToast("确定");
                    });

                    builder.SetNegativeButton("取消", (d, which) =>
                    {
                        //main.showToast("取消");
                        dialog.Dismiss();
                    });
                    dialog = builder.Create();
                    dialog.Show();
                }
                return true;
            }
        }

        private void showToast(string str)
        {
            Toast.MakeText(this, str, ToastLength.Short).Show();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            ProgressDialog pd = new ProgressDialog(this);
            pd.SetProgressStyle(ProgressDialogStyle.Horizontal);
            pd.SetTitle("更新下载");
            pd.Show();

            Task.Factory.StartNew(() =>
            {
                downLoad(pd);
            });
        }

        private void downLoad(ProgressDialog pd)
        {
            string url = "http://192.168.2.131:9872/1.apk";
            WebRequest request = WebRequest.Create(url);

            var response = request.GetResponse();
            var stream = response.GetResponseStream();

            var length = stream.Length;
            pd.Max = (int)length;

            BinaryReader br = new BinaryReader(stream);

            var root = Android.OS.Environment.ExternalStorageDirectory.Path;
            var fullPath = System.IO.Path.Combine(root, "update.apk");
            var fs = File.Create(fullPath);

            int total = 0;
            while (true)
            {
                var buffer = br.ReadBytes(1024);
                total += buffer.Length;
                this.RunOnUiThread(new Action(() =>
                {
                    pd.Progress = total;
                }));

                fs.Write(buffer, 0, buffer.Length);
                if (buffer.Length < 1024)
                {
                    break;
                }
            }

            br.Close();
            stream.Close();
            response.Close();

            fs.Close();

            pd.Dismiss();

            Bundle bundle = new Bundle();
            bundle.PutString("path", fullPath);
            Message message = handler.ObtainMessage();
            message.What = setup;
            message.Data = bundle;
            handler.SendMessage(message);
        }

        private void SetupApk(string path)
        {
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(Android.Net.Uri.Parse(string.Concat("file://", path)), "application/vnd.android.package-archive");
            this.StartActivity(intent);
        }
    }
}

