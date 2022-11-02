using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;
using YoutubeExplode;
using YoutubeExplode.Converter;

namespace MusikPlayer.Methods.Forms
{
    public partial class Youtube_Downloader : Form
    {
        string placeHolder;
        int SongDuration;

        public List<Design.iLabel> Playlist_Labels = new List<Design.iLabel>();
        public List<Design.iCheckBox> Playlist_CheckBox = new List<Design.iCheckBox>();

        public List<int> Checked_CheckBox_Indices = new List<int>();
        int PlaylistsCount = DataHandling.Handler.ReturnJsonFile<DataHandling.Playlist>("Playlists.json").Count;

        private bool mouseDown;
        private Point lastLocation;
        Design.iWaitDialog iWaitDialog = new Design.iWaitDialog();

        public Youtube_Downloader()
        {
            InitializeComponent();
            var form1 = Application.OpenForms.OfType<Form1>().Single();
            this.Size = new Size(675, 505);

            placeHolder = iTextBox2.PlaceholderText;
            Add_CheckBoxesToList();


            for (int i = 0; i < PlaylistsCount; i++)
            {
                string Content = DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.Playlist>("Playlists.json")[i].PlaylistName;
                if (i == 0)
                    Content = form1.FavouriteSongsLabel.Text;

                CreateControls.Create_iLabel(Playlist_Labels, i, Content, new Font("Microsoft Sans Serif", 10, FontStyle.Regular), new Point(0, 0 + (i * 35)),
                new Size(iPanel1.Width, 50), false, true);

                iPanel1.Controls.Add(Playlist_Labels[i]);
                iPanel1.Controls.Add(Playlist_CheckBox[i]);

            }
        }


        void Add_CheckBoxesToList()
        {
            Color fillColor = Color.FromArgb(255, 20, 20, 20);
            Point Location;
            for (int i = 0; i < PlaylistsCount; i++)
            {
                Location = new Point(iPanel1.Width - 30, 35 * i);
                Playlist_CheckBox.Add(CreateControls.iCheckBox(Location, fillColor));
            }
        }



        string[] SplitString(string Text, string Chars)
        {
            return Text.Split(new string[] { Chars }, StringSplitOptions.None);
        }

