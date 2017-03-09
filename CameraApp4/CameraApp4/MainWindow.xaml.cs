using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebServer_WPF.server;
using WebSocketSharp.Server;

namespace WebServer_WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            start();
        }

        WebSocketServer wssv = null;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void start()
        {
            wssv = new WebSocketServer(4649);

            wssv.AddWebSocketService<Echo>("/Echo", initEcho);
            wssv.Start();
            if (wssv.IsListening)
            {
                Console.WriteLine("Listening on port {0}, and providing WebSocket services:", wssv.Port);
                foreach (var path in wssv.WebSocketServices.Paths)
                    Console.WriteLine("- {0}", path);
            }
            btnserver.IsEnabled = false;
        }

        private Echo echo = null;
        private Echo initEcho()
        {
            var echo = new Echo(getImage);
            this.echo = echo;
            return echo;
        }

        private void getImage(byte[] data)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = new MemoryStream(data);
                bi.Rotation = Rotation.Rotate270;
                bi.EndInit();
                img.Source = bi;
            }));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //wssv.WebSocketServices.Broadcast("我发送广播了");

            command cmd = new command { action = "capture" };
            var js = new JavaScriptSerializer();
            var json = js.Serialize(cmd);
            this.echo.Context.WebSocket.Send(json);
        }
    }

    class command
    {
        public string action { get; set; }
    }
}
