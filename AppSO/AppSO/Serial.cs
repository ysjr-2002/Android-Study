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
using Java.IO;
using System.Runtime.InteropServices;
using Mono;

namespace com.pci.pca.readcard
{
    class SerialPort
    {
        const string TAG = "shit";

        private Android.Content.Res.AssetFileDescriptor mFd;
        private FileInputStream mFileInputStream;
        private FileOutputStream mFileOutputStream;
        private bool trig_on = false;

        public SerialPort()
        {

        }

        public SerialPort(int port, int baudrate, int flags)
        {

            var su = Java.Lang.Runtime.GetRuntime().Exec("/system/xbin/su");

            mFd = open(port, baudrate);

            if (mFd == null)
            {
                Android.Util.Log.Error(TAG, "native open returns null");
                throw new IOException();
            }

            mFileInputStream = new FileInputStream(mFd.FileDescriptor);
            mFileOutputStream = new FileOutputStream(mFd.FileDescriptor);
        }

        public void psam_poweron()
        {
            psampoweron();
        }

        [DllImport("devapi")]
        public static extern void psampoweron();

        [DllImport("SerialPort")]
        private static extern Android.Content.Res.AssetFileDescriptor open(int port, int baudrate);
    }
}