        string ConvertToUseableString(string Text)
        {
            string[] cutChars = { "(", "[", "{" };
            string[] cutStrings = { "feat.", "prod." };

            Text = RemoveIllegalChars(Text);

            if (Text.Contains("(") || Text.Contains("[") || Text.Contains("{"))
            {
                for (int i = 0; i < Text.Length; i++)
                {
                    for (int j = 0; j < cutChars.Length; j++)
                    {
                        if ((char)Text[i] == Convert.ToChar(cutChars[j]))
                        {
                            return SplitString(Text, cutChars[j])[0];
                        }
                    }
                }
            }

            if (Text.Contains("feat."))
            {
                return SplitString(Text, "feat.")[0];
            }
            return Text;
        }
        string RemoveIllegalChars(string text)
        {
            string[] illegal = { "\"", "-" };
            foreach (string c in illegal)
            {
                text = text.Replace(c, "");
            }

            return text;
        }
        private async void iButton21_Click(object sender, EventArgs e)
        {
            iWaitDialog = new Design.iWaitDialog();
            try
            {
                iWaitDialog.Show("Die Suche läuft");

                YoutubeClient youtube = new YoutubeClient();
                YoutubeExplode.Videos.Video video = await youtube.Videos.GetAsync(iTextBox1.Texts);
                iTextBox2.RemovePlaceholder();
                iTextBox4.RemovePlaceholder();
                iTextBox3.RemovePlaceholder();
                iTextBox5.RemovePlaceholder();
                iTextBox2.Texts = video.Title;
                string BandName = "";
                string SongName = "";
                string SearchQuery = "";



                this.Size = new Size(675, 505);
                WebClient client = new WebClient();
                #region Napster_API
                //Napster_API napster_API = new Napster_API();
                /*if (napster_API.GetImageURL(BandName, SongName) != "Image Not Found")
                {
                    Console.WriteLine(napster_API.GetImageURL(BandName, SongName));
                    byte[] Data = client.DownloadData(napster_API.GetImageURL(BandName, SongName));
                    MemoryStream memstr = new MemoryStream(Data);
                    iPictureBox1.Image = Image.FromStream(memstr);
                    this.Size = new Size(1101, this.Height);
                }*/
                #endregion

                Napster_API2 napster_API2 = new Napster_API2();

                SearchQuery = ConvertToUseableString(RemoveIllegalChars(iTextBox2.Texts));
                Console.WriteLine(SearchQuery);

                BandName = napster_API2.GetArtistName(SearchQuery);
                if (BandName == "Not Found")
                {
                    if (video.Title.Contains("-"))
                    {
                        iTextBox4.Texts = ConvertToUseableString(RemoveIllegalChars(SplitString(video.Title, " - ")[0]));
                        iTextBox3.Texts = ConvertToUseableString(RemoveIllegalChars(SplitString(video.Title, " - ")[1]));
                    }
                    else
                    {
                        iTextBox4.Texts = ConvertToUseableString(RemoveIllegalChars(video.Title));
                        iTextBox3.Texts = ConvertToUseableString(RemoveIllegalChars(video.Author.ToString()));
                    }

                    if (iTextBox4.Texts.EndsWith(" "))
                        iTextBox4.Texts = iTextBox4.Texts.Substring(0, iTextBox4.Texts.Length - 1);
                    if (iTextBox3.Texts.EndsWith(" "))
                        iTextBox3.Texts = iTextBox3.Texts.Substring(0, iTextBox3.Texts.Length - 1);

                    iTextBox5.Texts = "Not Found";
                    SongDuration = (int)video.Duration.Value.TotalSeconds;
                    iPictureBox1.Image = Resources_Images.Images.Icons.music_logo;
                    iPictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.Size = new Size(1101, this.Height);
                    searchState = SearchState.Searched;
                }
                else
                {
                    SongName = napster_API2.GetSongName(SearchQuery);
                    iTextBox4.Texts = BandName;
                    iTextBox3.Texts = SongName;
                    iTextBox5.Texts = napster_API2.GetAlbumName(SearchQuery);
                    SongDuration = (int)video.Duration.Value.TotalSeconds;
                }

                if (napster_API2.GetImageURL(BandName, SongName, new Size(400, 400)) != "Image Not Found")
                {
                    Console.WriteLine(napster_API2.GetImageURL(BandName, SongName, new Size(400, 400)));
                    string ImageUri = napster_API2.GetImageURL(BandName, SongName, new Size(400, 400));
                    string InvalidImage = "https://api.napster.com/imageserver/v2/albums/alb.24285420/images/400x400.jpg";
                    if (ImageUri != InvalidImage)
                    {
                        byte[] Data = client.DownloadData(ImageUri);
                        MemoryStream memstr = new MemoryStream(Data);
                        iPictureBox1.Image = Image.FromStream(memstr);
                        this.Size = new Size(1101, this.Height);
                    }
                }
                searchState = SearchState.Searched;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                Global.iMessageBox.Show(ex.Message);
            }
            finally
            {
                iWaitDialog.Hide();
            }
        }

        private void iTextBox2__TextChanged(object sender, EventArgs e)
        {
            if (iTextBox2.Texts == "" && iTextBox2.PlaceholderText == "")
            {
                iTextBox2.PlaceholderColor = Color.Gray;
                iTextBox2.PlaceholderText = placeHolder;
            }
        }

        private void gradientBackground1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void gradientBackground1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void gradientBackground1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void iButton1_Click(object sender, EventArgs e)
        {
            if (iPictureBox1.Image != null)
                iPictureBox1.Image.Dispose();
            this.Hide();
        }

        private void iButton3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        enum SearchState
        {
            None,
            Searched
        }

        enum Download_Status
        {
            None,
            Starting,
            Finished
        }
        Download_Status DownloadStatus = Download_Status.None;
        SearchState searchState = SearchState.None;

