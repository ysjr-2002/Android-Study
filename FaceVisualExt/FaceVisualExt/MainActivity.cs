using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Hardware.Display;
using Android.Media;
using System.IO;
using System.Threading.Tasks;
using Android.Net;

namespace FaceVisualExt
{
    [Activity(Label = "FaceVisualExt", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Unspecified)]
    public class MainActivity : Activity
    {
        int count = 100;
        static readonly string Filename = "count";
        string path;
        string filename;

        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            //this.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            SetContentView(Resource.Layout.front);
            GetDisplay();

            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            filename = Path.Combine(path, Filename);

            var btn = FindViewById<Button>(Resource.Id.MyButton);
            btn.Click += async delegate
            {
                await writeFileAsync();
            };

            var task = loadFileAsync();

            try
            {
                ConnectivityManager connectivityManager = (ConnectivityManager)this.GetSystemService(ConnectivityService);
                NetworkInfo networkInfo = connectivityManager.ActiveNetworkInfo;
                bool isOnline = networkInfo.IsConnected;
                if (isOnline)
                {
                    if (networkInfo.Type == ConnectivityType.Wifi)
                    {

                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }

        async Task<int> loadFileAsync()
        {
            if (File.Exists(filename))
            {
                using (var f = new StreamReader(OpenFileInput(Filename)))
                {
                    string line;
                    do
                    {
                        line = await f.ReadLineAsync();
                    } while (!f.EndOfStream);
                    Console.WriteLine("Load Finished");
                    return int.Parse(line);
                }
            }
            return 0;
        }

        async Task writeFileAsync()
        {
            using (var f = new StreamWriter(OpenFileOutput(Filename, FileCreationMode.Append | FileCreationMode.WorldReadable)))
            {
                await f.WriteLineAsync(count.ToString()).ConfigureAwait(false);
            }
            Console.WriteLine("Save Finished!");
        }

        private void GetDisplay()
        {
            DisplayManager displayManager;//屏幕管理类
            Display[] displays;//屏幕数组
            displayManager = (DisplayManager)this.GetSystemService(Context.DisplayService);
            displays = displayManager.GetDisplays();
            //主屏
            var display1 = displays[0];
            if (displays.Length >= 2)
            {
                //副屏
                var display2 = displays[1];
            }
            else
            {
                return;
            }

            DifferentDislay anotherDisplay = new DifferentDislay(this, display1);
            //mPresentation.getWindow().setType(
            //WindowManager.LayoutParams.TYPE_SYSTEM_ALERT);
            //mPresentation.Window.SetType(WindowManagerLayoutParams)
            //anotherDisplay.Window.SetType(WindowManagerTypes.SystemAlert);
            anotherDisplay.Window.SetType(WindowManagerTypes.SystemOverlay);
            anotherDisplay.Show();
        }
    }
}

