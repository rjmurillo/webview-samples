// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Forms;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Win32.UI.Controls.WinForms;

namespace Microsoft.Toolkit.Win32.Samples.WinForms.WebView
{
    public partial class Form1 : Form
    {
        private bool isFullScreen;

        private bool processExitedAttached;

        public Form1()
        {
            this.InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.webView1?.GoBack();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.webView1?.GoForward();
        }

        private void Go_Click(object sender, EventArgs e)
        {
            var result = (Uri)new WebBrowserUriTypeConverter().ConvertFromString(this.url.Text);
            this.webView1.Source = result;
        }

        private void OnFormLoaded(object sender, EventArgs e)
        {
            this.TryAttachProcessExitedEventHandler();
        }

        private void TryAttachProcessExitedEventHandler()
        {
            if (!this.processExitedAttached && this.webView1?.Process != null)
            {
                this.webView1.Process.ProcessExited += (o, a) =>
                {
                    // WebView has encountered and error and was terminated
                    this.Close();
                };

                this.processExitedAttached = true;
            }
        }

        private void Url_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && this.webView1 != null)
            {
                var result = (Uri)new WebBrowserUriTypeConverter().ConvertFromString(this.url.Text);
                this.webView1.Source = result;
            }
        }

        private void WebView1_ContainsFullScreenElementChanged(object sender, object e)
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
        }

        private void WebView1_NavigationCompleted(object sender, WebViewControlNavigationCompletedEventArgs e)
        {
            this.TryAttachProcessExitedEventHandler();
            this.url.Text = e.Uri?.ToString() ?? string.Empty;
            this.Text = this.webView1.DocumentTitle;
            if (!e.IsSuccess)
            {
                MessageBox.Show(
                    $"Could not navigate to {e.Uri}",
                    $"Error: {e.WebErrorStatus}",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void WebView1_NavigationStarting(object sender, WebViewControlNavigationStartingEventArgs e)
        {
            this.Text = "Navigating " + e.Uri?.ToString() ?? string.Empty;
            this.url.Text = e.Uri?.ToString() ?? string.Empty;
        }

        private void WebView1_PermissionRequested(object sender, WebViewControlPermissionRequestedEventArgs e)
        {
            if (e.PermissionRequest.State == WebViewControlPermissionState.Allow)
            {
                return;
            }

            var msg = $"Allow {e.PermissionRequest.Uri.Host} to access {e.PermissionRequest.PermissionType}?";

            var response = MessageBox.Show(msg, "Permission Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (response == DialogResult.Yes)
            {
                if (e.PermissionRequest.State == WebViewControlPermissionState.Defer)
                {
                    this.webView1.GetDeferredPermissionRequestById(e.PermissionRequest.Id)?.Allow();
                }
                else
                {
                    e.PermissionRequest.Allow();
                }
            }
            else
            {
                if (e.PermissionRequest.State == WebViewControlPermissionState.Defer)
                {
                    this.webView1.GetDeferredPermissionRequestById(e.PermissionRequest.Id)?.Deny();
                }
                else
                {
                    e.PermissionRequest.Deny();
                }
            }
        }

        private void WebView1_ScriptNotify(object sender, WebViewControlScriptNotifyEventArgs e)
        {
            MessageBox.Show(e.Value, e.Uri?.ToString() ?? string.Empty);
        }
    }
}
