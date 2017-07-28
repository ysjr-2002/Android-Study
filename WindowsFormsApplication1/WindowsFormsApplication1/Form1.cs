using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VideoPublic;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.ShowDialog();
            var path = ofd.FileName;

            VideoController vc = new VideoController();
            var temp = vc.GetVideoFileInfo(path);
            if (temp != null)
            {
                MessageBox.Show(temp.FrameRate.ToString());
            }
        }
    }
}
