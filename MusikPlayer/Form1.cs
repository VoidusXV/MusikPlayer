using DiscordRPC;
using MusikPlayer.Methods;
using MusikPlayer.Methods.Authentication;
using MusikPlayer.Methods.Partymode;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Runtime.Caching;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Media;
using Windows.Media.Core;
using Windows.Storage.Streams;
using WMPLib;
using YoutubeExplode;

namespace MusikPlayer
{
    public partial class Form1 : Form
    {
        private bool mouseDown;
        private Point lastLocation;

        Methods.Forms.Friendlist.Main_FriendList friendlist; //= new Methods.Forms.Friendlist.Main_FriendList();
        Methods.Forms.Friendlist.FriendsForm friendsForm;
        public Methods.Forms.MainPlayer mainPlayer;

        public Design.iContextStripMenu iContextStripMenu;
        public Design.iPictureBox CurrentPlaying_Icon = CreateControls.iPictureBoxs(Resources_Images.Images.Icons.greenSound_Icon);

        //Song Variables
        public double pausedSongTime = 0;
        int Volume = 0;

        public Form1()
        {
            InitializeComponent();
            friendsForm = new Methods.Forms.Friendlist.FriendsForm();

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.Size = new Size(1700, 900);
            this.Text = "Musicedy Beta Version";
            FormFade.Start();

            CheckPlayingSongExists();
            iTrackBar2.MouseWheel += ITrackBar2_MouseWheel;
            Volume = iTrackBar2.Value;
            pictureBox1.Image = Global.SetImageOpacity(pictureBox1.Image, 0.5f);
            iLabel4.AlphaColor = 90;

            MediaPlayerControlsValues();
            //Methods.Discord.Window.LoadUserStatePresence();

            FavouriteSongExists();
            SetPlayingIcon();
            DeleteUpdater();
            MarkPlayingSong();
            Global.ReadPlaylists(panel6, CreateControls.iLabels);
            Add_PlaylistLabel_Controls();
            WireMouseEvents(panel4);
            Global.SetHandlerVariables();
            ContextStripSettings();
        }

