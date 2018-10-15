using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebViewSamples.Forms.MultipleWebViews
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void terminateRandomWebViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var provider = new RNGCryptoServiceProvider();
            var byteArray = new byte[4];
            provider.GetBytes(byteArray);

            //convert 4 bytes to an integer
            var randomInteger = BitConverter.ToUInt32(byteArray, 0) % 4;
            switch (randomInteger)
            {
                case 0:
                    webViewControl1.Terminate();
                    break;
                case 1:
                    webViewControl2.Terminate();
                    break;
                case 2:
                    webViewControl3.Terminate();
                    break;
                case 3:
                    webViewControl4.Terminate();
                    break;
            }
        }
    }
}
