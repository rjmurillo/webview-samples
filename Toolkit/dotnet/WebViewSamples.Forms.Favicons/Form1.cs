using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Toolkit.Forms.UI.Controls;

namespace WebViewSamples.Forms.Favicons
{
    public partial class Form1 : Form
    {
        private WebView _webView1;

        public Form1()
        {
            this.InitializeComponent();

            this.SetDefaultIcon();

            this._webView1 = new WebView();
            ((ISupportInitialize)this._webView1).BeginInit();
            this._webView1.Size = this.Size;
            this._webView1.Dock = DockStyle.Fill;
            this._webView1.NavigationStarting += this.OnWebViewNavigationStarting;
            this._webView1.ContentLoading += this.OnWebViewContentLoading;
            this._webView1.DOMContentLoaded += this.OnWebViewDOMCOntentLoaded;
            this._webView1.NavigationCompleted += this.OnWebViewNavigationCompleted;
            this._webView1.NewWindowRequested += this.OnWebViewNewWindowRequested;
            this._webView1.Source = Constants.DefaultNavigationTargetUri;
            this.Controls.Add(this._webView1);
            ((ISupportInitialize)this._webView1).EndInit();
        }

        private void OnWebViewNewWindowRequested(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNewWindowRequestedEventArgs e)
        {
            this._webView1.Source = e.Uri;
            e.Handled = true;
        }

        private void OnWebViewNavigationCompleted(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationCompletedEventArgs e)
        {
            this.Text = this._webView1.DocumentTitle;
#pragma warning disable 4014
            this._webView1.SetFavIconAsync(this);
#pragma warning restore 4014
        }

        private void OnWebViewNavigationStarting(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationStartingEventArgs e)
        {
            this.Text = "Navigating " + e.Uri?.Host ?? string.Empty;
            this.SetDefaultIcon();
        }

        private void OnWebViewDOMCOntentLoaded(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlDOMContentLoadedEventArgs e)
        {
            this.Text = this._webView1.DocumentTitle;
#pragma warning disable 4014
            this._webView1.SetFavIconAsync(this);
#pragma warning restore 4014
        }

        private void OnWebViewContentLoading(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlContentLoadingEventArgs e)
        {
            this.Text = "Waiting for " + e.Uri?.Host ?? string.Empty;
        }
    }
}
