using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Microsoft.Toolkit.Win32.UI.Controls.WPF;

namespace WebViewSamples.WPF.WebViewControlProcess
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

        private async void OnWindowLoad(object sender, RoutedEventArgs e)
        {
            // Default MainWindow.xml with named Grid
            // Create a WebView that fills the entire Grid
            var webViewProcess = new Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlProcess();
            var hwndSource = (HwndSource)HwndSource.FromVisual(this.Grid);
            var webViewControl = (WebView)await webViewProcess.CreateWebViewAsync(hwndSource.Handle, new Rect(new Size(this.Grid.ActualWidth, this.Grid.ActualHeight)));
            Grid.SetColumn(webViewControl, 0);
            webViewControl.Navigate(Constants.DefaultNavigationTarget);
        }
    }
}
