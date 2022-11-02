using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods.Updater
{
    public partial class UpdateWarning : Form
    {
        string current_path = Directory.GetCurrentDirectory();

        public UpdateWarning()
        {
            InitializeComponent();
            iLabel1.BackColor = Color.Transparent;
            iLabel1.Parent = gradientBackground1;

            Downloader_Load();
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
                webClient.DownloadFileAsync(new Uri("https://www.hlde1.online/Musicedy/Latest/Updater.exe"), current_path + "/Updater.exe");
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);
            }
            catch
            {
                MessageBox.Show("Ein Fehler ist aufgetreten", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Process.Start($"{Global.current_path}/Updater.exe");
            Application.Exit();
        }

        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {

        }
    }
}
