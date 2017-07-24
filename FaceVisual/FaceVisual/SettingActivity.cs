using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FaceVisual
{
    [Activity()]
    public class SettingActivity : RootActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            this.SetContentView(Resource.Layout.Settings);

            var btnSave = this.FindViewById<Button>(Resource.Id.btnSave);
            var koala = this.FindViewById<EditText>(Resource.Id.koalaEditText);
            var cameraMain = this.FindViewById<EditText>(Resource.Id.etCameraMain);
            var cameraSub = this.FindViewById<EditText>(Resource.Id.etCamerasub);
            var welcome1 = this.FindViewById<EditText>(Resource.Id.welcomeEditText1);
            var welcome2 = this.FindViewById<EditText>(Resource.Id.welcomeEditText2);
            var delay = this.FindViewById<EditText>(Resource.Id.tvDelay);
            var btnSelect = this.FindViewById<Button>(Resource.Id.btnSelect);
            var btnDefault = this.FindViewById<Button>(Resource.Id.btnDefault);
            btnSelect.Click += BtnSelect_Click;
            btnDefault.Click += BtnDefault_Click;

            var cfg = Config.Profile;
            koala.Text = cfg.ServerIp;
            cameraMain.Text = cfg.CameraMain;
            cameraSub.Text = cfg.CameraSub;
            welcome1.Text = cfg.Welcome1;
            welcome2.Text = cfg.Welcome2;
            delay.Text = cfg.Delay.ToString();

            btnSave.Click += (s, e) =>
            {
                if (IsEmpty(koala))
                {
                    toast("请输入设备IP");
                    return;
                }
                if (IsEmpty(cameraMain))
                {
                    toast("请输入主摄像机IP");
                    return;
                }
                if (IsEmpty(cameraSub))
                {
                    toast("请输入副摄像机IP");
                    return;
                }
                if (IsEmpty(welcome1))
                {
                    toast("请输入欢迎语一");
                    return;
                }
                if (IsEmpty(welcome2))
                {
                    toast("请输入欢迎语二");
                    return;
                }
                Config.Profile.ServerIp = koala.Text;
                Config.Profile.CameraMain = cameraMain.Text;
                Config.Profile.CameraSub = cameraSub.Text;
                Config.Profile.Welcome1 = welcome1.Text;
                Config.Profile.Welcome2 = welcome2.Text;
                Config.Profile.Delay = Int32.Parse(delay.Text);
                Config.SaveProfile();

                StartActivity(typeof(FaceMainActivity));
                this.Finish();
            };
        }

        private void BtnDefault_Click(object sender, EventArgs e)
        {
            Config.Profile.BgUri = string.Empty;
            Toast.MakeText(this, "设置成功", ToastLength.Short).Show();
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(imageIntent, "photo"), 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                var temp = data.Data.ToString();
                if (temp.StartsWith("file"))
                {
                    Config.Profile.BgUri = data.Data.Path;
                }
                else
                {
                    var imagePath = GetPathToImage(data.Data);
                    Config.Profile.BgUri = imagePath;
                }
            }
        }

        private string GetPathToImage(Android.Net.Uri uri)
        {
            ICursor cursor = this.ContentResolver.Query(uri, null, null, null, null);
            cursor.MoveToFirst();
            string document_id = cursor.GetString(0);
            document_id = document_id.Split(':')[1];
            cursor.Close();

            cursor = ContentResolver.Query(
            Android.Provider.MediaStore.Images.Media.ExternalContentUri,
            null, MediaStore.Images.Media.InterfaceConsts.Id + " = ? ", new String[] { document_id }, null);
            cursor.MoveToFirst();
            string path = cursor.GetString(cursor.GetColumnIndex(MediaStore.Images.Media.InterfaceConsts.Data));
            cursor.Close();

            return path;
        }

        private int getNumber(string s)
        {
            int i = 0;
            Int32.TryParse(s, out i);
            return i;
        }

        private bool IsEmpty(EditText et)
        {
            var val = et.Text.Trim();
            if (string.IsNullOrEmpty(val))
                return true;
            else
                return false;
        }

        private void toast(string msg)
        {
            var t = Toast.MakeText(this, msg, ToastLength.Short);
            t.SetGravity(GravityFlags.CenterVertical, 0, 0);
            t.Show();
        }
    }
}