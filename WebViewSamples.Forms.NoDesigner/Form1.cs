using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Toolkit.Forms.UI.Controls;

namespace WebViewSamples.Forms.NoDesigner
{
    public partial class Form1 : Form
    {
        private WebView webView1;

        public Form1()
        {
            this.InitializeComponent();

            // Since no designer is used, we must hand-roll the code that the designer would have generated
            // WebView implements ISupportInitialize, so we must call the BeginInit and EndInit methods
            this.webView1 = new WebView();
            ((ISupportInitialize)this.webView1).BeginInit();
            this.webView1.Size = this.Size;
            this.Controls.Add(this.webView1);
            ((ISupportInitialize)this.webView1).EndInit();

            // We're not using Dock = Fill, but we could. This tests to ensure the HWND RECT is resized properly when we manually set Height and Width
            this.Resize += (o, e) =>
            {
                this.webView1.Size = this.Size;
            };

            this.webView1.Navigate(Constants.DefaultNavigationTarget);
        }
    }
}