        async void YT_Downloader(string YTLink, string outputPath, string Format)
        {
            var youtube = new YoutubeClient();
            DownloadStatus = Download_Status.Starting;
            await youtube.Videos.DownloadAsync(YTLink, $"{outputPath}.{Format}", o => o
               .SetFormat(Format) // override format
               .SetPreset(ConversionPreset.UltraFast) // change preset
               .SetFFmpegPath($"{Global.current_path}/ffmpeg/ffmpeg.exe") // custom FFmpeg location
           );
            DownloadStatus = Download_Status.Finished;
        }

        void UploadToFirestoreStorage()
        {
            Console.WriteLine($"AB: {Checked_CheckBox_Indices.Count}");
            for (int i = 0; i < Checked_CheckBox_Indices.Count; i++)
            {
                int CheckBoxVal = Checked_CheckBox_Indices[i];
                string Artist = iTextBox4.Texts;
                string SongName = iTextBox3.Texts;

                string SongArtistName = $"{Artist}_{SongName}";
                string PlaylistName = $"{Playlist_Labels[CheckBoxVal].Text}";
                string DirPath = Global.current_path + $"/Playlists/{Playlist_Labels[CheckBoxVal].Text}/{SongArtistName}";
                string SongPath = $"{DirPath}/{SongName}.mp3";
                string ImagePath = $"{DirPath}/{SongName}.png";

                if (iPictureBox1.Image == Resources_Images.Images.Icons.music_logo)
                    Firebase_Methods.Storage_Methods.UploadSong(SongPath, ImagePath, PlaylistName, SongName, SongArtistName);
                else
                    Firebase_Methods.Storage_Methods.UploadSong(SongPath, ImagePath, PlaylistName, SongName, SongArtistName, true);

            }
            Checked_CheckBox_Indices.Clear();
        }

        void Download_StatusChecker()
        {
            while (true)
            {
                if (DownloadStatus == Download_Status.Finished)
                {
                    //Global.iMessageBox.Show("Der Song wurde erfolgreich heruntergeladen");
                    this.Invoke((MethodInvoker)delegate
                    {
                        iWaitDialog.iLabel1.Text = "Der Song wurde erfolgreich heruntergeladen";
                        UploadToFirestoreStorage();
                    });
                    Thread.Sleep(1000);
                    this.Invoke((MethodInvoker)delegate
                    {
                        iWaitDialog.Hide();
                    });
                    DownloadStatus = Download_Status.None;
                    break;
                }
                this.Invoke((MethodInvoker)delegate
                {
                    //Console.WriteLine("Location Fix");
                    iWaitDialog.Location = new Point(this.Location.X + (this.Width - iWaitDialog.Width) / 2, this.Location.Y + (this.Height - iWaitDialog.Height) / 2);
                });

                //Thread.Sleep(200);
            }
        }


        void AddDataToJson(string SongArtist, string SongName, string SongDuration, string SongPath, string Album, string JsonFile, string ImagePath)
        {
            string Added_Date = DateTime.Now.ToString("dd. MMM yyyy");
            int Count = DataHandling.Methods.GetJsonCount(JsonFile);
            DataHandling.Handler.CreateJsonFile(Count, Count, SongDuration, Added_Date, SongArtist, SongName, Album, SongPath, JsonFile, ImagePath);
        }

        string PathFormatter(string Text)
        {
            if (Text.Length > 55)
                return SplitString(Text, "_")[0];
            else
                return Text;
        }
        bool CheckSongAlreadyExistsInPlaylist(int i)
        {
            string JsonFilePath = "";
            string Selected_Playlist = Playlist_Labels[Checked_CheckBox_Indices[i]].Text;
            if (Selected_Playlist == "Lieblingssongs")
                JsonFilePath = Global.current_path + $"/Playlists/FavouriteSongs/FavouriteSongs.json";
            else
                JsonFilePath = Global.current_path + $"/Playlists/{Playlist_Labels[Checked_CheckBox_Indices[i]].Text}/{Playlist_Labels[Checked_CheckBox_Indices[i]].Text}.json";

            for (int j = 0; j < DataHandling.Methods.GetJsonCount(JsonFilePath); j++)
            {
                if (DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(JsonFilePath)[j].SongName.Contains(iTextBox3.Texts) && DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(JsonFilePath)[j].SongAuthor.Contains(iTextBox4.Texts))
                {
                    return true;
                }
            }

            return false;
        }

