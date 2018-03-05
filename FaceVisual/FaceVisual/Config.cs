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

namespace FaceVisual
{
    class Config
    {
        private const char spliter = '$';
        private const string faceroot = "facevisual";
        private const string config = "config.txt";
        private const string log = "log.txt";
        public static Profile Profile { get; set; }

        static Config()
        {
            Profile = new Profile();
        }

        public static void Log(string content)
        {
            var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.Path;
            var dir = System.IO.Path.Combine(sdCardPath, faceroot);
            var filePath = System.IO.Path.Combine(dir, log);
            content = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + "->" + content + System.Environment.NewLine;
            System.IO.File.AppendAllText(filePath, content, Encoding.UTF8);
        }

        public static void ReadProfile()
        {
            ISharedPreferences sp = Application.Context.GetSharedPreferences("face", FileCreationMode.Private);
            Profile.ServerIp = sp.GetString("serverIp", "192.168.1.100");
            Profile.CameraMain = sp.GetString("cameraIp", "192.168.1.101");
            Profile.Welcome1 = sp.GetString("welcome1", "ª∂”≠π‚¡Ÿ");
            Profile.Welcome2 = sp.GetString("welcome2", "ª∂”≠π‚¡Ÿ");
            Profile.Delay = sp.GetInt("delay", 2000);
            Profile.BgUri = sp.GetString("bgUri", "");
        }

        public static void SaveProfile()
        {
            ISharedPreferences sp = Application.Context.GetSharedPreferences("face", FileCreationMode.Private);
            ISharedPreferencesEditor editor = sp.Edit();
            editor.PutString("serverIp", Profile.ServerIp);
            editor.PutString("cameraIp", Profile.CameraMain);
            editor.PutString("welcome1", Profile.Welcome1);
            editor.PutString("welcome2", Profile.Welcome2);
            editor.PutInt("delay", Profile.Delay);
            editor.PutString("bgUri", Profile.BgUri);
            editor.Commit();
        }
    }

    public class Profile
    {
        public Profile()
        {
            ServerIp = "192.168.0.53";
            CameraMain = "192.168.0.10";
            CameraSub = "192.168.1.10";
            Delay = 2000;
            Welcome1 = "ª∂”≠π‚¡Ÿ";
            Welcome2 = "ª∂”≠π‚¡Ÿ";
        }

        public string ServerIp { get; set; }
        public string CameraMain { get; set; }
        public string CameraSub { get; set; }
        public string Welcome1 { get; set; }
        public string Welcome2 { get; set; }
        public int Delay { get; set; }
        public string BgUri { get; set; }
    }
}