        void MediaPlayerControlsValues()
        {
            Global.mediaPlayerElement.AreTransportControlsEnabled = true;

            Global.mediaElement.LoadedBehavior = System.Windows.Controls.MediaState.Manual;
            Global.mediaElement.UnloadedBehavior = System.Windows.Controls.MediaState.Manual;
            Global.systemControls = SystemMediaTransportControls.GetForCurrentView();
            Global.systemControls.ButtonPressed += SongPlayer.SystemControls_ButtonPressed;
            Global.systemControls.IsPlayEnabled = true;
            Global.systemControls.IsPauseEnabled = true;
            Global.systemControls.IsNextEnabled = true;
            Global.systemControls.IsPreviousEnabled = true;
            Global.systemControls.PlaybackStatus = MediaPlaybackStatus.Closed;

        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            //Methods.Discord.Window.LoadUserStatePresence();

            if (FilesDownloader.IsBusy == false)
                FilesDownloader.RunWorkerAsync();
        }
        private async void Form1_Load(object sender, EventArgs e)
        {

            AdjustControls();
            await friendsForm.Get_FriendInfo_Firestore();
            await SetStatus("Online");

            string IP = new WebClient().DownloadString("https://api.ipify.org");
            string Login_Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")).ToString("dd.MM.yyyy");
            await FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).Collection("data").Document("credentials").UpdateAsync(new Dictionary<string, object> { { "IP", IP } });
            await FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).Collection("data").Document("credentials").UpdateAsync(new Dictionary<string, object> { { "Last_Login", Login_Date } });
        }

        private void DeleteUpdater()
        {
            if (File.Exists($"{Global.current_path}/Updater.exe"))
                File.Delete($"{Global.current_path}/Updater.exe");
        }
        private void SetPlayingIcon()
        {
            CurrentPlaying_Icon.BackColor = Color.Transparent;
            CurrentPlaying_Icon.SizeMode = PictureBoxSizeMode.StretchImage;
            CurrentPlaying_Icon.Size = new Size(15, 15);
        }

        private void FavouriteSongExists()
        {

            string PlaylistPath = Global.current_path + $"/Playlists/FavouriteSongs";
            string FileName = $"{Global.current_path}/Playlists.json";
            List<Methods.DataHandling.Playlist> Playlist = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.Playlist>(FileName);
            if (!Directory.Exists(PlaylistPath))
            {
                Directory.CreateDirectory(PlaylistPath);
                int Count = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.Songs>(FileName).Count;
                if (Count == 0)
                    Methods.DataHandling.Handler.CreateJsonFile_Playlist(0, 0, DateTime.Now.ToString("MM/dd/yyyy"), DateTime.Now.ToString("HH:mm"), "FavouriteSongs", PlaylistPath, $"{PlaylistPath}/FavouriteSongs.json");
            }
            /*else if (Playlist[0].PlaylistName != "FavouriteSongs")
            {
                var jsonParsed = JsonObject.Parse(File.ReadAllText(FileName));
                Console.WriteLine(jsonParsed);
            }*/
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            /*var region = new Region(new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height));
            region.Exclude(Top);
            region.Exclude(Left);
            region.Exclude(Bottom);
            region.Exclude(Right);

            this.panel8.Region = region;
            this.Invalidate();*/
        }

        public async Task SetStatus(string myStatus)
        {
            await FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).UpdateAsync(new Dictionary<string, object> { { "Status", myStatus } });
        }





        void DisableControls()
        {
            this.Size = new Size(1700, 900);
            iCircleButton1.Enabled = false;
            iCircleButton2.Enabled = false;
            iCircleButton3.Enabled = false;
            iTrackBar1.Enabled = false;
            iTrackBar1.Value = 0;
            panel10.Visible = false;
            label2.Text = "00:00";
        }
        public void DisposeImages()
        {
            if (this.iPictureBox5.Image != null)
                this.iPictureBox5.Image.Dispose();
            if (mainPlayer != null)
            {
                for (int i = 0; i < mainPlayer.ImageList.Count; i++)
                    mainPlayer.ImageList[i].Dispose();
            }
        }

        void ContextStripSettings()
        {
            iContextStripMenu = new Design.iContextStripMenu();
            iContextStripMenu.AddOption("Bearbeiten", PlaylistEdit_Click);
            iContextStripMenu.AddOption("Löschen", PlaylistDelete_Click);
            iContextStripMenu.AddOption("Playlist erstellen", iLabel3_Click);
            iContextStripMenu.AddLine();
            iContextStripMenu.AddOption("Teilen", PlaylistLink_Click);
        }
        private void PlaylistEdit_Click(object sender, EventArgs e)
        {
            try
            {
                //int index = Convert.ToInt32(GetControl().Split('_')[1]);

                Methods.Forms.Edit.EditPlaylist editPlaylist = new Methods.Forms.Edit.EditPlaylist();
                editPlaylist.TopMost = true;
                editPlaylist.PlaylistIndex = old_Selected_Label;
                editPlaylist.Show(CreateControls.iLabels[old_Selected_Label].Text);
                this.Opacity = 0.99f;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void PlaylistDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DisposeImages();
                string FileName = $"{Global.current_path}/Playlists.json";
                int Count = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.Songs>(FileName).Count;
                List<Methods.DataHandling.Playlist> Playlist = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.Playlist>(FileName);
                var jsonParsed = JsonObject.Parse(File.ReadAllText(FileName));

                if (Directory.Exists($"{Global.current_path}/Playlists/{CreateControls.iLabels[old_Selected_Label].Text}"))
                    Directory.Delete($"{Global.current_path}/Playlists/{CreateControls.iLabels[old_Selected_Label].Text}", true);

                jsonParsed.AsArray().RemoveAt(old_Selected_Label);
                if (old_Selected_Label == Count)
                    return;

                List<Methods.DataHandling.Playlist> newJson = JsonConvert.DeserializeObject<List<Methods.DataHandling.Playlist>>(jsonParsed.ToString());

                for (int i = old_Selected_Label; i < newJson.Count; i++)
                {
                    newJson[i].ID--;
                    newJson[i].Index--;
                }

                if (Global.Selected_Playlist.PlaylistName == CreateControls.iLabels[old_Selected_Label].Text)
                    panel7.Controls.Clear();

                if (Global.Selected_Playlist.PlaylistName == Global.Current_PlayingPlaylist.PlaylistName)
                {
                    SongPlayer.PauseSong();
                    SongPlayer.ResetPlayer();
                }

                string jsonData = JsonConvert.SerializeObject(newJson, Formatting.Indented);
                File.WriteAllText(FileName, jsonData);
                ContextMenuStrip_FocusCheckTimer.Stop();
                Global.ReadPlaylists(panel6, CreateControls.iLabels);
                iContextStripMenu.Hide();

                Console.WriteLine($"kok: {CreateControls.iLabels[old_Selected_Label].Text}");
                //Methods.Firebase_Methods.Storage_Methods.DeletePlaylist(CreateControls.iLabels[old_Selected_Label].Text);
            }
            catch (Exception ex)
            {
                Global.iMessageBox.Show(ex.Message);
            }

        }
        private void PlaylistLink_Click(object sender, EventArgs e)
        {
            Global.iMessageBox.Show("Link");
        }

        private void ContextMenuStrip_FocusCheckTimer_Tick(object sender, EventArgs e)
        {
            if (iContextStripMenu.ContainsFocus == false)
            {
                iContextStripMenu.Hide();
                ContextMenuStrip_FocusCheckTimer.Stop();
            }
        }

        void Download_FFMPEG()
        {
            WebProxy myproxy;
            WebClient web;
            if (!Directory.Exists(Global.current_path + "/ffmpeg"))
            {
                myproxy = new WebProxy("zproxy.lum-superproxy.io", 22225);
                myproxy.Credentials = new NetworkCredential("lum-customer-hl_758e55cc-zone-zone1-route_err-pass_dyn", "y29oc6wsuy9a");
                web = new WebClient();
                web.Proxy = myproxy;
                Directory.CreateDirectory(Global.current_path + "/ffmpeg");
                Directory.CreateDirectory(Global.current_path + "/ffmpeg/__MACOSX");
                web.DownloadFile("https://www.hlde1.online/Musicedy/Placeholder/ffmpeg/ffmpeg.exe", $"{Global.current_path}/ffmpeg/ffmpeg.exe");
                web.DownloadFile("https://www.hlde1.online/Musicedy/Placeholder/ffmpeg/__MACOSX/._ffmpeg.exe", $"{Global.current_path}/ffmpeg/__MACOSX/._ffmpeg.exe");
            }
            else if (!File.Exists(Global.current_path + "/ffmpeg/__MACOSX/._ffmpeg.exe"))
            {
                myproxy = new WebProxy("zproxy.lum-superproxy.io", 22225);
                myproxy.Credentials = new NetworkCredential("lum-customer-hl_758e55cc-zone-zone1-route_err-pass_dyn", "y29oc6wsuy9a");
                web = new WebClient();
                web.Proxy = myproxy;
                Directory.CreateDirectory(Global.current_path + "/ffmpeg");
                Directory.CreateDirectory(Global.current_path + "/ffmpeg/__MACOSX");
                web.DownloadFile("https://www.hlde1.online/Musicedy/Placeholder/ffmpeg/__MACOSX/._ffmpeg.exe", $"{Global.current_path}/ffmpeg/__MACOSX/._ffmpeg.exe");
            }
            else if (!File.Exists(Global.current_path + "/ffmpeg/ffmpeg.exe"))
            {
                myproxy = new WebProxy("zproxy.lum-superproxy.io", 22225);
                myproxy.Credentials = new NetworkCredential("lum-customer-hl_758e55cc-zone-zone1-route_err-pass_dyn", "y29oc6wsuy9a");
                web = new WebClient();
                web.Proxy = myproxy;
                Directory.CreateDirectory(Global.current_path + "/ffmpeg");
                Directory.CreateDirectory(Global.current_path + "/ffmpeg/__MACOSX");
                web.DownloadFile("https://www.hlde1.online/Musicedy/Placeholder/ffmpeg/ffmpeg.exe", $"{Global.current_path}/ffmpeg/ffmpeg.exe");
            }
        }

        private void FilesDownloader_DoWork(object sender, DoWorkEventArgs e)
        {
            Download_FFMPEG();
        }

        public void CheckPlayingSongExists()
        {
            try
            {
                int count = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.FormSettings>("AppSettings.json").Count;
                if (count > 0)
                {
                    if (Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.FormSettings>("AppSettings.json")[0].songs == null)
                    {
                        DisableControls();
                    }
                    else
                    {
                        Methods.DataHandling.Songs songs = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.FormSettings>("AppSettings.json")[0].songs;
                        Methods.DataHandling.Playlist playlist = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.FormSettings>("AppSettings.json")[0].playlist;

                        int Volume = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.FormSettings>("AppSettings.json")[0].Volume;
                        int SongPausedTime = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.FormSettings>("AppSettings.json")[0].SongPausedTime;
                        string FormWindowState = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.FormSettings>("AppSettings.json")[0].FormWindowState;
                        int PlaylistIndex = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.FormSettings>("AppSettings.json")[0].PlaylistIndex;
                        Size FormSize = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.FormSettings>("AppSettings.json")[0].FormSize;
                        Global.Shuffle = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.FormSettings>("AppSettings.json")[0].Shuffle;
                        Global.LoopSongs = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.FormSettings>("AppSettings.json")[0].Loop;

                        Global.Current_PlayingSong = songs;
                        Global.Selected_Playlist = playlist;

                        if (FormWindowState == "Normal")
                            this.Size = FormSize;
                        else if (FormWindowState == "Maximized")
                            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

                        if (Global.LoopSongs == Structures.Loop.Off)
                            iCircleButton4.Image = Global.resizeImage(Resources_Images.Images.Icons.loop_64, new Size(20, 20));
                        else if (Global.LoopSongs == Structures.Loop.LoopPlaylist)
                            iCircleButton4.Image = Global.resizeImage(Resources_Images.Images.Icons.loop_green, new Size(20, 20));
                        else if (Global.LoopSongs == Structures.Loop.LoopSong)
                            iCircleButton4.Image = Global.resizeImage(Resources_Images.Images.Icons.Loop_1_Song, new Size(20, 20));

                        if (Global.Shuffle == false)
                            iCircleButton5.Image = Global.resizeImage(Resources_Images.Images.Icons.shuffle, new Size(20, 20));
                        else if (Global.Shuffle == true)
                            iCircleButton5.Image = Global.resizeImage(Resources_Images.Images.Icons.shuffle_green, new Size(20, 20));


                        Global.SongPausedTime = SongPausedTime;
                        using (Stream imageStream = File.OpenRead(songs.ImagePath))
                        {
                            iPictureBox5.Image = Image.FromStream(imageStream);
                            //iPictureBox5.Image = Image.FromFile(songs.ImagePath);
                        }
                        label1.Text = SongPlayer.currentTimeFormat(SongPausedTime);
                        label2.Text = songs.SongDuration;
                        label3.Text = Volume.ToString();

                        iLabel7.Text = songs.SongName;
                        iLabel8.Text = songs.SongAuthor;

                        iTrackBar1.MaxValue = SongPlayer.SongDurationToSeconds(songs.SongDuration);
                        iTrackBar1.Value = SongPausedTime;
                        iTrackBar2.Value = Volume;
                        Global.mediaElement.Volume = ((double)Volume) / 100;

                        Global.Selected_Playlist.Index = PlaylistIndex;

                        SongPlayer.UpdateCurrentPlaylist(Global.Selected_Playlist.Index);

                    }
                }
                else
                {
                    DisableControls();
                }
            }
            catch
            {
                DisableControls();
            }
        }



        public void AdjustControls()
        {

            //if (panel9.Width != 700)
            panel9.Size = new Size(700, panel9.Height);

            panel10.AutoSize = false;

            //panel10.Size = new Size(panel9.Location.X - panel10.Location.X, panel10.Height);

            int distPanels = panel2.Location.X - (panel10.Location.X + panel10.Width);

            if (distPanels < panel9.Width)
                panel9.Width = distPanels - 50;

            int LenthOfPanels = panel10.Location.X + panel10.Width + panel2.Location.X;
            int MiddleOf2Panels = (LenthOfPanels - panel9.Width) / 2;
            panel9.Location = new Point(MiddleOf2Panels, (iPanel21.Height - panel9.Height) / 2);

            label2.Location = new Point(iTrackBar1.Width + 60, 56);
            iLabel2.Location = new Point((iPanel1.Width - iLabel2.Width) / 2, (iPanel1.Height - iLabel2.Height) / 2);

            int Distance_ToNextButton = 15;
            iCircleButton1.Location = new Point((iTrackBar1.Width + iCircleButton1.Width) / 2, 10);
            iCircleButton2.Location = new Point(iCircleButton1.Location.X + iCircleButton1.Width + Distance_ToNextButton, 10);
            iCircleButton3.Location = new Point(iCircleButton1.Location.X - (Distance_ToNextButton + iCircleButton3.Width), 10);

            iCircleButton4.Location = new Point(iCircleButton2.Location.X + iCircleButton2.Width + Distance_ToNextButton, 10);
            iCircleButton5.Location = new Point(iCircleButton3.Location.X - iCircleButton3.Width - Distance_ToNextButton, 10);


            if (friendlist != null)
            {
                friendlist.Size = new Size(friendlist.Width, this.Height - panel3.Height);
                friendlist.Location = new Point(this.Location.X + this.Width - friendlist.Width, this.Location.Y + panel3.Height);
            }

            //panel10.AutoSize = true;
            //panel10.MaximumSize = new Size(panel9.Location.X - panel10.Location.X, 0);
        }


        public void Add_PlaylistLabel_Controls()
        {
            int Count = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.Playlist>("Playlists.json").Count;
            for (int i = 0; i < Count; i++)
            {
                CreateControls.iLabels[i].MouseClick += iLabels_MouseClick;
            }
        }
        public void Remove_PlaylistLabel_Controls()
        {
            int Count = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.Playlist>("Playlists.json").Count;
            for (int i = 0; i < Count; i++)
            {
                CreateControls.iLabels[i].MouseClick -= iLabels_MouseClick;
            }
        }


        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (mouseDown)
                {
                    if (this.WindowState == FormWindowState.Maximized)
                    {
                        iButton2.Image = Global.resizeImage(Resources_Images.Images.Icons.Maximize, new Size(20, 20));
                        WindowState = FormWindowState.Normal;
                        this.Size = new Size(1700, 900);
                    }

                    this.Location = new Point(
                        (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);


                    if (Global.CheckOpened("FriendList") == true)
                    {
                        FriendListTimer.Stop();
                        friendlist.Focus();

                        friendlist.Location = new Point(this.Location.X + this.Width - friendlist.Width, this.Location.Y + panel3.Height);
                    }
                    this.Update();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            if (Global.CheckOpened("FriendList") == true)
            {
                friendlist.Focus();
                FriendListTimer.Start();
            }
        }

        private void iButton3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        void MinimizeMaximize()
        {
            Rectangle rect = Screen.FromHandle(this.Handle).WorkingArea;
            rect.Location = new Point(0, 0);
            this.MaximizedBounds = rect;
            if (this.WindowState != FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
                iButton2.Image = Global.resizeImage(Resources_Images.Images.Icons.Minimize, new Size(20, 20));

            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = new Size(1700, 900);
                iButton2.Image = Global.resizeImage(Resources_Images.Images.Icons.Maximize, new Size(20, 20));
            }

            AdjustControls();
        }
        private void iButton2_Click(object sender, EventArgs e)
        {
            MinimizeMaximize();
        }
        void SaveAppSettings()
        {
            try
            {
                string FormSizeState = "";
                if (this.WindowState == FormWindowState.Maximized)
                    FormSizeState = "Maximized";
                else if (this.WindowState == FormWindowState.Normal)
                    FormSizeState = "Normal";
                SongPlayer.songThread.Abort();

                Methods.DataHandling.Handler.CreateJsonFile_FormSettings(Global.Current_PlayingSong, Global.Current_PlayingPlaylist, Global.Current_PlayingPlaylist.Index, iTrackBar2.Value, iTrackBar1.Value, Global.Shuffle, Global.LoopSongs, FormSizeState, this.Size);
            }
            catch { }
        }
        private async void iButton1_Click(object sender, EventArgs e)
        {
            //Methods.DataHandling.Handler.CreateMetaData($"Offline");
            //Multithreading.Background_UploadData_Thread = new Thread(Multithreading.Background_Upload_MetaData);
            //Multithreading.Background_UploadData_Thread.Start();

            this.Hide();
            SaveAppSettings();
            await SetStatus("Offline");
            Environment.Exit(-1);
            Application.Exit();
        }



        private async void iCircleButton1_Click(object sender, EventArgs e)
        {

            if (Global.PartyMode)
            {
                //PartyMode_SongPlayer.PartyModeSongsVars.SongURL
                // PartyMode_SongPlayer.PlaySong(PartyMode_SongPlayer.PartyModeSongsVars.SongURL, PartyMode_SongPlayer.PartyModeSongsVars.SongName, PartyMode_SongPlayer.PartyModeSongsVars.Artist, PartyMode_SongPlayer.PartyModeSongsVars.SongDuration, PartyMode_SongPlayer.PartyModeSongsVars.ImageURL);

                if (PartyMode_SongPlayer.mediaPlayer.playState != WMPPlayState.wmppsPlaying)
                {
                    PartyMode_SongPlayer.SetPlayOnServer();
                    await PartyMode_Room.Logs($"{Global.Authentication.Username}#{Global.Authentication.UsernameID} hat den Song pausiert");

                }
                else
                {
                    await PartyMode_Room.Logs($"{Global.Authentication.Username}#{Global.Authentication.UsernameID} hat den Song gestartet");
                    PartyMode_SongPlayer.SetPauseOnServer();
                }
                return;
            }

            if (Global.systemControls.PlaybackStatus != MediaPlaybackStatus.Playing)
                SongPlayer.PlaySong(Global.Current_PlayingSong.SongPath, Global.Current_PlayingSong.SongName, Global.Current_PlayingSong.SongAuthor, Global.SongPausedTime, Global.Current_PlayingSong.SongDuration, Global.Current_PlayingSong.ImagePath);
            else
                SongPlayer.PauseSong();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(mplayer.status); // Sound Quality
            //mplayer.settings.rate = 320; // Velocity
            //Console.WriteLine(mplayer.playState);
            //Console.WriteLine(Math.Round(mplayer.controls.currentPosition));
            //Console.WriteLine(mplayer.controls.currentItem.durationString);

            //Global.songDuration = Methods.Global.mplayer.controls.currentItem.durationString;
            //label2.Text = Methods.Global.mplayer.controls.currentItem.durationString;

        }


        private void iTrackBar2_Click(object sender, EventArgs e)
        {
            if (Global.PartyMode)
                PartyMode_SongPlayer.mediaPlayer.settings.volume = iTrackBar2.Value / 2;
            else
                Global.mediaElement.Volume = ((double)iTrackBar2.Value) / 100;
            label3.Text = iTrackBar2.Value.ToString();
        }

        private void iTrackBar1_Click(object sender, EventArgs e)
        {
            if (Global.PartyMode)
                PartyMode_SongPlayer.SetTrackValueOnServer(iTrackBar1.Value);
            else
                Global.mediaElement.Position = TimeSpan.FromSeconds(iTrackBar1.Value);
        }

        private void iButton5_MouseDown(object sender, MouseEventArgs e)
        {
            SongTrackBar_HoldTimer.Start();
        }

        private void iButton5_MouseUp(object sender, MouseEventArgs e)
        {
            SongTrackBar_HoldTimer.Stop();

        }


        private void SongTrackBar_HoldTimer_Tick(object sender, EventArgs e)
        {
            try
            {


                float calc = (iTrackBar1.PointToClient(Cursor.Position).X / (float)iTrackBar1.Width);
                float songDuration = SongPlayer.SongDurationToSeconds(Global.Current_PlayingSong.SongDuration);
                if (Global.PartyMode)
                    songDuration = PartyMode_SongPlayer.PartyModeSongsVars.SongDuration;
                float DeciNums = songDuration * calc;

                if (DeciNums <= 0)
                    iTrackBar1.Value = 0;
                if (DeciNums >= songDuration)
                    iTrackBar1.Value = (int)songDuration;

                if (DeciNums <= songDuration && DeciNums >= 0)
                    iTrackBar1.Value = (int)DeciNums;

                label1.Text = Methods.SongPlayer.currentTimeFormat(iTrackBar1.Value);
            }
            catch
            {
                Console.WriteLine("TrackBar Error");
            }
        }

        private void iTrackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            SongTrackBar_HoldTimer.Start();

        }

        private async void iTrackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            SongTrackBar_HoldTimer.Stop();
            if (Global.PartyMode)
            {
                await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(Global.PartyMode_SessionID).UpdateAsync(new Dictionary<string, object> { { "SongPosition", iTrackBar1.Value } });
                return;
            }
            Global.mediaElement.Position = TimeSpan.FromSeconds(iTrackBar1.Value);
        }

        private void VolumeTrackBar_HoldTimer_Tick(object sender, EventArgs e)
        {
            float calc = (iTrackBar2.PointToClient(Cursor.Position).X / (float)iTrackBar2.Width);
            float maxVolume = 100;
            float DeciNums = maxVolume * calc;

            if (DeciNums <= 0)
                iTrackBar2.Value = 0;
            if (DeciNums >= maxVolume)
                iTrackBar2.Value = (int)maxVolume;
            if (DeciNums <= maxVolume && DeciNums >= 0)
                iTrackBar2.Value = (int)DeciNums;

            if (Global.PartyMode)
                PartyMode_SongPlayer.mediaPlayer.settings.volume = iTrackBar2.Value / 2;
            else
                Global.mediaElement.Volume = ((double)iTrackBar2.Value) / 100;

            if (iTrackBar2.Value == 0)
                iButton7.Image = Resources_Images.Images.Icons.sound_0;
            else
                iButton7.Image = Resources_Images.Images.Icons.sound_1;

            iButton7.ImageSize = new Size(20, 20);
            label3.Text = iTrackBar2.Value.ToString();

        }
        private void ITrackBar2_MouseWheel(object sender, MouseEventArgs e)
        {
            int mousedeltaval = e.Delta / 120;
            if (mousedeltaval == 1) //mousewheel up move
            {
                iTrackBar2.Value++;
                //Global.mediaElement.Volume = ((double)iTrackBar2.Value) / 100;
            }
            if (mousedeltaval == -1) //mousewheel down move
            {
                iTrackBar2.Value--;
                //Global.mediaElement.Volume = ((double)iTrackBar2.Value) / 100;
            }

            if (Global.PartyMode)
                PartyMode_SongPlayer.mediaPlayer.settings.volume = iTrackBar2.Value / 2;
            else
                Global.mediaElement.Volume = ((double)iTrackBar2.Value) / 100;

            label3.Text = iTrackBar2.Value.ToString();
        }
        private void iTrackBar2_MouseDown(object sender, MouseEventArgs e)
        {
            VolumeTrackBar_HoldTimer.Start();
        }

        private void iTrackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            VolumeTrackBar_HoldTimer.Stop();
        }

        private void iButton7_Click(object sender, EventArgs e)
        {
            if (iTrackBar2.Value == 0)
            {
                iButton7.Image = Resources_Images.Images.Icons.sound_1;
                Global.mediaElement.Volume = Volume / 100;
                label3.Text = Volume.ToString();
                iTrackBar2.Value = Volume;
            }
            else
            {
                iButton7.Image = Resources_Images.Images.Icons.sound_0;
                Volume = (int)(Global.mediaElement.Volume * 100);
                Global.mediaElement.Volume = 0;
                label3.Text = "0";
                iTrackBar2.Value = 0;
            }
            iButton7.ImageSize = new Size(20, 20);
        }

        private void iLabel3_Click(object sender, EventArgs e)
        {
            Methods.Forms.CreatePlaylist createPlaylist = new Methods.Forms.CreatePlaylist();
            createPlaylist.Show();
        }

        int a = 0;
        private void iButton4_Click(object sender, EventArgs e)
        {
            //Console.WriteLine($"{panel6.VerticalScroll.Maximum} | {a}");
            //Console.WriteLine($"{panel6.ClientSize.Height} | {panel6.PreferredSize.Height}");

            a += 5;
            if (panel6.VerticalScroll.Maximum >= a)
            {
                //Console.WriteLine("eggeeg");
                panel6.VerticalScroll.Value = a;
            }
        }

        private void iButton6_Click(object sender, EventArgs e)
        {
            //iButton6.Size = new Size(iButton6.Width - 10, iButton6.Height - 10);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point pnt);
        public static string GetControl()
        {
            try
            {

                IntPtr hWnd = WindowFromPoint(Control.MousePosition);
                if (hWnd != IntPtr.Zero)
                {
                    Control ctl = Control.FromHandle(hWnd);
                    if (ctl != null && ctl.Name.Contains("iLabel_") && ctl.Visible == true)
                    {
                        return ctl.Name;
                    }
                }
            }
            catch { }
            return "-1";

        }

        int old_Selected_Label = -1;
        public void iLabels_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(GetControl().Split('_')[1]);
                if (e.Button == MouseButtons.Right)
                {
                    //ContextStripSettings();
                    iContextStripMenu.StartPosition = FormStartPosition.Manual;
                    iContextStripMenu.ShowInTaskbar = false;
                    iContextStripMenu.Location = new Point(MousePosition.X + 10, MousePosition.Y + 10);
                    old_Selected_Label = index;

                    if (iContextStripMenu.Visible == false)
                    {
                        ContextMenuStrip_FocusCheckTimer.Start();
                        iContextStripMenu.Show();
                    }
                    else
                    {
                        iContextStripMenu.Focus();
                    }
                }

                if (e.Button == MouseButtons.Left)
                {
                    if (iContextStripMenu.Visible == true)
                        return;

                    panel7.Controls.Clear();
                    if (index > 0)
                    {

                        #region Playlist_Label_Selection

                        if (old_Selected_Label >= 0)
                        {
                            CreateControls.iLabels[old_Selected_Label].HoverAnimation = true;
                            CreateControls.iLabels[old_Selected_Label].Select = false;
                        }
                        old_Selected_Label = index;
                        CreateControls.iLabels[index].Select = true;
                        #endregion

                        Global.Selected_Playlist = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.Playlist>(Global.playlists_path)[index];
                        mainPlayer = new Methods.Forms.MainPlayer();
                        mainPlayer.iLabel1.Text = CreateControls.iLabels[index].Text;

                        mainPlayer.TopLevel = false;
                        mainPlayer.Dock = DockStyle.None;
                        mainPlayer.Size = panel7.Size;
                        mainPlayer.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                        mainPlayer.Location = new Point((panel7.Width - mainPlayer.Width) / 2, (panel7.Height - mainPlayer.Height) / 2);
                        panel7.Controls.Add(mainPlayer);
                        mainPlayer.Show();

                        //if (Global.CurrentPlayingSongInPlaylist >= 0 && Global.Selected_Playlist.Index == Global.Current_PlayingPlaylist.Index)
                        //Methods.Forms.MainPlayer.Title_Labels[Global.CurrentPlayingSongInPlaylist].ForeColor = Color.FromArgb(255, 0, 200, 0);

                    }
                }


            }
            catch (Exception ex)
            {
                Global.iMessageBox.Show(ex.Message, "Fehler");
                Console.WriteLine("Error Form1: iLabels_MouseClick");
                //Global.iMessageBox.Show("Die Playlist existiert nicht mehr", "Fehler");
            }
        }
        private void iLabel4_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(Global.current_path + "/Playlists/FavouriteSongs"))
                    Directory.CreateDirectory(Global.current_path + "/Playlists/FavouriteSongs");

                panel7.Controls.Clear();
                Global.Selected_Playlist = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.Playlist>(Global.playlists_path)[0];

                mainPlayer = new Methods.Forms.MainPlayer();
                mainPlayer.TopLevel = false;
                mainPlayer.Dock = DockStyle.None;
                mainPlayer.Size = panel7.Size;
                mainPlayer.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                mainPlayer.Location = new Point((panel7.Width - mainPlayer.Width) / 2, (panel7.Height - mainPlayer.Height) / 2);
                panel7.Controls.Add(mainPlayer);
                mainPlayer.Show();

                if (Global.CurrentPlayingSongInPlaylist >= 0 && Global.Selected_Playlist.Index == Global.Current_PlayingPlaylist.Index)
                    Methods.Forms.MainPlayer.Title_Labels[Global.CurrentPlayingSongInPlaylist].ForeColor = Color.FromArgb(255, 0, 200, 0);
            }
            catch (Exception ex)
            {
                // Global.iMessageBox.Show("FavSong open Error");
                Global.iMessageBox.Show(ex.Message);
            }
        }
        void MarkPlayingSong()
        {
            try
            {
                int count = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.FormSettings>("AppSettings.json").Count;
                if (count > 0)
                {
                    var nullChecker = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.FormSettings>("AppSettings.json")[0].songs;
                    if (nullChecker != null)
                    {
                        Global.CurrentPlayingSongInPlaylist = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.FormSettings>("AppSettings.json")[0].songs.Index;
                        if (Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.FormSettings>("AppSettings.json")[0].playlist != null)
                            Global.Current_PlayingPlaylist = Methods.DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.FormSettings>("AppSettings.json")[0].playlist;
                    }
                }
            }
            catch { }
        }

        private void iScrollBarV1_MouseDown(object sender, MouseEventArgs e)
        {
            PlaylistScrollbar_HoldTimer.Start();
        }

        private void iScrollBarV1_MouseUp(object sender, MouseEventArgs e)
        {
            PlaylistScrollbar_HoldTimer.Stop();
        }



        private void iCircleButton1_MouseDown(object sender, MouseEventArgs e)
        {
            iCircleButton1.Location = new Point(iCircleButton1.Location.X + 2, iCircleButton1.Location.Y + 2);
            iCircleButton1.Size = new Size(iCircleButton1.Size.Width - 4, iCircleButton1.Size.Height - 4);

        }

        private void iCircleButton1_MouseUp(object sender, MouseEventArgs e)
        {
            iCircleButton1.Location = new Point(iCircleButton1.Location.X - 2, iCircleButton1.Location.Y - 2);
            iCircleButton1.Size = new Size(iCircleButton1.Size.Width + 4, iCircleButton1.Size.Height + 4);
        }

        private void iButton21_Click(object sender, EventArgs e)
        {
            // AdjustControls();
            iTrackBar1.Width++;

        }
        private void iLabel6_Click(object sender, EventArgs e)
        {
            Methods.Forms.Youtube_Downloader youtube_Downloader = new Methods.Forms.Youtube_Downloader();
            youtube_Downloader.Show();
        }

        private void iLabel1_Click(object sender, EventArgs e)
        {
            panel7.Controls.Clear();
        }

        private void iCircleButton4_Click(object sender, EventArgs e)
        {
            if (Global.LoopSongs == Structures.Loop.Off)
            {
                iCircleButton4.Image = Global.resizeImage(Resources_Images.Images.Icons.loop_green, new Size(20, 20));
                Global.LoopSongs = Structures.Loop.LoopPlaylist;
            }
            else if (Global.LoopSongs == Structures.Loop.LoopPlaylist)
            {
                iCircleButton4.Image = Global.resizeImage(Resources_Images.Images.Icons.Loop_1_Song, new Size(20, 20));
                Global.LoopSongs = Structures.Loop.LoopSong;
            }
            else if (Global.LoopSongs == Structures.Loop.LoopSong)
            {
                iCircleButton4.Image = Global.resizeImage(Resources_Images.Images.Icons.loop_64, new Size(20, 20));
                Global.LoopSongs = Structures.Loop.Off;
            }
        }

        private void iCircleButton5_Click(object sender, EventArgs e)
        {
            if (Global.Shuffle == false)
            {
                iCircleButton5.Image = Global.resizeImage(Resources_Images.Images.Icons.shuffle_green, new Size(20, 20));
                Global.Shuffle = true;
            }
            else if (Global.Shuffle == true)
            {
                iCircleButton5.Image = Global.resizeImage(Resources_Images.Images.Icons.shuffle, new Size(20, 20));
                Global.Shuffle = false;
            }
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            /*friendlist = new Methods.Forms.Friendlist.Main_FriendList();

            friendlist.ShowIcon = false;
            friendlist.BringToFront();
            //friendlist.TopMost = true;
            friendlist.Size = new Size(friendlist.Width, this.Height - panel3.Height);
            friendlist.isOpen = true;
            friendlist.ShowInTaskbar = false;
            this.Opacity = 0.99;
            friendlist.Show();
            friendlist.Location = new Point(this.Location.X + this.Width - friendlist.Width, this.Location.Y + panel3.Height);
            FriendListTimer.Start();*/

            if (friendsForm.Visible)
                friendsForm.Focus();

            string a = "102";
            Console.WriteLine(a.Length);
            friendsForm.Show();
        }

        private void iCircleButton2_Click(object sender, EventArgs e)
        {
            if (Global.PartyMode)
            {
                return;
            }
            SongPlayer.SkipSongForward();
            SongPlayer.UpdateCurrentPlaylist(Global.Current_PlayingPlaylist.Index);
        }

        private void iCircleButton3_Click(object sender, EventArgs e)
        {
            if (Global.PartyMode)
            {
                return;
            }

            if (iTrackBar1.Value <= 2)
                SongPlayer.SkipSongBackward();
            else
                iTrackBar1.Value = 0;

            SongPlayer.UpdateCurrentPlaylist(Global.Current_PlayingPlaylist.Index);
            Global.mediaElement.Position = TimeSpan.FromSeconds(iTrackBar1.Value);
        }

        private void panel4_MouseEnter(object sender, EventArgs e)
        {
            //Console.WriteLine("panel4_MouseEnter");
        }

        private void panel4_MouseLeave(object sender, EventArgs e)
        {
            //Console.WriteLine("panel4_MouseLeave");
        }

        private void FriendListTimer_Tick(object sender, EventArgs e)
        {

            if (friendlist.ContainsFocus == false && mouseDown == false)
            {
                if (friendlist.friendListSearcher.iContextStripMenu.Visible == true)
                    return;

                friendlist.Hide();

                friendlist.friendListSearcher.isOpen = false;
                Methods.Forms.Friendlist.Friend_Inbox.isOpen = false;
                friendlist.isOpen = false;

                this.Opacity = 1;
                FriendListTimer.Stop();
            }
        }

        private void FormFade_Tick(object sender, EventArgs e)
        {
            if (this.Opacity <= 0.99f)
            {
                this.Opacity += 0.1f;
            }
            else
            {
                FormFade.Stop();
            }
        }

        private void iButton22_Click(object sender, EventArgs e)
        {

        }

        private void panel3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MinimizeMaximize();
            }
        }

        private async void iButton23_Click(object sender, EventArgs e)
        {
            //var youtube = new YoutubeClient();
            //var video = await youtube.Videos.GetAsync("https://www.youtube.com/watch?v=Hn907P90aAA&list=PLW0vld845GwBB6ZRywQOdDH_DmF1l6o_Q&index=5");
            //Global.mplayer.URL = "https://www.youtube.com/watch?v=xXuG1F1MDS4";
            //Global.mplayer.controls.play();
            //pictureBox1.Image = Global.SetImageOpacity(pictureBox1.Image, 2f);

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //SetStatus("Offline");
            SaveAppSettings();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            AdjustControls();
        }
        void WireMouseEvents(Control container)
        {
            foreach (Control c in container.Controls)
            {
                c.Click += (s, e) => OnClick(e);
                c.MouseEnter += (s, e) => OnMouseEnter(e);
                c.MouseLeave += (s, e) => OnMouseLeave(e);

                c.MouseClick += (s, e) =>
                {
                    var p = PointToThis((Control)s, e.Location);
                    OnMouseClick(new MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta));
                    panel4_Click(panel4, e);
                };
            };
        }

        Point PointToThis(Control c, Point p)
        {
            return PointToClient(c.PointToScreen(p));
        }

        private void iLabel2_Click(object sender, EventArgs e)
        {
            Methods.Forms.Settings.MainSettings mainSettings = new Methods.Forms.Settings.MainSettings();
            mainSettings.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (Global.titleExport == Structures.Obs_SongTitleExport.Off)
            {
                if (Global.systemControls.PlaybackStatus == MediaPlaybackStatus.Playing)
                    SongPlayer.ExportSongTitle(Global.Current_PlayingSong.SongAuthor, Global.Current_PlayingSong.SongName);

                Global.titleExport = Structures.Obs_SongTitleExport.On;
                pictureBox2.Image = Resources_Images.Images.Icons.monitor_50px_green;
            }
            else
            {
                if (File.Exists($"{Global.current_path}/currentSong.txt"))
                    File.Delete($"{Global.current_path}/currentSong.txt");

                Global.titleExport = Structures.Obs_SongTitleExport.Off;
                pictureBox2.Image = Resources_Images.Images.Icons.monitor_50px;
            }
        }
        private void iButton21_Click_1(object sender, EventArgs e)
        {
            //Methods.Forms.Friendlist.FriendsForm friendsForm = new Methods.Forms.Friendlist.FriendsForm();
            //friendsForm.Show();
            AdjustControls();

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Methods.Partymode.PartyMode_Dashboard partyMode_Dashboard = new Methods.Partymode.PartyMode_Dashboard();
            partyMode_Dashboard.Show();
        }


        public static ObjectCache songChache = MemoryCache.Default;
        byte[] songData;
        private void iButton22_Click_1(object sender, EventArgs e)
        {
            //Console.WriteLine(iTrackBar1.MaxValue)
            string URL = "https://www.hlde1.online/Musicedy/Test/Samuel%20Kim%20Music.mp3";
            string URL2 = "https://firebasestorage.googleapis.com/v0/b/musicedy-29653.appspot.com/o/Clients%2F6a4d4799-f424-4081-9f57-3903fd763160%2FPlaylists%2Fkok%2FDemon%20Slayer%20Season%202%20Main%20Theme_Samuel%20Kim%20Music%2FSamuel%20Kim%20Music.mp3?alt=media&token=caad134f-b340-4c73-a261-d78ef6e6d13d";

            string Artist = "Secrets";
            string SongName = "Samuel Kim Music";

            WebClient webClient = new WebClient();
            songData = webClient.DownloadData(URL);
            var GetSongChache = songChache.AddOrGetExisting($"{Artist}_{SongName}", songData, null);
            Console.WriteLine(GetSongChache);
        }
        internal static IRandomAccessStream ConvertTo(byte[] arr)
        {
            return arr.AsBuffer().AsStream().AsRandomAccessStream();
        }
        private void iButton23_Click_1(object sender, EventArgs e)
        {
            //string URL = "https://firebasestorage.googleapis.com/v0/b/musicedy-29653.appspot.com/o/Clients%2F6a4d4799-f424-4081-9f57-3903fd763160%2FPlaylists%2Fkok%2FDemon%20Slayer%20Season%202%20Main%20Theme_Samuel%20Kim%20Music%2FSamuel%20Kim%20Music.mp3?alt=media&token=caad134f-b340-4c73-a261-d78ef6e6d13d";
            //string Artist = "Secrets";
            //string SongName = "Samuel Kim Music";

            //Console.WriteLine(SongPlayer.GetSongDuration("https://www.hlde1.online/Musicedy/Test/Samuel%20Kim%20Music.mp3"));
        }
    }
}
