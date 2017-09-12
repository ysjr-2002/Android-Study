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
using System.Runtime.InteropServices;
using Android.Content.Res;
using Java.IO;
using Android.Util;

namespace AppSO
{
    class SerialPort
    {
        FileDescriptor fd = null;
        private FileDescriptor mFd;
        private FileInputStream mFileInputStream;
        private FileOutputStream mFileOutputStream;
        private const string TAG = "SerialPort";
        public SerialPort(int port, int baud)
        {
            fd = open(port, baud);

            if (fd == null)
            {
                Log.Error(TAG, "native open returns null");
                return;
            }

            mFileInputStream = new FileInputStream(fd);
            mFileOutputStream = new FileOutputStream(fd);

        }

        [DllImport("libSerialPort.so")]
        public static extern FileDescriptor open(int port, int baud);

        [DllImport("libSerialPort.so")]
        public static extern void close(int port);
    }
}