using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Locations;

namespace App1
{
    [Activity(Label = "App1", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, ILocationListener
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.NoTitle);
            this.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            //RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            Button button1 = FindViewById<Button>(Resource.Id.MyButton1);
            Button button2 = FindViewById<Button>(Resource.Id.MyButton2);
            Button button3 = FindViewById<Button>(Resource.Id.MyButtonInit);
            Button button4 = FindViewById<Button>(Resource.Id.MyButton4);
            Button button5 = FindViewById<Button>(Resource.Id.MyButton5);
            Button button6 = FindViewById<Button>(Resource.Id.MyButton6);
            Button button7 = FindViewById<Button>(Resource.Id.MyButton7);
            button.Click += delegate
            {
                Writefile("Java Test");
            };

            button1.Click += delegate
            {
                writefile1(DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss"));
            };

            button2.Click += delegate
            {
                readfile1();
            };

            button3.Click += delegate
            {
                var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.Path;
                var name = "ysj";
                var dir = System.IO.Path.Combine(sdCardPath, name);

                if (System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.Delete(dir, true);
                }
                else
                {
                    Toast.MakeText(this, "目录不存在->" + dir, ToastLength.Short).Show();
                }
            };

            button4.Click += delegate
            {
                DisplayMetrics dm = new DisplayMetrics();
                WindowManager.DefaultDisplay.GetMetrics(dm);
                var info = dm.WidthPixels + " " + dm.HeightPixels + " " + dm.Xdpi + " " + dm.Ydpi;
                Toast.MakeText(this, info, ToastLength.Short).Show();
            };

            button5.Click += delegate
            {
                var builder = new AlertDialog.Builder(this);
                builder.SetTitle("警告");
                builder.SetMessage("你很漂亮？？？");
                builder.SetPositiveButton("确定", (a, b) =>
                {
                });
                var dialog = builder.Create();
                dialog.Show();
                //builder.Show();
            };

            button6.Click += delegate
            {
                StartActivity(typeof(SubActivity));
                this.Finish();
            };

            button7.Click += delegate
            {
                StartActivity(typeof(SubActivity));
            };

            Console.WriteLine("onCreate " + typeof(MainActivity).FullName);
        }

        protected override void OnStart()
        {
            base.OnStart();
            Console.WriteLine("OnStart " + typeof(MainActivity).FullName);
        }

        protected override void OnResume()
        {
            base.OnResume();
            Console.WriteLine("OnResume " + typeof(MainActivity).FullName);
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            Console.WriteLine("OnRestart " + typeof(MainActivity).FullName);
        }

        protected override void OnPause()
        {
            base.OnPause();
            Console.WriteLine("OnPause " + typeof(MainActivity).FullName);
        }

        protected override void OnStop()
        {
            base.OnStop();
            Console.WriteLine("OnStop " + typeof(MainActivity).FullName);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Console.WriteLine("OnDestroy " + typeof(MainActivity).FullName);
        }

        private void readfile1()
        {
            try
            {
                var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.Path;
                var name = "ysj";
                var dir = System.IO.Path.Combine(sdCardPath, name);
                var filePath = System.IO.Path.Combine(dir, "xamarin.txt");
                if (System.IO.File.Exists(filePath))
                {
                    var content = System.IO.File.ReadAllText(filePath);
                    Toast.MakeText(this, content, ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "文件路径不存在->" + filePath, ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        private void writefile1(string content)
        {
            try
            {
                var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.Path;
                var name = "ysj";
                var dir = System.IO.Path.Combine(sdCardPath, name);
                var sub = new System.IO.DirectoryInfo(dir);
                if (!sub.Exists)
                    sub.Create();

                var filePath = System.IO.Path.Combine(dir, "xamarin.txt");
                var data = System.Text.Encoding.UTF8.GetBytes(content);
                System.IO.File.AppendAllText(filePath, content);
                Toast.MakeText(this, "ok", ToastLength.Short).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        private void Writefile(string content)
        {
            try
            {
                var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.Path;
                var filePath = System.IO.Path.Combine(sdCardPath, "java.txt");
                var data = System.Text.Encoding.UTF8.GetBytes(content);
                Java.IO.FileOutputStream fs = new Java.IO.FileOutputStream(filePath);
                fs.Write(data, 0, data.Length);
                fs.Close();
                Toast.MakeText(this, "ok", ToastLength.Short).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        void ILocationListener.OnLocationChanged(Location location)
        {
            Toast.MakeText(this, location.Latitude.ToString() + " " + location.Longitude.ToString(), ToastLength.Short).Show();
        }

        void ILocationListener.OnProviderDisabled(string provider)
        {
        }

        void ILocationListener.OnProviderEnabled(string provider)
        {
        }

        void ILocationListener.OnStatusChanged(string provider, Availability status, Bundle extras)
        {
        }
    }
}

