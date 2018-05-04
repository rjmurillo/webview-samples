using System.Windows;
using Microsoft.Toolkit.Win32.UI.Controls.WPF;

namespace WebViewSamples.WPF.WebViewControlProcessHelperAsync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Default MainWindow.xml with named Grid
            // Create a WebView that fills the entire Grid
            var webViewProcess = new Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlProcess();
            var webViewControl = (WebView)await webViewProcess.CreateWebViewAsync(this.Grid);
            webViewControl.Navigate(Constants.DefaultNavigationTarget);
        }
    }
}
