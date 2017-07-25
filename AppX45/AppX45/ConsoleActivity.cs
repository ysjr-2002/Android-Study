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
using Hoho.Android.UsbSerial.Driver;
using Android.Hardware.Usb;
using Hoho.Android.UsbSerial.Util;
using Android.Util;
using System.Threading.Tasks;

namespace AppX45
{
    [Activity(Label = "ConsoleActivity")]
    public class ConsoleActivity : Activity
    {
        static readonly string TAG = typeof(ConsoleActivity).Name;
        public const string EXTRA_TAG = "PortInfo";

        IUsbSerialPort port;

        UsbManager usbManager;
        TextView titleTextView;
        TextView dumpTextView;
        ScrollView scrollView;

        SerialInputOutputManager serialIoManager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            this.SetContentView(Resource.Layout.console);

            usbManager = GetSystemService(Context.UsbService) as UsbManager;
            titleTextView = FindViewById<TextView>(Resource.Id.demoTitle);
            dumpTextView = FindViewById<TextView>(Resource.Id.consoleText);
            scrollView = FindViewById<ScrollView>(Resource.Id.demoScroller);
        }

        protected override void OnPause()
        {
            base.OnPause();

            if (serialIoManager != null && serialIoManager.IsOpen)
            {
                serialIoManager.Close();
            }
        }

        internal static Task<IList<IUsbSerialDriver>> FindAllDriversAsync(UsbManager usbManager)
        {
            // using the default probe table
            // return UsbSerialProber.DefaultProber.FindAllDriversAsync (usbManager);

            // adding a custom driver to the default probe table
            var table = UsbSerialProber.DefaultProbeTable;
            table.AddProduct(0x1b4f, 0x0008, Java.Lang.Class.FromType(typeof(CdcAcmSerialDriver))); // IOIO OTG
            var prober = new UsbSerialProber(table);
            return prober.FindAllDriversAsync(usbManager);
        }

        protected async override void OnResume()
        {
            base.OnResume();

            var portInfo = Intent.GetParcelableExtra(EXTRA_TAG) as UsbSerialPortInfo;
            int vendorId = portInfo.VendorId;
            int deviceId = portInfo.DeviceId;
            int portNumber = portInfo.PortNumber;

            Log.Info(TAG, string.Format("VendorId: {0} DeviceId: {1} PortNumber: {2}", vendorId, deviceId, portNumber));

            var drivers = await FindAllDriversAsync(usbManager);

            var driver = drivers.Where((d) => d.Device.VendorId == vendorId && d.Device.DeviceId == deviceId).FirstOrDefault();
            if (driver == null)
                throw new Exception("Driver specified in extra tag not found.");

            port = driver.Ports[portNumber];
            if (port == null)
            {
                titleTextView.Text = "No serial device.";
                return;
            }
            Log.Info(TAG, "port=" + port);

            titleTextView.Text = "Serial device: " + port.GetType().Name;

            serialIoManager = new SerialInputOutputManager(port)
            {
                BaudRate = 115200,
                DataBits = 8,
                StopBits = StopBits.One,
                Parity = Parity.None,
            };
            serialIoManager.DataReceived += (sender, e) =>
            {
                RunOnUiThread(() =>
                {
                    UpdateReceivedData(e.Data);
                });
            };
            serialIoManager.ErrorReceived += (sender, e) =>
            {
                RunOnUiThread(() =>
                {
                    //var intent = new Intent(this, typeof(DeviceListActivity));
                    //StartActivity(intent);
                });
            };

            Log.Info(TAG, "Starting IO manager ..");
            try
            {
                serialIoManager.Open(usbManager);
            }
            catch (Java.IO.IOException e)
            {
                titleTextView.Text = "Error opening device: " + e.Message;
                return;
            }
        }

        void UpdateReceivedData(byte[] data)
        {
            var message = "Read " + data.Length + " bytes: \n"
                + HexDump.DumpHexString(data) + "\n\n";

            dumpTextView.Append(message);
            scrollView.SmoothScrollTo(0, dumpTextView.Bottom);
        }
    }
}