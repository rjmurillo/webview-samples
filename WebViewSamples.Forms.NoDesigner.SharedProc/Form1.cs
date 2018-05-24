using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Toolkit.Win32.UI.Controls.WinForms;

namespace WebViewSamples.Forms.NoDesigner.SharedProc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //// Create a new WebView from the existing WebView's process and add to panel2
            //WebView webView2 = new WebView();
            //((ISupportInitialize)webView2).BeginInit();

            //splitContainer1.Panel2.Controls.Add(webView2);

            //webView2.Dock = DockStyle.Fill;
            //webView2.Location = splitContainer1.Panel2.Location;
            //webView2.MinimumSize = webView1.MinimumSize;
            //webView2.Name = "webView2";
            //webView2.Source = webView2.Source;

            //((ISupportInitialize)(webView2)).EndInit();

            // NavigationStarting event will have already fired, so sync up the source initially
            webView2.Source = webView1.Source;
        }

        private void OnWebViewNavigationStarting(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationStartingEventArgs e)
        {
            webView2.Navigate(e.Uri);
        }
    }
}
