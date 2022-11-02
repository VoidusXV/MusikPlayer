using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WMPLib;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Net;
using System.Collections.Specialized;
using Windows.Media;
using Microsoft.Toolkit.Forms.UI.Controls;
using Windows.Media.Playback;
namespace MusikPlayer.Methods
{
    public class Global
    {
        public static readonly string current_path = Directory.GetCurrentDirectory();
        public static readonly string playlists_path = Directory.GetCurrentDirectory() + "/Playlists.json";

        public static Design.MessageBox iMessageBox = new Design.MessageBox();

        public static string songDuration = "";
        //public static WindowsMediaPlayer mplayer = new WindowsMediaPlayer();

        //public static bool SongPlaying = false;
        public static int SongPausedTime = 0;


        public static int CurrentPlayingSongInPlaylist = -1;


        public static DataHandling.Playlist Selected_Playlist;


        public static DataHandling.Songs Current_PlayingSong;
        public static DataHandling.Playlist Current_PlayingPlaylist;

        public static DataHandling.ProfileSettings ProfileSettings = new DataHandling.ProfileSettings();
        public static DataHandling.Authentication Authentication = new DataHandling.Authentication();
        public static Structures.Client_Credentials client_Data = new Structures.Client_Credentials();

        public static bool SongEnded = false;
        public static Structures.Loop LoopSongs = Structures.Loop.Off;
        public static bool Shuffle = false;

        public static bool PartyMode = false;
        public static string PartyModeRole = "";
        public static string PartyMode_SessionID = "";

        Design.iToolTip SongNameToolTip;

        public static Structures.Obs_SongTitleExport titleExport = Structures.Obs_SongTitleExport.Off;

        public static List<string> CurrentPlaylistSongsPath = new List<string>();

        public static FileStream fileStream;


        public static MediaPlayerElement mediaPlayerElement = new MediaPlayerElement();
        public static System.Windows.Controls.MediaElement mediaElement = new System.Windows.Controls.MediaElement();
        public static SystemMediaTransportControls systemControls = BackgroundMediaPlayer.Current.SystemMediaTransportControls;

        public static Image resizeImage(Image imgToResize, Size size)
        {
            //Get the image current width  
            int sourceWidth = imgToResize.Width;
            //Get the image current height  
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //Calulate  width with new desired size  
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //Calculate height with new desired size  
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //New Width  
            int destWidth = (int)(sourceWidth * nPercent);
            //New Height  
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // Draw image with new width and height  
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }

        public static void ReadPlaylists(Panel panel, List<Design.iLabel> iLabels)
        {
            int Count = DataHandling.Handler.ReturnJsonFile<DataHandling.Playlist>("Playlists.json").Count;
            panel.Controls.Clear();
            for (int i = 0; i < Count; i++)
            {
                string Content = DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.Playlist>("Playlists.json")[i].PlaylistName;
                CreateControls.Create_iLabel(iLabels, i, Content, new Font("Microsoft Sans Serif", 10, FontStyle.Regular), new Point(0, -35 + (i * 35)),
                  new Size(panel.Width, 50), false, true);

                if (i > 0)
                    panel.Controls.Add(iLabels[i]);
            }

        }

        public static Image SetImageOpacity(Image image, float opacity)
        {
            try
            {
                Bitmap bmp = new Bitmap(image.Width, image.Height);
                using (Graphics gfx = Graphics.FromImage(bmp))
                {
                    ColorMatrix matrix = new ColorMatrix();
                    matrix.Matrix33 = opacity;
                    ImageAttributes attributes = new ImageAttributes();
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
                return bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public static bool CheckOpened(string name)
        {
            FormCollection fc = Application.OpenForms;

            foreach (Form frm in fc)
            {
                if (frm.Text == name)
                {
                    return true;
                }
            }
            return false;
        }


        public static void SetHandlerVariables()
        {
            try
            {
                if (DataHandling.Handler.ReturnJsonFile<DataHandling.Authentication>(Global.current_path + "/ProfileSettings.json").Count > 0)
                    ProfileSettings = DataHandling.Handler.ReturnJsonFile<DataHandling.ProfileSettings>(Global.current_path + "/ProfileSettings.json")[0];

                if (DataHandling.Handler.ReturnJsonFile<DataHandling.Authentication>(Global.current_path + "/Authentication.json").Count > 0)
                {
                    Authentication = DataHandling.Handler.ReturnJsonFile<DataHandling.Authentication>(Global.current_path + "/Authentication.json")[0];
                }
            }
            catch { }
        }


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

        public static void Error_Debugger(string ErrorMessage)
        {
            File.AppendAllText($"{current_path}/Error_Debugger.txt", $"{DateTime.Now}: {ErrorMessage}\n");
        }
        public static Image ImageStream(string ImageURL)
        {
            using (Stream imageStream = File.OpenRead(ImageURL))
            {
                return Image.FromStream(imageStream);
            }
        }
        public static void Discord_Presence(string SongName, string SongArtist)
        {
            Methods.Discord.PresenceManager.discordPresenceDetail = $"{SongArtist} - {SongName}";
            // Methods.Discord.PresenceManager.discordPresenceState = $"{CurrentPos} - {SongDuration}";

            Methods.Discord.PresenceManager.discordLargeImageKey = "profileimage_placeholder";
            Methods.Discord.PresenceManager.discordLargeImageText = "BlubKopf";

            Methods.Discord.PresenceManager.discordSmallImageKey = "musicedy_logo_fullhd";
            Methods.Discord.PresenceManager.discordSmallImageText = "Musicedy";

            Methods.Discord.PresenceManager.UpdatePresence();
        }

        public static void Discord_Presence_Duration(string CurrentPos, string SongDuration)
        {
            Methods.Discord.PresenceManager.discordPresenceState = $"{CurrentPos} - {SongDuration}";
            Methods.Discord.PresenceManager.UpdatePresence();
        }

        public static void SetToolTip(Control control, string Text, Design.iToolTip iToolTip)
        {
            //Design.iToolTip iToolTip = new Design.iToolTip();
            //iToolTip = new Design.iToolTip();
            if (iToolTip == null)
            {
                Console.WriteLine("SetToolTip keine Instanz");
                return;
            }
            iToolTip.AutomaticDelay = 300;
            iToolTip.RemoveAll();
            iToolTip.SetToolTip(control, Text);
        }

        public static bool isOnCurrentSongAndPlaylist(int currentSelection)
        {
            if (currentSelection == Global.Current_PlayingSong.Index && Global.Selected_Playlist.PlaylistName == Global.Current_PlayingPlaylist.PlaylistName)
            {
                return true;
            }
            return false;
        }

        #region GetControl
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point pnt);
        public static string GetControl(string ContainsText = "")
        {
            try
            {

                IntPtr hWnd = WindowFromPoint(Control.MousePosition);
                if (hWnd != IntPtr.Zero)
                {
                    Control ctl = Control.FromHandle(hWnd);

                    if (ctl != null && ctl.Name.Contains(ContainsText) && ctl.Visible == true && !string.IsNullOrEmpty(ContainsText))
                    {
                        return ctl.Name;
                    }
                    else if(ctl != null && ctl.Name.Contains(ContainsText) && ctl.Visible == true && string.IsNullOrEmpty(ContainsText))
                    {
                        return ctl.Name;
                    }
                }
            }
            catch { }
            return "-1";

        }
        #endregion
    }
}
