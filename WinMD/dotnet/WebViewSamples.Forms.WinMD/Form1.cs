using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Foundation;
using Windows.Web.UI;
using Windows.Web.UI.Interop;

namespace WebViewSamples.Forms.WinMD
{
    public partial class Form1 : Form
    {
        private bool isFullScreen;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            var options = new WebViewControlProcessOptions
            {
                // Enable private network access to enable Enterprise SSO
                PrivateNetworkClientServerCapability = WebViewControlProcessCapabilityState.Enabled
            };
            var process = new WebViewControlProcess(options);
            var control = await process.CreateWebViewControlAsync(
                (long)this.Handle,
                new Windows.Foundation.Rect(0.0f, 0.0f, Width, Height));

            this.Layout += (o, a) =>
            {
                // This event is raised once at startup with the AffectedControl and AffectedProperty properties
                // on the LayoutEventArgs as null.
                if (a.AffectedControl != null && a.AffectedProperty != null)
                {
                    // Ensure that the affected property is the Bounds property to the control
                    if (a.AffectedProperty == nameof(this.Bounds))
                    {
                        // In a typical control the DisplayRectangle is the interior canvas of the control
                        // and in a scrolling control the DisplayRectangle would be larger than the ClientRectangle.
                        // However, that is abstracted from us in WebView so we need to synchronize the ClientRectangle
                        // and permit WebView to handle scrolling based on the new viewport
                        var rect = new Rect(
                            this.ClientRectangle.X,
                            this.ClientRectangle.Y,
                            this.ClientRectangle.Width,
                            this.ClientRectangle.Height);

                        control.Bounds = rect;
                    }
                }
            };

            control.ContainsFullScreenElementChanged += (o, a) =>
            {
                void EnterFullScreen()
                {
                    this.WindowState = FormWindowState.Normal;
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;
                }

                void LeaveFullScreen()
                {
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    this.WindowState = FormWindowState.Normal;
                }

                // Toggle
                this.isFullScreen = !this.isFullScreen;

                if (this.isFullScreen)
                {
                    EnterFullScreen();
                }
                else
                {
                    LeaveFullScreen();
                }
            };

            control.ScriptNotify += (o, a) => { MessageBox.Show(a.Value, a.Uri?.ToString() ?? string.Empty); };

            control.NavigationCompleted += (o, a) =>
            {
                this.Text = o.DocumentTitle;
                if (!a.IsSuccess)
                {
                    MessageBox.Show(
                        $"Could not navigate to {a.Uri}",
                        $"Error: {a.WebErrorStatus}",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            };

            control.NavigationStarting += (o, a) =>
            {
                this.Text = "Navigating " + (a.Uri?.ToString() ?? string.Empty);
            };

            control.PermissionRequested += (o, a) =>
            {
                if (a.PermissionRequest.State == WebViewControlPermissionState.Allow)
                {
                    return;
                }

                var msg = $"Allow {a.PermissionRequest.Uri.Host} to access {a.PermissionRequest.PermissionType}?";

                var response = MessageBox.Show(msg, "Permission Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (response == DialogResult.Yes)
                {
                    if (a.PermissionRequest.State == WebViewControlPermissionState.Defer)
                    {
                        o.GetDeferredPermissionRequestById(a.PermissionRequest.Id, out var permission);
                        permission?.Allow();
                    }
                    else
                    {
                        a.PermissionRequest.Allow();
                    }
                }
                else
                {
                    if (a.PermissionRequest.State == WebViewControlPermissionState.Defer)
                    {
                        o.GetDeferredPermissionRequestById(a.PermissionRequest.Id, out var permission);
                        permission?.Deny();
                    }
                    else
                    {
                        a.PermissionRequest.Deny();
                    }
                }
            };

            control.Navigate(new Uri("http://bing.com"));
        }
    }
}
