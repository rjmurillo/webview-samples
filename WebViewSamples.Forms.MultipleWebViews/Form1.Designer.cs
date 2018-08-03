namespace WebViewSamples.Forms.MultipleWebViews
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.webViewControl1 = new WebViewSamples.Forms.MultipleWebViews.WebViewControl();
            this.webViewControl2 = new WebViewSamples.Forms.MultipleWebViews.WebViewControl();
            this.webViewControl3 = new WebViewSamples.Forms.MultipleWebViews.WebViewControl();
            this.webViewControl4 = new WebViewSamples.Forms.MultipleWebViews.WebViewControl();
            this.terminateRandomWebViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.webViewControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.webViewControl2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.webViewControl3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.webViewControl4, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1560, 1122);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.terminateRandomWebViewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1560, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // webViewControl1
            // 
            this.webViewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webViewControl1.Location = new System.Drawing.Point(3, 3);
            this.webViewControl1.Name = "webViewControl1";
            this.webViewControl1.Size = new System.Drawing.Size(774, 555);
            this.webViewControl1.TabIndex = 0;
            // 
            // webViewControl2
            // 
            this.webViewControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webViewControl2.Location = new System.Drawing.Point(783, 3);
            this.webViewControl2.Name = "webViewControl2";
            this.webViewControl2.Size = new System.Drawing.Size(774, 555);
            this.webViewControl2.TabIndex = 1;
            // 
            // webViewControl3
            // 
            this.webViewControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webViewControl3.Location = new System.Drawing.Point(3, 564);
            this.webViewControl3.Name = "webViewControl3";
            this.webViewControl3.Size = new System.Drawing.Size(774, 555);
            this.webViewControl3.TabIndex = 2;
            // 
            // webViewControl4
            // 
            this.webViewControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webViewControl4.Location = new System.Drawing.Point(783, 564);
            this.webViewControl4.Name = "webViewControl4";
            this.webViewControl4.Size = new System.Drawing.Size(774, 555);
            this.webViewControl4.TabIndex = 3;
            // 
            // terminateRandomWebViewToolStripMenuItem
            // 
            this.terminateRandomWebViewToolStripMenuItem.Name = "terminateRandomWebViewToolStripMenuItem";
            this.terminateRandomWebViewToolStripMenuItem.Size = new System.Drawing.Size(172, 20);
            this.terminateRandomWebViewToolStripMenuItem.Text = "Terminate Random WebView";
            this.terminateRandomWebViewToolStripMenuItem.Click += new System.EventHandler(this.terminateRandomWebViewToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1560, 1146);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Form1";
            this.Text = "Multiple WebViews with Crash Recovery";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private WebViewControl webViewControl1;
        private WebViewControl webViewControl2;
        private WebViewControl webViewControl3;
        private WebViewControl webViewControl4;
        private System.Windows.Forms.ToolStripMenuItem terminateRandomWebViewToolStripMenuItem;
    }
}