        private async void iButton22_Click(object sender, EventArgs e)
        {

            try
            {

                string DirPath = "";
                string Album = "";
                string SongPath = "";
                string JsonFilePath = "";
                string ImagePath = "";
                for (int i = 0; i < Playlist_CheckBox.Count; i++)
                {
                    if (Playlist_CheckBox[i].Checked == true)
                    {
                        Checked_CheckBox_Indices.Add(i);
                    }
                }
                if (searchState == SearchState.None)
                {
                    Global.iMessageBox.Show("Du hast noch nicht nach einem Song gesucht");
                    return;
                }
                if (Checked_CheckBox_Indices.Count == 0)
                {
                    Global.iMessageBox.Show("Es wurde keine Playlist ausgewählt");
                    return;
                }


                if (DownloadStatus != Download_Status.Starting)
                {
                    iWaitDialog = new Design.iWaitDialog();
                    for (int i = 0; i < Checked_CheckBox_Indices.Count; i++)
                    {
                        if (CheckSongAlreadyExistsInPlaylist(i) == false)
                        {
                            int CheckBoxVal = Checked_CheckBox_Indices[i];

                            if (CheckBoxVal == 0)
                            {
                                DirPath = Global.current_path + $"/Playlists/FavouriteSongs/{iTextBox4.Texts}_{iTextBox3.Texts}";
                                JsonFilePath = Global.current_path + $"/Playlists/FavouriteSongs/FavouriteSongs.json";
                            }
                            else
                            {
                                DirPath = Global.current_path + $"/Playlists/{Playlist_Labels[CheckBoxVal].Text}/{iTextBox4.Texts}_{iTextBox3.Texts}";
                                JsonFilePath = Global.current_path + $"/Playlists/{Playlist_Labels[CheckBoxVal].Text}/{Playlist_Labels[CheckBoxVal].Text}.json";
                            }

                            ImagePath = $"{DirPath}/{iTextBox4.Texts}_{iTextBox3.Texts}.png";
                            SongPath = $"{DirPath}/{iTextBox4.Texts}_{iTextBox3.Texts}.mp3";
                            Album = iTextBox5.Texts;
                            //Console.WriteLine(SongPath);
                            if (!Directory.Exists(DirPath))
                                Directory.CreateDirectory(DirPath);

                            if (ImagePath.Length > 55)
                            {
                                ImagePath = $"{DirPath}/{ConvertToUseableString(iTextBox3.Texts)}.png";
                                SongPath = $"{DirPath}/{ConvertToUseableString(iTextBox3.Texts)}";
                            }
                            if (iPictureBox1.Image != null)
                                iPictureBox1.Image.Save(ImagePath);

                            YT_Downloader(iTextBox1.Texts, SongPath, "mp3");
                            AddDataToJson(iTextBox4.Texts, iTextBox3.Texts, SongPlayer.currentTimeFormat(SongDuration), $"{SongPath}.mp3", Album, JsonFilePath, ImagePath);
                            if (i < 1)
                                iWaitDialog.Show("Der Download wurde gestartet");
                            //Global.iMessageBox.Show("Der Download wurde gestartet");
                        }
                        else
                        {
                            Global.iMessageBox.Show($"Der Song \"{iTextBox3.Texts} - {iTextBox4.Texts}\" existiert bereits in der Playlist \"{Playlist_Labels[Checked_CheckBox_Indices[i]].Text}\"");
                        }
                    }
                }
                else
                {
                    Global.iMessageBox.Show("Ein Song wird momenatan heruntergeladen");
                }

                Thread t1 = new Thread(Download_StatusChecker);
                t1.Start();
            }
            catch (Exception ex)
            {
                iWaitDialog.Hide();
                Console.WriteLine(ex.Message);
                Global.iMessageBox.Show("Download Error");
            }
        }

