using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Win32.UI.Controls.WinForms;

namespace WebViewSamples.Forms.MultipleWebViews
{
    public partial class WebViewControl : UserControl
    {
        public WebViewControl()
        {
            InitializeComponent();

            webView1.IsJavaScriptEnabled = true;
            webView1.Source = new Uri("http://www.fishgl.com");
            webView1.Process.ProcessExited += OnWebViewProcessExited;

            
        }

        private void OnWebViewProcessExited(object o, object e)
        {
            if (webView1 != null)
            {
                Controls.Remove(webView1);
                webView1 = null;
            }

            webView1 = new WebView();
            ((ISupportInitialize)(webView1)).BeginInit();
            webView1.Dock = DockStyle.Fill;
            webView1.Location = new Point(0, 0);
            webView1.MinimumSize = new Size(20, 20);
            webView1.Name = "webView1";
            webView1.Size = new Size(150, 128);
            webView1.TabIndex = 1;
            webView1.NavigationStarting += webView1_NavigationStarting;
            webView1.IsJavaScriptEnabled = true;
            webView1.Source = new Uri("http://www.fishgl.com");
            Controls.Add(webView1);
            ((ISupportInitialize)(webView1)).EndInit();

            // Need the WebView to be initialized so we can hook onto the Process object
            webView1.Process.ProcessExited += OnWebViewProcessExited;
        }

        internal void Terminate()
        {
            webView1.Process.Terminate();
        }

        private void SetProcessIdStatus(WebView webView)
        {
            toolStripStatusLabel1.Text = $"PID: {webView.Process.ProcessId}";
        }

        private void SetWorkingSetStatus(WebView webView)
        {
            var pid = webView?.Process?.ProcessId;
            if (pid.HasValue)
            {
                var process = Process.GetProcessById((int)pid);
                if (process != null)
                {
                    var workingSet = process.WorkingSet64;
                    var workingSetMb = workingSet / 1048576d; // 1024 * 1024
                    SetProcessIdStatus(webView);
                    toolStripStatusLabel1.Text += $" {Math.Round(workingSetMb, 1)} MB";
                }
            }
        }

        private void InvokeIfRequired(MethodInvoker action)
        {
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void webView1_NavigationStarting(object sender, WebViewControlNavigationStartingEventArgs e)
        {
            InvokeIfRequired(() => SetProcessIdStatus(sender as WebView));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            InvokeIfRequired(()=> SetWorkingSetStatus(webView1));
        }
    }
}
