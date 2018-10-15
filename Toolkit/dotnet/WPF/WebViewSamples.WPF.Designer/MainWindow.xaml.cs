using System.Windows;

namespace WebViewSamples.WPF.Designer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            // Can also use the Source property in XAML
            this.WebView.Navigate(Constants.DefaultNavigationTarget);
        }
    }
}
