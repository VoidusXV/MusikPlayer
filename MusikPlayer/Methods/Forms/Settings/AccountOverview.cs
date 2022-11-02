using Firebase.Auth;
using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods.Forms.Settings
{
    public partial class AccountOverview : Form
    {
        bool ShowPassword = false;
        public AccountOverview()
        {
            InitializeComponent();
            CopiedLabel.AlphaColor = 0;
            iLabel6.Text = $"{Global.Authentication.Username}#{Global.Authentication.UsernameID}";
            iLabel7.Text = Global.Authentication.Email;
            iLabel8.Text = new string('*', Global.Authentication.Password.Length);
            iLabel9.Text = Global.client_Data.Register_Date;

            /*if (File.Exists($"{Global.current_path}/profileImage.png"))
            iPictureBox1.Image = Image.FromFile($"{Global.current_path}/profileImage.png");
            else
            iPictureBox1.Image = Resources_Images.Images.Icons.profileImage_Placeholder;*/

            Client_DataHandling.GetProfileImage(iRoundedPictureBox1);
        }

    

        public static Thread Thread_CopiedAnimation;
        bool ThreadsRunning = false;

        public void CopiedAnimation()
        {
            ThreadsRunning = true;
            for (int i = 0; i <= 255; i += 5)
            {
                CopiedLabel.AlphaColor = i;
                Thread.Sleep(10);
            }
            for (int i = 255; i >= 0; i -= 5)
            {
                CopiedLabel.AlphaColor = i;
                Thread.Sleep(10);
            }
            ThreadsRunning = false;
        }

        private void iLabel6_Click(object sender, EventArgs e)
        {
            int X_Middle = iLabel6.Location.X + (iLabel6.Width - CopiedLabel.Width) / 2;
            CopiedLabel.Location = new Point(X_Middle, iLabel6.Location.Y - 20);
            Thread_CopiedAnimation = new Thread(CopiedAnimation);
            if (ThreadsRunning == false)
            {
                Thread_CopiedAnimation.Start();
            }
            Clipboard.SetText(iLabel6.Text);
        }

        private void iLabel7_Click(object sender, EventArgs e)
        {
            int X_Middle = iLabel7.Location.X + (iLabel7.Width - CopiedLabel.Width) / 2;
            CopiedLabel.Location = new Point(X_Middle, iLabel7.Location.Y - 20);
            Thread_CopiedAnimation = new Thread(CopiedAnimation);
            if (ThreadsRunning == false)
            {
                Thread_CopiedAnimation.Start();
            }
            Clipboard.SetText(iLabel7.Text);
        }

        private void iCircleButton1_Click(object sender, EventArgs e)
        {
            if (ShowPassword == false)
            {
                iLabel8.Text = Global.Authentication.Password;
                ShowPassword = true;
            }
            else
            {
                iLabel8.Text = new string('*', Global.Authentication.Password.Length);
                ShowPassword = false;
            }
        }
        public static Thread Background_UploadData_Thread;//= new Thread(Background_Upload_MetaData);


        public static void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc, WebProxy myproxy = null)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            if (myproxy != null)
                wr.Proxy = myproxy;
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);

            }
            catch (Exception ex)
            {

                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
        }

        public static void UploadData(string FilePath, NameValueCollection nameValueCollection)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            WebProxy myproxy = new WebProxy("zproxy.lum-superproxy.io", 22225);
            myproxy.Credentials = new NetworkCredential("lum-customer-hl_758e55cc-zone-zone1-route_err-pass_dyn", "y29oc6wsuy9a");
            HttpUploadFile("https://www.hlde1.online/Musicedy/Server/uploadData.php", FilePath, "file", "application/octet-stream", nameValueCollection, myproxy);
        }
        public static void Background_Upload_MetaData(string FilePath)
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            nameValueCollection.Add("FileUploading", "");
            nameValueCollection.Add("Username", Global.Authentication.Username);
            nameValueCollection.Add("UsernameID", Global.Authentication.UsernameID);
            UploadData(FilePath, nameValueCollection);
            Background_UploadData_Thread.Abort();
        }



        private async void iRoundedPictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "All Files|*.*";
                //openFileDialog.Filter = "PNG|*.png|JPEG|*.jpeg";

                DialogResult dr = openFileDialog.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    iRoundedPictureBox1.Image = Global.ImageStream(openFileDialog.FileName);
                    var stream = File.Open(openFileDialog.FileName, FileMode.Open);
                    await Authentication.FirestoreGlobal.firebaseStorage.Child("Clients").Child(Global.client_Data.folderID).Child("profileImage.png").PutAsync(stream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Global.iMessageBox.Show("Ein Fehler ist aufgetreten", "Fehler");
            }
        }
    }
}
