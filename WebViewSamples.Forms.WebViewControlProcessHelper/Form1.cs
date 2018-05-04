using System.Windows.Forms;
using Microsoft.Toolkit.Win32.UI.Controls.WinForms;

namespace WebViewSamples.Forms.WebViewControlProcessHelper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.InitializeComponent();

            // The form has a panel with Dock=DockStyle.Fill
            // Create a WebView that fills the entire Panel
            var webViewProcess = new Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlProcess();
            var webView = (WebView)webViewProcess.CreateWebView(this.panel1);
            webView.Dock = DockStyle.Fill;
            webView.Navigate(Constants.DefaultNavigationTarget);
        }
    }
}
