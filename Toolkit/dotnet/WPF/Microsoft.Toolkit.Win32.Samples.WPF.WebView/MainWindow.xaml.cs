// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Win32.UI.Controls;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Win32.Samples.WPF.WebView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isFullScreen;
        private bool processExitedAttached;

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void BrowseBack_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.WebView1 != null && this.WebView1.CanGoBack;
        }

        private void BrowseBack_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.WebView1?.GoBack();
        }

        private void BrowseForward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.WebView1 != null && this.WebView1.CanGoForward;
        }

        private void GoToPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void GoToPage_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var result = (Uri)new WebBrowserUriTypeConverter().ConvertFromString(this.Url.Text);
            this.WebView1.Source = result;
        }

        private void BrowseForward_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.WebView1?.GoForward();
        }

        private void Url_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && this.WebView1 != null)
            {
                var result =
                    (Uri)new WebBrowserUriTypeConverter().ConvertFromString(
                        this.Url.Text);
                this.WebView1.Source = result;
            }
        }

        private void WebView1_OnNavigationCompleted(object sender, WebViewControlNavigationCompletedEventArgs e)
        {
            this.Url.Text = e.Uri?.ToString() ?? string.Empty;
            this.Title = this.WebView1.DocumentTitle;
            if (!e.IsSuccess)
            {
                MessageBox.Show($"Could not navigate to {e.Uri?.ToString() ?? "NULL"}", $"Error: {e.WebErrorStatus}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void WebView1_OnNavigationStarting(object sender, WebViewControlNavigationStartingEventArgs e)
        {
            this.TryAttachProcessExitedEventHandler();
            this.Title = $"Waiting for {e.Uri?.Host ?? string.Empty}";
            this.Url.Text = e.Uri?.ToString() ?? string.Empty;
        }

        private void WebView1_OnPermissionRequested(object sender, WebViewControlPermissionRequestedEventArgs e)
        {
            if (e.PermissionRequest.State == WebViewControlPermissionState.Allow)
            {
                return;
            }

            var msg = $"Allow {e.PermissionRequest.Uri.Host} to access {e.PermissionRequest.PermissionType}?";

            var response = MessageBox.Show(msg, "Permission Request", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

            if (response == MessageBoxResult.Yes)
            {
                if (e.PermissionRequest.State == WebViewControlPermissionState.Defer)
                {
                    this.WebView1.GetDeferredPermissionRequestById(e.PermissionRequest.Id)?.Allow();
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
                    this.WebView1.GetDeferredPermissionRequestById(e.PermissionRequest.Id)?.Deny();
                }
                else
                {
                    e.PermissionRequest.Deny();
                }
            }
        }

        private void WebView1_OnScriptNotify(object sender, WebViewControlScriptNotifyEventArgs e)
        {
            MessageBox.Show(e.Value, e.Uri?.ToString() ?? string.Empty);
        }

        private void WebView1_OnContainsFullScreenElementChanged(object sender, object e)
        {
            void EnterFullScreen()
            {
                this.WindowState = WindowState.Normal;
                this.ResizeMode = ResizeMode.NoResize;
                this.WindowState = WindowState.Maximized;
            }

            void LeaveFullScreen()
            {
                this.ResizeMode = ResizeMode.CanResize;
                this.WindowState = WindowState.Normal;
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

        private void TryAttachProcessExitedEventHandler()
        {
            if (!this.processExitedAttached && this.WebView1?.Process != null)
            {
                this.WebView1.Process.ProcessExited += (o, a) =>
                {
                    // WebView has encountered and error and was terminated
                    this.Close();
                };

                this.processExitedAttached = true;
            }
        }
    }
}