        private void iButton23_Click(object sender, EventArgs e)
        {
            iPanel1.AutoScrollPosition = new Point(-3, -3);
        }


        private void iButton24_Click(object sender, EventArgs e)
        {
            //Console.WriteLine($"{iPanel1.VerticalScroll.Value}");
        }


        async Task API_Auth()
        {
            //client_id={api_key}&client_secret={api_secret}&response_type=code&grant_type=authorization_code&redirect_uri={redirect_uri}&code={temporary_code}"
            var client = new HttpClient();
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api.napster.com/v2.2/me/account"),
                Headers = { { "Authorization", "Bearer YTMyOTkyNmYtMGIzMy00YzQwLWE0YzgtNmE5MzBmYmE2NmVh" } }
            };
            var response = await client.SendAsync(requestMessage);
            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            //Console.WriteLine(iPanel1.VerticalScroll.LargeChange);
        }
        private void PlaylistScrollbar_HoldTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                /*float calc = (iScrollBarV1.PointToClient(Cursor.Position).Y / (float)iScrollBarV1.Height);
                float MaxValue = iScrollBarV1.MaxValue;
                float DeciNums = MaxValue * calc;

                if (DeciNums <= 0)
                    iPanel1.AutoScrollPosition = new Point(iPanel1.AutoScrollPosition.X, 0);
                if (DeciNums >= MaxValue)
                    iPanel1.VerticalScroll.Value = iScrollBarV1.Value;
                //if ((int)DeciNums > iScrollBarV1.Value + iScrollBarV1.SmallChange || iScrollBarV1.Value < (int)DeciNums + iScrollBarV1.SmallChange)
                iScrollBarV1.Value = (int)DeciNums - iScrollBarV1.SmallChange;
                iPanel1.VerticalScroll.Value = iScrollBarV1.Value;*/

                iPanel1.VerticalScroll.Value = vScrollBar1.Value;
                //iPanel1.AutoScrollPosition = new Point(iPanel1.AutoScrollPosition.X, iPanel1.VerticalScroll.Maximum);
                //Console.WriteLine($"{(int)DeciNums} {vScrollBar1.Value}");
            }
            catch
            {
                //iPanel1.AutoScrollPosition = new Point(iPanel1.AutoScrollPosition.X, iScrollBarV1.Value);
                Console.WriteLine("Error");
            }
        }

        private void iScrollBarV1_MouseDown(object sender, MouseEventArgs e)
        {
            PlaylistScrollbar_HoldTimer.Start();
        }

        private void iScrollBarV1_MouseUp(object sender, MouseEventArgs e)
        {
            PlaylistScrollbar_HoldTimer.Stop();
        }

        private void iScrollBarV1_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch { }
        }


        int iPanel1MaxVal = 0;
        private void vScrollBar1_Scroll_1(object sender, ScrollEventArgs e)
        {

            //Console.WriteLine($"{iPanel1.PreferredSize.Height - iPanel1.Height} | {iPanel1.VerticalScroll.Maximum} | {vScrollBar1.Value}");
            try
            {
                if (vScrollBar1.Value <= 0)
                    iPanel1.AutoScrollPosition = new Point(iPanel1.AutoScrollPosition.X, 0);
                if (iPanel1.PreferredSize.Height - iPanel1.Height >= 0)
                {
                    iPanel1MaxVal++;
                    iPanel1.VerticalScroll.Maximum = vScrollBar1.Value + iPanel1MaxVal;
                    iPanel1.VerticalScroll.Value = vScrollBar1.Value;
                }
            }
            catch
            {

            }
        }

        private void iButton23_Click_2(object sender, EventArgs e)
        {
            string g = "Buried In Verona - Can't Be Unsaid (Official Music Video)";
            Console.WriteLine(g);
            Console.WriteLine(ConvertToUseableString(RemoveIllegalChars(g)));
        }

        private void Youtube_Downloader_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (iPictureBox1.Image != null)
                iPictureBox1.Image.Dispose();
        }
    }
}