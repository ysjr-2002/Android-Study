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

namespace CameraApp4
{
    class Config
    {
        private const char spliter = '$';
        private const string faceroot = "obria";
        private const string config = "config.txt";
        private const string log = "log.tx";
        public static Profile Profile { get; set; }

        static Config()
        {
            Profile = new Profile();
        }

        public static void Log(string log)
        {
            var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.Path;
            var dir = System.IO.Path.Combine(sdCardPath, faceroot);
            var filePath = System.IO.Path.Combine(dir, log);
            log = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + " " + log + System.Environment.NewLine;
            System.IO.File.AppendAllText(filePath, log);
        }

        public static void ReadProfile()
        {
            var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.Path;
            var dir = System.IO.Path.Combine(sdCardPath, faceroot);
            var filePath = System.IO.Path.Combine(dir, config);

            if (System.IO.File.Exists(filePath))
            {
                var content = System.IO.File.ReadAllText(filePath);
                var array = content.Split(spliter);
                Profile.ServerIp = array[0];
                Profile.Delay = Convert.ToInt32(array[1]);
            }
        }

        public static void SaveProfile()
        {
            var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.Path;
            var dir = System.IO.Path.Combine(sdCardPath, faceroot);
            var sub = new System.IO.DirectoryInfo(dir);
            if (!sub.Exists)
                sub.Create();

            var filePath = System.IO.Path.Combine(dir, config);
            var content = string.Concat(Profile.ServerIp, spliter, Profile.Delay);
            System.IO.File.AppendAllText(filePath, content);
        }
    }

    public class Profile
    {
        public string ServerIp { get; set; }
        public int Delay { get; set; }
    }
}