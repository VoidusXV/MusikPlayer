using Google.Cloud.Firestore;
using MusikPlayer.Methods.Authentication;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods.Partymode
{
    public partial class PartyMode_Queue : Form
    {
        private bool mouseDown;
        private Point lastLocation;

        private static Dictionary<string, object> QueueDict;

        public static int QueueElements = 0;

        private static Dictionary<string, object> AlreadyListenedDict;
        private static List<UserControl> SongPanelsList = new List<UserControl>();
        private FirestoreChangeListener firestoreQueue_Listener;
        bool QueueSelected = true;

        int panel2_PrefSizeHeight = 0;
        int oldSelectedPanel = -1;
        int iContextSelectedPanel = -1;

        Design.iContextStripMenu iContextStripMenu;// = new Design.iContextStripMenu();

        public PartyMode_Queue()
        {
            InitializeComponent();
            iScrollBarV1.MovingState = false;
            AdjustControls();
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

        }
        private void PartyMode_Queue_Load(object sender, EventArgs e)
        {
            ContextStripSettings();
            Main();
        }
        void ContextStripSettings()
        {
            iContextStripMenu = new Design.iContextStripMenu();
            iContextStripMenu.AddOption("Zur Warteschlange hinzufügen", AddQueue_Click);
            iContextStripMenu.AddOption("Aus Warteschlange entfernen", RemoveQueue_Click);
        }
        private async void AddQueue_Click(object sender, EventArgs e)
        {
            if (!Global.PartyMode)
            {
                Global.iMessageBox.Show("Du bist in keiner Party", "Fehler");
                return;
            }
            if (!PartyMode_SongPlayer.PartyModeAuthorization())
                return;

            /*int PM_index = old_selectedPanel;
            string PM_FileName = Global.Selected_Playlist.JsonPath;
            string PM_SongName = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(PM_FileName)[PM_index].SongName;
            string PM_Artist = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(PM_FileName)[PM_index].SongAuthor;
            string PM_Album = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(PM_FileName)[PM_index].Album;
            int PM_SongDuration = SongPlayer.SongDurationToSeconds(DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(PM_FileName)[PM_index].SongDuration);
            string SongLink = await PartyMode_Room.GetSongLink(PM_Artist, PM_SongName);
            await GetQueueInfo();
            Dictionary<string, Dictionary<string, object>> AddQueueDict = new Dictionary<string, Dictionary<string, object>>
            {
                {
                    QueueElements.ToString(),
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
            iContextStripMenu.Hide();*/

        }

        private async void RemoveQueue_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(QueueDict.Keys.ElementAt(iContextSelectedPanel));

            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { index.ToString(), FieldValue.Delete }
            };
            await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(Global.PartyMode_SessionID).Collection("queue").Document("queue").UpdateAsync(updates);
            iContextStripMenu.Hide();
        }

        void AdjustControls()
        {

            SongPanel_0.Location = new Point(0, SongPanel_0.Location.Y);
            SongPanel_0.Width = panel2.Width;
            SongPanel_0.NumLabel.ForeColor = Color.CornflowerBlue;

            SongPanel_0.SongLabel.ForeColor = Color.CornflowerBlue;
            SongPanel_0.SongLabel.Location = new Point(SongPanel_0.pictureBox1.Location.X + SongPanel_0.pictureBox1.Width + 5, SongPanel_0.pictureBox1.Location.Y + 2);

            SongPanel_0.AlbumLabel.Location = new Point(Album_Label.Location.X, SongPanel_0.pictureBox1.Location.Y + (SongPanel_0.pictureBox1.Height - SongPanel_0.AlbumLabel.Height) / 2);
            SongPanel_0.UsernameLabel.Location = new Point(AddedBy_Label.Location.X, (SongPanel_0.Height - SongPanel_0.UsernameLabel.Height) / 2);
            SongPanel_0.DurationLabel.Location = new Point(SongDuration_Label.Location.X, (SongPanel_0.Height - SongPanel_0.DurationLabel.Height) / 2);
            SongPanel_0.pictureBox1.Location = new Point(SongPanel_0.pictureBox1.Location.X, (SongPanel_0.Height - SongPanel_0.pictureBox1.Height) / 2);
            SongPanel_0.pictureBox1.Image = Resources_Images.Images.Icons.music_logo;


        }
        void WireMouseEvents(Control container, EventHandler MouseEnter, EventHandler MouseLeave, MouseEventHandler MouseClick)
        {
            foreach (Control c in container.Controls)
            {
                c.Click += (s, e) => OnClick(e);
                //c.DoubleClick += (s, e) => OnDoubleClick(e);
                c.MouseEnter += (s, e) => OnMouseEnter(e);
                c.MouseLeave += (s, e) => OnMouseLeave(e);

                /*c.MouseDoubleClick += (s, e) =>
                {
                    var p = PointToThis((Control)s, e.Location);
                    OnMouseDoubleClick(new MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta));
                    //SongPanel_MouseDoubleClick(s, e);
                };*/

                c.MouseClick += MouseClick;
                c.MouseEnter += MouseEnter;
                c.MouseLeave += MouseLeave;
            };
        }

        async void Main()
        {
            await GetQueueInfo();
            WireMouseEvents(SongPanel_0, SongPanel_0_MouseEnter, SongPanel_0_MouseLeave, SongPanel_0_MouseClick);
            ListenQueue();
        }

        public void ListenQueue()
        {
            try
            {
                Query query = FirestoreGlobal.firestoreDb.Collection("Sessions").Document(Global.PartyMode_SessionID).Collection("queue");

                firestoreQueue_Listener = query.Listen(async snapshot =>
                {
                    foreach (DocumentChange change in snapshot.Changes)
                    {
                        if (change.ChangeType == DocumentChange.Type.Added)
                        {
                            //Console.WriteLine("New: {0}", change.Document.Id);
                            this.Invoke((MethodInvoker)delegate
                            {
                                if (change.Document.Id == "queue")
                                    CreateQueuePanels();
                            });
                        }
                        else if (change.ChangeType == DocumentChange.Type.Modified)
                        {
                            //Console.WriteLine("Modified: {0}", change.Document.Id);
                            //DocumentReference docRef = FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID).Collection("queue").Document(change.Document.Id);
                            this.Invoke((MethodInvoker)async delegate
                            {
                                await GetQueueInfo();
                                CreateQueuePanels();
                            });
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        void ResetScroller(Panel panel)
        {
            iScrollBarV1.MovingState = false;
            iScrollBarV1.Value = 0;
            panel.VerticalScroll.Value = 0;
            panel.AutoScrollPosition = new Point(0, 0);
        }

        void SetScroller(Panel panel)
        {
            this.Invoke((MethodInvoker)delegate
            {
                iScrollBarV1.MovingState = false;
                panel.VerticalScroll.Maximum = panel.PreferredSize.Height;
                panel2_PrefSizeHeight = panel.PreferredSize.Height;
                Console.WriteLine(panel2_PrefSizeHeight);
                if (panel2_PrefSizeHeight > panel.Height)
                {
                    float a = ((float)panel.Height / (float)panel.PreferredSize.Height);
                    int LargeChangeVal = Convert.ToInt32((float)iScrollBarV1.Height * a);
                    if (LargeChangeVal < 70)
                        LargeChangeVal = 70;

                    iScrollBarV1.MovingState = true;
                    iScrollBarV1.LargeChange = LargeChangeVal;
                }
            });
        }
        private void iScrollBarV1_Scroll()
        {
            try
            {
                //panel2.SuspendLayout();

                var a = panel2_PrefSizeHeight - panel2.Size.Height;
                var scroll_Value = (iScrollBarV1.Value * a) / (iScrollBarV1.Size.Height - iScrollBarV1.LargeChange);
                panel2.VerticalScroll.Value = scroll_Value;
                panel2.AutoScrollPosition = new Point(0, scroll_Value);
                //panel2.PerformLayout();
                //panel2.Refresh();
                panel2.Update();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        string GetNestedDictionary(Dictionary<string, object> dict, int headKey, string Key)
        {
            var a = (Dictionary<string, object>)dict[headKey.ToString()];
            return a[Key].ToString();
        }


        public async static Task GetQueueInfo()
        {
            DocumentSnapshot QueueSnapshot = await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(Global.PartyMode_SessionID).Collection("queue").Document("queue").GetSnapshotAsync();

            QueueDict = QueueSnapshot.ToDictionary();
            QueueDict = QueueDict.OrderBy(x => Convert.ToInt32(x.Key)).ToDictionary(x => x.Key, y => y.Value);
            QueueElements = Convert.ToInt32(QueueDict.Keys.ElementAt(QueueDict.Count - 1)) + 1;

            DocumentSnapshot AlreadyListenedSnapshot = await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(Global.PartyMode_SessionID).Collection("queue").Document("alreadyListened").GetSnapshotAsync();
            AlreadyListenedDict = AlreadyListenedSnapshot.ToDictionary();
        }

        QueueForms.SongPanel CreateSongPanel(int index, string SongName, string Artist, string SongDuration, string Album, string SongFromUser, int Num, string ImageUri)
        {
            QueueForms.SongPanel songPanel = new QueueForms.SongPanel();

            songPanel.pictureBox1.Name = $"pictureBox1_{index}";//
            songPanel.pictureBox1.Size = SongPanel_0.pictureBox1.Size;
            songPanel.pictureBox1.Location = new Point(SongPanel_0.pictureBox1.Location.X, (songPanel.Height - songPanel.pictureBox1.Height) / 2);
            if (string.IsNullOrEmpty(ImageUri))
                songPanel.pictureBox1.Image = Resources_Images.Images.Icons.music_logo;
            else
                songPanel.pictureBox1.ImageLocation = ImageUri;

            songPanel.SongLabel.Name = $"SongLabel_{index}";
            songPanel.SongLabel.Text = SongName;
            songPanel.SongLabel.Location = new Point(songPanel.pictureBox1.Location.X + songPanel.pictureBox1.Width + 5, songPanel.pictureBox1.Location.Y + 2);

            songPanel.ArtistLabel.Name = $"ArtistLabel_{index}";
            songPanel.ArtistLabel.Text = Artist;
            songPanel.ArtistLabel.Location = new Point(songPanel.pictureBox1.Location.X + songPanel.pictureBox1.Width + 5, (songPanel.pictureBox1.Location.Y + songPanel.pictureBox1.Height) - songPanel.ArtistLabel.Height - 2);

            songPanel.AlbumLabel.Name = $"AlbumLabel_{index}";
            songPanel.AlbumLabel.Text = Album;
            songPanel.AlbumLabel.Location = new Point(Album_Label.Location.X, songPanel.pictureBox1.Location.Y + (songPanel.pictureBox1.Height - songPanel.AlbumLabel.Height) / 2);

            songPanel.DurationLabel.Name = $"DurationLabel_{index}";
            songPanel.DurationLabel.Text = SongDuration;
            songPanel.DurationLabel.Location = new Point(SongDuration_Label.Location.X, songPanel.pictureBox1.Location.Y + (songPanel.pictureBox1.Height - songPanel.DurationLabel.Height) / 2);

            songPanel.UsernameLabel.Name = $"UsernameLabel_{index}";
            songPanel.UsernameLabel.Text = SongFromUser;
            songPanel.UsernameLabel.Location = new Point(AddedBy_Label.Location.X, (songPanel.pictureBox1.Height - songPanel.UsernameLabel.Height) / 2);

            songPanel.NumLabel.Name = $"NumLabel_{index}";
            songPanel.NumLabel.Text = Num.ToString();
            songPanel.NumLabel.Location = new Point(songPanel.pictureBox1.Location.X - songPanel.NumLabel.Size.Width - 6, (songPanel.Height - songPanel.NumLabel.Height) / 2);






            //SongPanelsList.Add(songPanel);
            return songPanel;
        }

        void RemoveSongPanels()
        {
            for (int i = 0; i < SongPanelsList.Count; i++)
            {
                panel2.Controls.Remove(SongPanelsList[i]);
            }
        }
        void CreateQueuePanels()
        {
            //panel2.Controls.Clear();
            RemoveSongPanels();
            SongPanelsList.Clear();
            ResetScroller(panel2);
            for (int i = 0; i < QueueDict.Count; i++)
            {
                int index = Convert.ToInt32(QueueDict.Keys.ElementAt(i));
                //Console.WriteLine(index);
                string SongName = GetNestedDictionary(QueueDict, index, "SongName");
                string Artist = GetNestedDictionary(QueueDict, index, "Artist");
                string SongDuration = GetNestedDictionary(QueueDict, index, "SongDuration");
                SongDuration = TimeSpan.FromSeconds(Convert.ToInt32(SongDuration)).ToString("mm\\:ss");
                string Album = GetNestedDictionary(QueueDict, index, "Album");
                string SongFromUser = GetNestedDictionary(QueueDict, index, "SongFromUser");

                QueueForms.SongPanel SongPanel = CreateSongPanel(i + 1, SongName, Artist, SongDuration, Album, SongFromUser, i + 2, "");
                SongPanel.Name = $"SongPanel_{i + 1}";
                //SongPanel.BorderStyle = BorderStyle.FixedSingle;
                SongPanel.Location = new Point(0, (iLabel4.Location.Y + iLabel4.Height + 10) + (65 * i));
                SongPanel.Size = new Size(panel2.Width, 60);//SongPanel_0.Size;
                //SongPanel.BackColor = Color.Red;
                SongPanel.MouseEnter += SongPanel_MouseEnter;
                SongPanel.MouseLeave += SongPanel_MouseLeave;
                SongPanel.MouseClick += SongPanel_MouseClick;

                foreach (Control control in SongPanel.Controls)
                {
                    control.MouseEnter += SongPanel_MouseEnter;
                    control.MouseLeave += SongPanel_MouseLeave;
                    control.MouseClick += SongPanel_MouseClick;
                }

                SongPanelsList.Add(SongPanel);
                panel2.Controls.Add(SongPanel);

            }
            SetScroller(panel2);

        }


        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point((this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);
                this.Update();
            }
        }

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private async void iButton25_Click(object sender, EventArgs e)
        {
            await GetQueueInfo();

            int index = Convert.ToInt32(QueueDict.Keys.ElementAt(QueueDict.Count - 1)) + 1;
            //Console.WriteLine(index);

            Dictionary<string, Dictionary<string, string>> AddQueueDict = new Dictionary<string, Dictionary<string, string>>
            {
                {
                    index.ToString(),
                    new Dictionary<string, string>
                    {
                        {"Album","10"},
                        {"Artist","11"},
                        {"SongDuration","12"},
                        {"SongFromUser","13"},
                        {"SongImage","14"},
                        {"SongLink","15"},
                        {"SongName","16"},
                    }
                },
            };
            await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(Global.PartyMode_SessionID).Collection("queue").Document("queue").SetAsync(AddQueueDict, SetOptions.MergeAll);

        }


        private void SongPanel_MouseClick(object sender, MouseEventArgs e)
        {

            SongPanelsList[oldSelectedPanel].BackColor = ControlPaint.Light(gradientPanel1.StartColor, 0.4f);
            if (e.Button == MouseButtons.Right)
            {
                OpenContextMenu();
            }

        }

        private void SongPanel_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                //if (iContextSelectedPanel != oldSelectedPanel && iContextStripMenu.Visible)
                if (iContextStripMenu.Visible)
                {
                    if (iContextSelectedPanel != oldSelectedPanel)
                    {
                        SongPanelsList[oldSelectedPanel].BackColor = Color.Transparent;
                    }
                }
                else
                {
                    SongPanelsList[oldSelectedPanel].BackColor = Color.Transparent;
                }
                //SongPanelsList[oldSelectedPanel].BackColor = Color.Transparent;
            }
            catch { }
        }

        private void SongPanel_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(Global.GetControl().Split('_')[1]) - 1;

                if (index < 0)
                    return;

                if (iContextStripMenu.Visible)
                {
                    if (iContextSelectedPanel != index)
                        SongPanelsList[index].BackColor = ControlPaint.Light(gradientPanel1.StartColor, 0.2f);
                }
                else
                {
                    SongPanelsList[index].BackColor = ControlPaint.Light(gradientPanel1.StartColor, 0.2f);
                }
                oldSelectedPanel = index;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void iButton24_Click(object sender, EventArgs e)
        {
            iButton23.BackColor = Color.Black;
            iButton24.BackColor = Color.FromArgb(20, 20, 20);
            iLabel2.Visible = false;
            iLabel1.Text = "Zuletzt gehört";
            QueueSelected = false;
            ResetScroller(panel2);
        }

        private void iButton23_Click(object sender, EventArgs e)
        {
            iButton24.BackColor = Color.Black;
            iButton23.BackColor = Color.FromArgb(20, 20, 20);
            iLabel2.Visible = true;
            iLabel1.Text = "Warteschlange";
            QueueSelected = true;
            ResetScroller(panel2);
        }

        private void iButton21_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void iButton22_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void SongPanel_0_MouseEnter(object sender, EventArgs e)
        {
            SongPanel_0.BackColor = ControlPaint.Light(gradientPanel1.StartColor, 0.2f);
        }

        private void SongPanel_0_MouseLeave(object sender, EventArgs e)
        {
            SongPanel_0.BackColor = Color.Transparent;

        }

        private void SongPanel_0_MouseClick(object sender, MouseEventArgs e)
        {
            SongPanel_0.BackColor = ControlPaint.Light(gradientPanel1.StartColor, 0.4f);
        }


        private async void iButton26_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { "10", FieldValue.Delete }
            };
            await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(Global.PartyMode_SessionID).Collection("queue").Document("queue").UpdateAsync(updates);
            iContextStripMenu.Hide();
        }

        private void ContextMenuStrip_FocusCheckTimer_Tick(object sender, EventArgs e)
        {
            if (iContextStripMenu.ContainsFocus == false)
            {
                iContextStripMenu.Hide();
                SongPanelsList[iContextSelectedPanel].BackColor = Color.Transparent;
                ContextMenuStrip_FocusCheckTimer.Stop();
            }
        }

        void OpenContextMenu()
        {
            try
            {
                //int index = Convert.ToInt32(Global.GetControl().Split('_')[1]) - 1;
                iContextStripMenu.StartPosition = FormStartPosition.Manual;
                iContextStripMenu.ShowInTaskbar = false;
                iContextStripMenu.Location = new Point(MousePosition.X + 10, MousePosition.Y + 10);
                if (iContextStripMenu.Visible == false)
                {
                    ContextMenuStrip_FocusCheckTimer.Start();
                    iContextSelectedPanel = oldSelectedPanel;
                    iContextStripMenu.Show();
                }
                else
                {
                    iContextStripMenu.Focus();
                }
            }
            catch { }

        }
        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {

        }
    }
}
