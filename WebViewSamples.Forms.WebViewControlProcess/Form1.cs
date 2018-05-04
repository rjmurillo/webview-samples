using System.Windows.Forms;
using Microsoft.Toolkit.Win32.UI.Controls.WinForms;

namespace WebViewSamples.Forms.WebViewControlProcess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.InitializeComponent();

            // The form has a panel with Dock=DockStyle.Fill
            // Create a WebView that fills the entire Panel
            var webViewProcess = new Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlProcess();
            var webViewControl = (WebView)webViewProcess.CreateWebView(this.panel1.Handle, this.panel1.Bounds);
            webViewControl.Dock = DockStyle.Fill;
            this.Controls.Add(webViewControl);
            webViewControl.Navigate(Constants.DefaultNavigationTarget);
        }
    }
}
