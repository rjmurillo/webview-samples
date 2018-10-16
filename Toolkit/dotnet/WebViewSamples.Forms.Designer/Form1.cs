using System.Windows.Forms;

namespace WebViewSamples.Forms.Designer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.InitializeComponent();
            this.webView1.Navigate(Constants.DefaultNavigationTarget);
        }
    }
}
