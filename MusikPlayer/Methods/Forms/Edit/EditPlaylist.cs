using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods.Forms.Edit
{
    public partial class EditPlaylist : Form
    {
        public int PlaylistIndex;

        public EditPlaylist()
        {
            InitializeComponent();
            iButton1.Location = new Point(iPanel22.Width - iButton1.Height - 15, (iPanel22.Height - iButton1.Height) / 2);
            iLabel2.Location = new Point((iPanel22.Width - iLabel2.Width) / 2, (iPanel22.Height - iLabel2.Height) / 2);

            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 5, 5));
            iTextBox1.RemovePlaceholder();
            this.Opacity = 0;
            OpenFormFadeTimer.Start();
        }

        #region RoundedBorder
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
     (
         int nLeftRect,     // x-coordinate of upper-left corner
         int nTopRect,      // y-coordinate of upper-left corner
         int nRightRect,    // x-coordinate of lower-right corner
         int nBottomRect,   // y-coordinate of lower-right corner
         int nWidthEllipse, // width of ellipse
         int nHeightEllipse // height of ellipse
     );
        #endregion


        private void OpenFormFadeTimer_Tick(object sender, EventArgs e)
        {
            if (Opacity >= 1)
                OpenFormFadeTimer.Stop();
            this.Opacity += .08;
        }
        private void CloseFormFadeTimer_Tick(object sender, EventArgs e)
        {
            if (Opacity <= 0)
            {
                Form1 form1 = Application.OpenForms.OfType<Form1>().Single();
                form1.Opacity = 1;
                this.Close();
                CloseFormFadeTimer.Stop();
            }
            this.Opacity -= .08;
        }

        private void iButton21_Click(object sender, EventArgs e)
        {
            //RenamePlaylistName();
            RenamePlaylistName_Better();
            CloseFormFadeTimer.Start();
        }
        private void iButton1_Click(object sender, EventArgs e)
        {
            CloseFormFadeTimer.Start();
        }
        public void Show(string PlaylistName)
        {
            iTextBox1.Texts = PlaylistName;
            this.Show();
        }

        public void RenamePlaylistName()
        {
            try
            {
                string FileName = $"{Global.current_path}/Playlists.json";
                string Old_PlaylistName = CreateControls.iLabels[PlaylistIndex].Text;
                string New_PlaylistName = iTextBox1.Texts;

                if (Old_PlaylistName == New_PlaylistName)
                    return;


                Form1 form1 = Application.OpenForms.OfType<Form1>().Single();
                if (form1.mainPlayer != null)
                    MainPlayer.imageStream.Close();

                string songURL = Global.mediaElement.Source.AbsolutePath; //Global.mplayer.URL;
                SongPlayer.PauseSong();
                Global.mediaElement.Source = new Uri("");
                //Global.mplayer.URL = "";


                List<DataHandling.Playlist> Playlist = DataHandling.Handler.ReturnJsonFile<DataHandling.Playlist>(FileName);
                var jsonParsed = JsonObject.Parse(File.ReadAllText(FileName));

                List<DataHandling.Playlist> newJson = JsonConvert.DeserializeObject<List<Methods.DataHandling.Playlist>>(jsonParsed.ToString());
                newJson[PlaylistIndex].PlaylistName = iTextBox1.Texts;
                newJson[PlaylistIndex].PlaylistPath = $"{Global.current_path}/Playlists/{iTextBox1.Texts}";
                newJson[PlaylistIndex].JsonPath = $"{Global.current_path}/Playlists/{iTextBox1.Texts}/{iTextBox1.Texts}.json";


                if (Directory.Exists($"{Global.current_path}/Playlists/{Old_PlaylistName}"))
                    Directory.Move($"{Global.current_path}/Playlists/{Old_PlaylistName}", $"{Global.current_path}/Playlists/{New_PlaylistName}");

                if (File.Exists($"{Global.current_path}/Playlists/{New_PlaylistName}/{Old_PlaylistName}.json"))
                    File.Move($"{Global.current_path}/Playlists/{New_PlaylistName}/{Old_PlaylistName}.json", $"{Global.current_path}/Playlists/{New_PlaylistName}/{New_PlaylistName}.json");

                string newJsonFileName = $"{Global.current_path}/Playlists/{New_PlaylistName}/{New_PlaylistName}.json";
                List<DataHandling.Songs> newSongsJson = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(newJsonFileName);

                for (int i = 0; i < newSongsJson.Count; i++)
                {
                    newSongsJson[i].SongPath = $"{Global.current_path}/Playlists/{New_PlaylistName}/{newSongsJson[i].SongAuthor}_{newSongsJson[i].SongName}/{newSongsJson[i].SongName}.mp3";
                    newSongsJson[i].ImagePath = $"{Global.current_path}/Playlists/{New_PlaylistName}/{newSongsJson[i].SongAuthor}_{newSongsJson[i].SongName}/{newSongsJson[i].SongName}.png";
                }

                string jsonData = JsonConvert.SerializeObject(newJson, Formatting.Indented);
                string newSongsjsonData = JsonConvert.SerializeObject(newSongsJson, Formatting.Indented);

                File.WriteAllText(FileName, jsonData);
                File.WriteAllText(newJsonFileName, newSongsjsonData);

                Global.ReadPlaylists(form1.panel6, CreateControls.iLabels);
                /*if (form1.mainPlayer != null)
                    form1.mainPlayer.iLabel1.Text = New_PlaylistName;


                Global.mplayer.URL = songURL;
                Global.Selected_Playlist.Index = PlaylistIndex;
                Global.Selected_Playlist.PlaylistName = CreateControls.iLabels[PlaylistIndex].Text;
                Global.Selected_Playlist = Methods.DataHandling.Handler.ReturnJsonFile<DataHandling.Playlist>(Global.playlists_path)[PlaylistIndex];
                Global.Current_PlayingSong = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(newJsonFileName)[PlaylistIndex - 1];*/
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RenamePlaylistName - Error:\n{ex.Message}");
            }
        }

        public void RenamePlaylistName_Better()
        {
            string FileName = $"{Global.current_path}/Playlists.json";
            string Old_PlaylistName = CreateControls.iLabels[PlaylistIndex].Text;
            string New_PlaylistName = iTextBox1.Texts;
            if (Old_PlaylistName == New_PlaylistName || Directory.Exists($"{Global.current_path}/Playlists/{New_PlaylistName}"))
                return;

            Form1 form1 = Application.OpenForms.OfType<Form1>().Single();
            CopyFilesRecursively($"{Global.current_path}/Playlists/{Old_PlaylistName}", $"{Global.current_path}/Playlists/{New_PlaylistName}");


            List<DataHandling.Playlist> Playlist = DataHandling.Handler.ReturnJsonFile<DataHandling.Playlist>(FileName);
            var jsonParsed = JsonObject.Parse(File.ReadAllText(FileName));
            List<DataHandling.Playlist> newJson = JsonConvert.DeserializeObject<List<Methods.DataHandling.Playlist>>(jsonParsed.ToString());

            newJson[PlaylistIndex].PlaylistName = iTextBox1.Texts;
            newJson[PlaylistIndex].PlaylistPath = $"{Global.current_path}/Playlists/{iTextBox1.Texts}";
            newJson[PlaylistIndex].JsonPath = $"{Global.current_path}/Playlists/{iTextBox1.Texts}/{iTextBox1.Texts}.json";

            string oldJsonFileName = $"{Global.current_path}/Playlists/{New_PlaylistName}/{Old_PlaylistName}.json";
            string newJsonFileName = $"{Global.current_path}/Playlists/{New_PlaylistName}/{New_PlaylistName}.json";
            List<DataHandling.Songs> newSongsJson = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(oldJsonFileName);

            for (int i = 0; i < newSongsJson.Count; i++)
            {
                newSongsJson[i].SongPath = $"{Global.current_path}/Playlists/{New_PlaylistName}/{newSongsJson[i].SongAuthor}_{newSongsJson[i].SongName}/{newSongsJson[i].SongName}.mp3";
                newSongsJson[i].ImagePath = $"{Global.current_path}/Playlists/{New_PlaylistName}/{newSongsJson[i].SongAuthor}_{newSongsJson[i].SongName}/{newSongsJson[i].SongName}.png";
            }

            string jsonData = JsonConvert.SerializeObject(newJson, Formatting.Indented);
            string newSongsjsonData = JsonConvert.SerializeObject(newSongsJson, Formatting.Indented);

            File.WriteAllText(FileName, jsonData);
            File.WriteAllText(newJsonFileName, newSongsjsonData);
            File.Delete($"{Global.current_path}/Playlists/{New_PlaylistName}/{Old_PlaylistName}.json");

            Global.Selected_Playlist = DataHandling.Handler.ReturnJsonFile<DataHandling.Playlist>(Global.playlists_path)[PlaylistIndex];
            Global.Current_PlayingSong = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(newJsonFileName)[PlaylistIndex - 1];

            Global.ReadPlaylists(form1.panel6, CreateControls.iLabels);
            if (form1.mainPlayer != null)
                form1.mainPlayer.iLabel1.Text = New_PlaylistName;

            Directory.Delete($"{Global.current_path}/Playlists/{Old_PlaylistName}", true);


        }

        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
    }

}
