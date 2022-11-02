using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using WMPLib;
using System.Text.Json.Nodes;
using System.Text.Json;
using Newtonsoft.Json;
using Windows.Media;
using Google.Cloud.Storage.V1;
using Google.Apis.Storage.v1.Data;
using Google.Apis.Auth.OAuth2;
using MusikPlayer.Methods.Authentication;
using MusikPlayer.Methods.Partymode;
using Google.Cloud.Firestore;

namespace MusikPlayer.Methods.Forms
{
    public partial class MainPlayer : Form
    {
        public static List<Panel> Panels = new List<Panel>();

        public static List<Design.iLabel> Title_Labels = new List<Design.iLabel>();
        public static List<Design.iLabel> Band_Labels = new List<Design.iLabel>();
        public static List<Design.iLabel> Album_Labels = new List<Design.iLabel>();
        public static List<Design.iLabel> AddedDate_Labels = new List<Design.iLabel>();
        public static List<Design.iLabel> SongDuration_Labels = new List<Design.iLabel>();


        public Design.iContextStripMenu iContextStripMenu;// = new Design.iContextStripMenu();

        public List<Image> ImageList = new List<Image>();

        public static Stream imageStream;
        public MainPlayer()
        {
            InitializeComponent();
            RefreshList();
            ContextStripSettings();
            MarkPlayingSong();

        }

        void MarkPlayingSong()
        {
            if (Title_Labels.Count <= Global.CurrentPlayingSongInPlaylist)
                return;

            if (Global.CurrentPlayingSongInPlaylist >= 0 && Global.Selected_Playlist.Index == Global.Current_PlayingPlaylist.Index)
                Title_Labels[Global.CurrentPlayingSongInPlaylist].ForeColor = Color.FromArgb(255, 0, 200, 0);
        }

        void ClearAllLists()
        {
            Panels.Clear();
            Title_Labels.Clear();
            Band_Labels.Clear();
            Album_Labels.Clear();
            AddedDate_Labels.Clear();
            SongDuration_Labels.Clear();
            ImageList.Clear();
        }

