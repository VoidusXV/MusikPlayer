using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Updater
{
    public partial class Form1 : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        string current_path = Directory.GetCurrentDirectory();

        public Form1()
        {
            InitializeComponent();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            if (File.Exists($"{current_path}/Musicedy.exe"))
                File.Delete($"{current_path}/Musicedy.exe");

            Downloader_Load();

            panel1.Parent = gradientBackground1;
            label1.BackColor = Color.Transparent;
            label1.Parent = gradientBackground1;
            label2.BackColor = Color.Transparent;
            label2.Parent = gradientBackground1;
            LabelUpdaterTimer.Start();

        }
        private string FormatBytes(long bytes, int decimalPlaces, bool showByteType)
        {
            double newBytes = bytes;
            string formatString = "{0";
            string byteType = "B";

            // Check if best size in KB
            if (newBytes > 1024 && newBytes < 1048576)
            {
                newBytes /= 1024;
                byteType = "KB";
            }
            else if (newBytes > 1048576 && newBytes < 1073741824)
            {
                // Check if best size in MB
                newBytes /= 1048576;
                byteType = "MB";
            }
            else
            {
                // Best size in GB
                newBytes /= 1073741824;
                byteType = "GB";
            }

            // Show decimals
            if (decimalPlaces > 0)
                formatString += ":0.";

            // Add decimals
            for (int i = 0; i < decimalPlaces; i++)
                formatString += "0";

            // Close placeholder
            formatString += "}";

            // Add byte type
            if (showByteType)
                formatString += byteType;

            return String.Format(formatString, newBytes);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point((this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);
                this.Update();
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;

        }

        int LabelState = 0;
        private void LabelUpdaterTimer_Tick(object sender, EventArgs e)
        {
            if (LabelState > 3)
                LabelState = 0;

            if (LabelState == 0)
                label2.Text = "Updating Musicedy";
            if (LabelState == 1)
                label2.Text = "Updating Musicedy.";
            if (LabelState == 2)
                label2.Text = "Updating Musicedy..";
            if (LabelState == 3)
                label2.Text = "Updating Musicedy...";
            LabelState++;
        }



        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Process.Start($"{current_path}/Musicedy.exe");
                Application.Exit();
            }
            else
            {
                //MessageBox.Show(e.Error.Message);
                MessageBox.Show("Ein Fehler ist aufgetreten", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
          ((WebClient)sender).Dispose();

        }
        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            label1.Visible = true;
            this.label1.Text = String.Format("Downloaded {0} of {1}", FormatBytes(e.BytesReceived, 1, true), FormatBytes(e.TotalBytesToReceive, 1, true));
            this.progressBar1.Value = e.ProgressPercentage;
        }
        private void Downloader_Load()
        {
            try
            {
                WebProxy myproxy = new WebProxy("zproxy.lum-superproxy.io", 22225);
                myproxy.Credentials = new NetworkCredential("lum-customer-hl_758e55cc-zone-zone1-route_err-pass_dyn", "y29oc6wsuy9a");
                WebClient webClient = new WebClient();
                webClient.Proxy = myproxy;

                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
                webClient.DownloadFileAsync(new Uri("http://www.hlde1.online/Musicedy/Latest/Musicedy.exe"), current_path + "/Musicedy.exe");
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);
            }
            catch
            {
                MessageBox.Show("Ein Fehler ist aufgetreten", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