        void WireMouseEvents(Control container)
        {
            foreach (Control c in container.Controls)
            {
                c.Click += (s, e) => OnClick(e);
                c.DoubleClick += (s, e) => OnDoubleClick(e);
                c.MouseEnter += (s, e) => OnMouseEnter(e);
                c.MouseLeave += (s, e) => OnMouseLeave(e);

                c.MouseClick += (s, e) =>
                {
                    var p = PointToThis((Control)s, e.Location);
                    OnMouseClick(new MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta));
                    customPanels_MouseClick(s, e);
                };
                c.MouseDoubleClick += (s, e) =>
                {
                    var p = PointToThis((Control)s, e.Location);
                    OnMouseDoubleClick(new MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta));
                    customPanels_MouseDoubleClick(s, e);

                };
                c.MouseEnter += (s, e) =>
                {
                    customPanels_MouseEnter(s, e);
                };
                c.MouseLeave += (s, e) =>
                {
                    customPanels_MouseLeave(s, e);
                };
            };
        }

        Point PointToThis(Control c, Point p)
        {
            return PointToClient(c.PointToScreen(p));
        }

        void ContextStripSettings()
        {
            iContextStripMenu = new Design.iContextStripMenu();
            iContextStripMenu.AddOption("Abspielen", Play_Click);
            iContextStripMenu.AddOption("Pausieren", Pause_Click);
            iContextStripMenu.AddLine();
            iContextStripMenu.AddOption("Aus der Playlist entfernen", Delete_Click);
            iContextStripMenu.AddOption("Zur Playlist hinzufügen", AddToPlaylist_Click);
            iContextStripMenu.AddLine();
            iContextStripMenu.AddOption("Zur Partymode-Warteschlange hinzufügen", PartyModeQueue_Click);
            iContextStripMenu.AddOption("Informationen", Info_Click);
        }

        private void Play_Click(object sender, EventArgs e)
        {
            try
            {
                int index = old_selectedPanel;

                Form1 form1 = Application.OpenForms.OfType<Form1>().Single();
                form1.iTrackBar1.Value = 0;
                form1.label1.Text = "00:00";
                Global.SongPausedTime = 0;
                string FileName = Global.Selected_Playlist.JsonPath;

                Global.Current_PlayingSong = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName)[index];
                Global.Current_PlayingPlaylist.PlaylistName = Global.Selected_Playlist.PlaylistName;
                Global.CurrentPlayingSongInPlaylist = index;

                SongPlayer.UpdateCurrentPlaylist(Global.Selected_Playlist.Index);

                string SongPath = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName)[index].SongPath;
                string SongName = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName)[index].SongName;
                string Artist = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName)[index].SongAuthor;
                string ImageURL = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName)[index].ImagePath;

                string SongDuration = SongPlayer.GetSongDuration(SongPath);
                MarkPlay();

                Title_Labels[index].ForeColor = Color.FromArgb(255, 0, 200, 0);

                SongPlayer.PlaySong(SongPath, SongName, Artist, Global.SongPausedTime, SongDuration, ImageURL);
                //await Partymode.PartyMode_SongPlayer.PartyModeAuthorization(Artist, SongName);
            }
            catch { }
        }
        private void Pause_Click(object sender, EventArgs e)
        {
            try
            {
                //Console.WriteLine("Pausieren_Click");
                int index = old_selectedPanel;
                if (Global.systemControls.PlaybackStatus != MediaPlaybackStatus.Playing)
                {
                    SongPlayer.PlaySong(Global.Current_PlayingSong.SongPath, Global.Current_PlayingSong.SongName, Global.Current_PlayingSong.SongAuthor, Global.SongPausedTime, Global.Current_PlayingSong.SongDuration, Global.Current_PlayingSong.ImagePath);
                    this.Text = $"{Global.Current_PlayingSong.SongAuthor} - {Global.Current_PlayingSong.SongName}";
                }
                else
                {
                    SongPlayer.PauseSong();
                    this.Text = "Musicedy Beta Version";
                }

            }
            catch { }
        }

        private void AddToPlaylist_Click(object sender, EventArgs e)
        {
            try
            {
                Global.iMessageBox.Show("AddToPlaylist Kopf");
            }
            catch { }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                Form1 form1 = Application.OpenForms.OfType<Form1>().Single();
                string FileName = Global.Selected_Playlist.JsonPath;
                int Count = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName).Count;
                List<DataHandling.Songs> Playlist = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName);
                var o = JsonObject.Parse(File.ReadAllText(FileName));

                form1.iPictureBox5.Image.Dispose();
                for (int i = 0; i < panel2.Controls.Count; i++)
                    ImageList[i].Dispose();


                Directory.Delete($"{Global.current_path}/Playlists/{Global.Selected_Playlist.PlaylistName}/{Playlist[old_selectedPanel].SongAuthor}_{Playlist[old_selectedPanel].SongName}", true);
                o.AsArray().RemoveAt(old_selectedPanel);
                if (old_selectedPanel == Count)
                    return;

                List<DataHandling.Songs> newJson = JsonConvert.DeserializeObject<List<DataHandling.Songs>>(o.ToString());
                for (int i = old_selectedPanel; i < newJson.Count; i++)
                {
                    newJson[i].ID--;
                    newJson[i].Index--;
                }
                string jsonData = JsonConvert.SerializeObject(newJson, Formatting.Indented);
                File.WriteAllText(FileName, jsonData);
                ContextMenuStrip_FocusCheckTimer.Stop();
                RefreshList();
                iContextStripMenu.Hide();
                MarkPlayingSong();
                Firebase_Methods.Storage_Methods.DeleteSong(Global.Selected_Playlist.PlaylistName, Playlist[old_selectedPanel].SongName, Playlist[old_selectedPanel].SongAuthor);

                if (Global.isOnCurrentSongAndPlaylist(old_selectedPanel) == false)
                    return;

                SongPlayer.PauseSong();
                SongPlayer.ResetPlayer();
            }
            catch (Exception ex)
            {
                Global.iMessageBox.Show(ex.Message);
            }
        }

        private void Info_Click(object sender, EventArgs e)
        {
            Global.iMessageBox.Show("Blubkopf");
        }
        private async void PartyModeQueue_Click(object sender, EventArgs e)
        {
            if (!Global.PartyMode)
            {
                Global.iMessageBox.Show("Du bist in keiner Party", "Fehler");
                return;
            }
            if (!PartyMode_SongPlayer.PartyModeAuthorization())
                return;

            int PM_index = old_selectedPanel; //Convert.ToInt32(GetControl().Split('_')[1]) - 1;
            string PM_FileName = Global.Selected_Playlist.JsonPath;
            string PM_SongName = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(PM_FileName)[PM_index].SongName;
            string PM_Artist = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(PM_FileName)[PM_index].SongAuthor;
            string PM_Album = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(PM_FileName)[PM_index].Album;
            int PM_SongDuration = SongPlayer.SongDurationToSeconds(DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(PM_FileName)[PM_index].SongDuration);
            string SongLink = await PartyMode_Room.GetSongLink(PM_Artist, PM_SongName);
            await PartyMode_Queue.GetQueueInfo();
            Dictionary<string, Dictionary<string, object>> AddQueueDict = new Dictionary<string, Dictionary<string, object>>
            {
                {
                    PartyMode_Queue.QueueElements.ToString(),
                    new Dictionary<string, object>
                    {
                        {"Album",PM_Album},
                        {"Artist", PM_Artist},
                        {"SongDuration", PM_SongDuration},
                        {"SongFromUser", $"{Global.Authentication.Username}#{Global.Authentication.UsernameID}"},
                        {"SongImage",""},
                        {"SongLink", SongLink},
                        {"SongName",PM_SongName},
                    }
                },
            };
            await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(Global.PartyMode_SessionID).Collection("queue").Document("queue").SetAsync(AddQueueDict, SetOptions.MergeAll);
            iContextStripMenu.Hide();

        }


        private void iButton1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "MP3|*.mp3|WAV|*.wav|All Files|*.*";
                DialogResult dr = openFileDialog.ShowDialog();
                string sourceFileName = openFileDialog.FileName;
                string destFileName = Global.current_path + $"/FavouriteSongs/{Path.GetFileName(openFileDialog.FileName)}";

                WindowsMediaPlayer mplayer = new WindowsMediaPlayer();
                IWMPMedia mediaInformation = mplayer.newMedia(Path.GetFileName(openFileDialog.FileName));

                if (dr == DialogResult.OK)
                {
                    if (File.Exists(destFileName))
                    {
                        DialogResult dialogResult = MessageBox.Show("Der Song existiert bereits, möchten Sie den Song ersetzen?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (dialogResult == DialogResult.Yes)
                        {
                            File.Delete(destFileName);
                            File.Copy(sourceFileName, destFileName);
                            Global.iMessageBox.Show("Der Song wurde hinzugefügt", "Information");
                        }
                    }
                    else
                    {
                        string SongArtist = SongPlayer.GetArtistFromFile(Path.GetFileName(openFileDialog.FileName));
                        string SongName = SongPlayer.GetSongNameFromFile(Path.GetFileName(openFileDialog.FileName));
                        string Added_Date = DateTime.Now.ToString("dd. MMM yyyy");
                        int Count = DataHandling.Methods.GetJsonCount(Global.current_path + "/FavouriteSongs.json");
                        DataHandling.Handler.CreateJsonFile(Count, Count, mediaInformation.durationString, Added_Date, SongArtist, SongName, destFileName, Global.current_path + "/FavouriteSongs.json");
                        File.Copy(sourceFileName, destFileName);
                        Global.iMessageBox.Show("Der Song wurde hinzugefügt", "Information");
                    }
                }
                RefreshList();
            }
            catch { }
        }

        private void panel3_MouseEnter(object sender, EventArgs e)
        {

        }

        private void panel3_MouseLeave(object sender, EventArgs e)
        {

        }


        private List<string> GetFileName(string path)
        {
            List<string> files = new List<string>();
            int fileCount = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length;
            string fileNames = "";
            for (int i = 0; i < fileCount; i++)
            {
                fileNames = Directory.GetFiles(path, "*")[i];
                if (fileNames.Contains(".mp3") || fileNames.Contains(".wav"))
                {
                    files.Add(fileNames);
                }
            }
            return files;

        }



        public void RefreshList()
        {
            try
            {
                panel2.Controls.Clear();
                ClearAllLists();
                string FileName = Global.Selected_Playlist.JsonPath; //$"{Global.current_path}/Playlists/{Global.Selected_Playlist.PlaylistName}/{Global.Selected_Playlist.PlaylistName}.json";
                int JsonCount = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName).Count;
                iLabel2.Text = JsonCount == 1 ? $"Song: {JsonCount}" : $"Songs: {JsonCount}";
                string SongTitle;
                string Band;
                string Album;
                string AddedDate;
                string SongDuration;
                string ImageURL;
                for (int i = 0; i < JsonCount; i++)
                {

                    SongTitle = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName)[i].SongName;
                    Band = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName)[i].SongAuthor;
                    Album = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName)[i].Album;
                    AddedDate = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName)[i].AddedDate;
                    SongDuration = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName)[i].SongDuration;
                    ImageURL = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName)[i].ImagePath;

                    Title_Labels.Add(CreateControls.Labels(SongTitle, new Point(Title_Label.Location.X + 60, 17), Title_Label.Anchor, Cursors.Hand, $"TitleLabel_{i + 1}"));
                    Band_Labels.Add(CreateControls.Labels(Band, new Point(Band_Label.Location.X - 8, 17), Title_Label.Anchor, Cursors.Hand, $"BandLabel_{i + 1}"));
                    Album_Labels.Add(CreateControls.Labels(Album, new Point(Album_Label.Location.X - 8, 17), Title_Label.Anchor, Cursors.Hand, $"AlbumLabel_{i + 1}"));
                    AddedDate_Labels.Add(CreateControls.Labels(AddedDate, new Point(AddedDate_Label.Location.X - 8, 17), Title_Label.Anchor, Cursors.Default, $"AddedDateLabel_{i + 1}"));
                    SongDuration_Labels.Add(CreateControls.Labels(SongDuration, new Point(SongDuration_Label.Location.X - 8, 17), Title_Label.Anchor, Cursors.Default, $"SongDurationLabel_{i + 1}"));

                    using (imageStream = File.OpenRead(ImageURL))
                    {
                        ImageList.Add(Image.FromStream(imageStream));
                    }

                    Panels.Add(CreateControls.Create_SongPlayerPanel($"{i + 1}", Title_Labels[i], Band_Labels[i], Album_Labels[i], AddedDate_Labels[i], SongDuration_Labels[i], new Point(5, 0 + (i * 60)), new Size(panel2.Width - 10, 54), ImageList[i]));

                    /*if (Title_Labels[i].Width >= Band_Labels[i].Location.X)
                    {
                        Console.WriteLine(Title_Labels[i].Text);
                    }*/
                    //Console.WriteLine($"{Band_Label.Location.X} | {Album_Label.Location.X} |  {AddedDate_Labels[i].Location.X}");

                    //if (Album_Labels[i].Width + Album_Labels[i].Location.X >= Album_Label.Location.X)
                    //Console.WriteLine(Album_Labels[i].Text);

                    Title_Labels[i].MaximumSize = new Size(Band_Labels[i].Location.X - Title_Labels[i].Location.X, 0);
                    Band_Labels[i].MaximumSize = new Size(Album_Labels[i].Location.X - Band_Labels[i].Location.X, 0);
                    //Album_Labels[i].MaximumSize = new Size(AddedDate_Label.Location.X - Album_Labels[i].Location.X, 0);


                    panel2.Controls.Add(Panels[i]);
                    Panels[i].MouseEnter += customPanels_MouseEnter;
                    Panels[i].MouseLeave += customPanels_MouseLeave;
                    Panels[i].MouseDoubleClick += customPanels_MouseDoubleClick;
                    Panels[i].MouseClick += customPanels_MouseClick;
                    WireMouseEvents(Panels[i]);
                }
            }
            catch (Exception ex)
            {
                Global.iMessageBox.Show(ex.Message);
                Console.WriteLine("Error MainPlayer: RefreshList");
            }
        }



        void MarkPlay()
        {
            for (int i = 0; i < Title_Labels.Count; i++)
            {
                Title_Labels[i].AlphaColor = 150;
                Title_Labels[i].ForeColor = Color.White;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (iContextStripMenu.ContainsFocus == false)
            {
                Panels[old_selectedPanel].BackColor = Color.FromArgb(20, 20, 20);
                iContextStripMenu.Hide();
                ContextMenuStrip_FocusCheckTimer.Stop();
            }
        }
        private void customPanels_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(GetControl().Split('_')[1]) - 1;

                if (e.Button == MouseButtons.Right)
                {


                    iContextStripMenu.StartPosition = FormStartPosition.Manual;
                    iContextStripMenu.ShowInTaskbar = false;
                    iContextStripMenu.Location = new Point(MousePosition.X + 10, MousePosition.Y + 10);
                    //iContextStripMenu.TopMost = true;
                    if (iContextStripMenu.Visible == false)
                    {
                        ContextMenuStrip_FocusCheckTimer.Start();
                        iContextStripMenu.Show();
                        Panels[index].BackColor = Color.FromArgb(60, 60, 60);
                        old_selectedPanel = index;
                    }
                    else
                    {
                        iContextStripMenu.Focus();
                    }
                }

                if (e.Button == MouseButtons.Left)
                {

                    if (index >= 0)
                    {
                        old_selectedPanel = index;
                        Panels[index].BackColor = Color.FromArgb(60, 60, 60);
                    }
                }
            }
            catch { }

        }

        private async void customPanels_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    Form1 form1 = Application.OpenForms.OfType<Form1>().Single();

                    if (Global.PartyMode)
                    {
                        if (PartyMode_SongPlayer.PartyModeAuthorization())
                        {
                            int PM_index = Convert.ToInt32(GetControl().Split('_')[1]) - 1;
                            string PM_FileName = Global.Selected_Playlist.JsonPath;
                            string PM_SongName = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(PM_FileName)[PM_index].SongName;
                            string PM_Artist = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(PM_FileName)[PM_index].SongAuthor;
                            int PM_SongDuration = SongPlayer.SongDurationToSeconds(DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(PM_FileName)[PM_index].SongDuration);
                            await PartyMode_SongPlayer.PartyMode_SetSong(PM_Artist, PM_SongName, PM_SongDuration, true);
                            await PartyMode_Room.Logs($"{Global.Authentication.Username}#{Global.Authentication.UsernameID} hat den Song {PM_SongName} von {PM_Artist} gestartet");
                        }
                        return;
                    }


                    form1.iTrackBar1.Value = 0;
                    form1.label1.Text = "00:00";
                    Global.SongPausedTime = 0;
                    int index = Convert.ToInt32(GetControl().Split('_')[1]) - 1;
                    string FileName = Global.Selected_Playlist.JsonPath;
                    Global.Current_PlayingSong = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName)[index];
                    Global.Current_PlayingPlaylist = Global.Selected_Playlist;
                    Global.CurrentPlayingSongInPlaylist = index;

                    SongPlayer.UpdateCurrentPlaylist(Global.Selected_Playlist.Index);

                    string SongPath = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName)[index].SongPath;
                    string SongName = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName)[index].SongName;
                    string Artist = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName)[index].SongAuthor;
                    string ImageURL = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName)[index].ImagePath;

                    string SongDuration = SongPlayer.GetSongDuration(SongPath);
                    MarkPlay();

                    Title_Labels[index].ForeColor = Color.FromArgb(255, 0, 200, 0);
                    SongPlayer.PlaySong(SongPath, SongName, Artist, Global.SongPausedTime, SongDuration, ImageURL);


                }

            }
            catch (Exception ex)
            {
                Global.iMessageBox.Show(ex.Message, "MainPlayer");
            }
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
                    if (ctl != null /*&& ctl.Name.Contains("Panel_")*/ && ctl.Visible == true)
                    {
                        return ctl.Name;
                    }
                }
            }
            catch { }
            return "-1";

        }


        int old_selectedPanel = -1;
        private void customPanels_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                if (ContextMenuStrip_FocusCheckTimer.Enabled == true)
                    return;

                int index = Convert.ToInt32(GetControl().Split('_')[1]) - 1;
                if (index >= 0)
                {
                    old_selectedPanel = index;
                    Panels[index].BackColor = Color.FromArgb(40, 40, 40);
                }

            }
            catch { }
        }
        private void customPanels_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                if (ContextMenuStrip_FocusCheckTimer.Enabled == true)
                    return;

                if (old_selectedPanel >= 0)
                {
                    Panels[old_selectedPanel].BackColor = Color.FromArgb(20, 20, 20);
                }
            }
            catch
            {
                Panels[old_selectedPanel].BackColor = Color.FromArgb(20, 20, 20);
            }
        }

        private void iButton2_Click(object sender, EventArgs e)
        {
            panel2.Size = new Size(9999, 999);
        }


        private void panel1_SizeChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < Title_Labels.Count; i++)
            {
                Title_Labels[i].Location = new Point(Title_Label.Location.X + 60, 17);
                Band_Labels[i].Location = new Point(Band_Label.Location.X - 8, 17);
                Album_Labels[i].Location = new Point(Album_Label.Location.X - 8, 17);
                AddedDate_Labels[i].Location = new Point(AddedDate_Label.Location.X - 8, 17);
                SongDuration_Labels[i].Location = new Point(SongDuration_Label.Location.X - 8, 17);
            }
        }

        private async void iButton3_Click(object sender, EventArgs e)
        {
            RefreshList();

        }
        void UploadToFirestoreStorage()
        {
            string FileName = Global.Selected_Playlist.JsonPath; //$"{Global.current_path}/Playlists/{Global.Selected_Playlist.PlaylistName}/{Global.Selected_Playlist.PlaylistName}.json";
            int JsonCount = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName).Count;

            string SongPath;
            string SongTitle;
            string Artist;
            string ImageURL;
            string SongArtistName;
            iLabel3.Visible = true;
            for (int i = 0; i < JsonCount; i++)
            {
                SongPath = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName)[i].SongPath;
                SongTitle = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName)[i].SongName;
                Artist = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName)[i].SongAuthor;
                ImageURL = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(FileName)[i].ImagePath;
                SongArtistName = $"{Artist}_{SongTitle}";

                if (string.IsNullOrEmpty(ImageURL))
                    Firebase_Methods.Storage_Methods.UploadSong(SongPath, ImageURL, Global.Selected_Playlist.PlaylistName, SongTitle, SongArtistName, false);
                else
                    Firebase_Methods.Storage_Methods.UploadSong(SongPath, ImageURL, Global.Selected_Playlist.PlaylistName, SongTitle, SongArtistName, true);


                iLabel3.Text = $"{i} von {JsonCount}";
            }
            iLabel3.Visible = false;
            //Global.iMessageBox.Show("Fertig");
        }

        private void iCircleButton3_Click(object sender, EventArgs e)
        {

        }

        private void iButton21_Click(object sender, EventArgs e)
        {
            // UploadToFirestoreStorage();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.Location = new Point(pictureBox1.Location.X + 2, pictureBox1.Location.Y + 2);
            pictureBox1.Size = new Size(pictureBox1.Size.Width - 4, pictureBox1.Size.Height - 4);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox1.Location = new Point(pictureBox1.Location.X - 2, pictureBox1.Location.Y - 2);
            pictureBox1.Size = new Size(pictureBox1.Size.Width + 4, pictureBox1.Size.Height + 4);
        }
        public IEnumerable<Bucket> ListBuckets(string projectId = "your-project-id")
        {
            GoogleCredential credential = GoogleCredential.FromJson(Encoding.UTF8.GetString(Firebase_AuthFile.cloudfire));
            var storage = StorageClient.Create(credential);
            var buckets = storage.ListBuckets(projectId);
            Console.WriteLine("Buckets:");
            foreach (var bucket in buckets)
            {
                Console.WriteLine(bucket.Name);
            }
            return buckets;
        }

        private async void pictureBox1_Click(object sender, EventArgs e)
        {
            /*string myBucket = "musicedy-29653.appspot.com";
            string projectId = "musicedy-29653";

            string c = "musicedy-29653.appspot.com/Clients/91d37dee-fd56-4c55-bbe1-5d8b92c437d7/Playlists/FavouriteSongs/August Burns Red_Creative Captivity/";
            GoogleCredential credential = GoogleCredential.FromJson(Encoding.UTF8.GetString(Firebase_AuthFile.cloudfire));
            var storage = StorageClient.Create(credential);

            Bucket sf = await storage.GetBucketAsync(myBucket);
            var buckets = storage.ListBuckets(projectId);
            */

            UploadToFirestoreStorage();
        }
    }
}
