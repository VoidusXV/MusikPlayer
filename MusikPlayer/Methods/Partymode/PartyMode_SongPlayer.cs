using Google.Cloud.Firestore;
using Microsoft.Toolkit.Forms.UI.Controls;
using MusikPlayer.Methods.Authentication;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Media;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Streams;
using WMPLib;

namespace MusikPlayer.Methods.Partymode
{
    public class PartyMode_SongPlayer
    {
        public class PartyModeSongs
        {
            public string SongURL { get; set; }
            public string SongName { get; set; }
            public string Artist { get; set; }
            public int SongDuration { get; set; }
            public int SongPosition { get; set; }
            public string SongFromUser { get; set; }

            public string ImageURL { get; set; }
        }

        public static WindowsMediaPlayer mediaPlayer = new WMPLib.WindowsMediaPlayer();
        public static PartyModeSongs PartyModeSongsVars = new PartyModeSongs();
        public static Thread songThread = new Thread(new ThreadStart(UI_Thread));



        public static bool PartyModeAuthorization()
        {
            if (Global.PartyModeRole == "Listener" || string.IsNullOrEmpty(Global.PartyModeRole))
            {
                Global.iMessageBox.Show("Du hast nicht die Berechtigung einen Song abzuspielen oder zur Warteschlange hinzuzufügen.\nUm einen Song abzuspielen benötigst du die Berechtigung oder verlasse den Raum, um wieder im Solo-Modus Songs abzuspielen", "Keine Berechtigung");
                return false;
            }
            else
            {
                return true;
            }
        }

        public static async Task PartyMode_SetSong(string Artist, string SongName, int SongDuration, bool StartNewSong = false)
        {
            string SongArtistName = $"{Artist}_{SongName}";
            string PlaylistName = Global.Selected_Playlist.PlaylistName;
            string SongLink = await FirestoreGlobal.firebaseStorage.Child("Clients").Child(Global.client_Data.folderID).Child("Playlists").Child(PlaylistName).Child(SongArtistName).Child($"{SongName}.mp3").GetDownloadUrlAsync();//Song File ;
                                                                                                                                                                                                                                    //string ImageLink = "";

            Dictionary<string, object> data = new Dictionary<string, object>()
                {
                    {"Artist", Artist},
                    {"SongFromUser", $"{Global.Authentication.Username}#{Global.Authentication.UsernameID}"},
                    {"SongLink", SongLink},
                    {"SongName", SongName},
                    {"SongDuration", SongDuration},

                };
            if (StartNewSong)
            {
                data.Add("SongPosition", 0);
                data.Add("PlayState", "Play");
            }
            await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(Global.PartyMode_SessionID).SetAsync(data, SetOptions.MergeAll);
        }
        private static async Task PartyMode_SetPlayState(string PlayState, int SongPosition)
        {

            if (Global.PartyModeRole == "Listener" || string.IsNullOrEmpty(Global.PartyModeRole))
            {
                Global.iMessageBox.Show("Du hast nicht die Berechtigung einen Song abzuspielen.\nUm einen Song abzuspielen benötigst du die Berechtigung oder verlasse den Raum, um wieder im Solo-Modus Songs abzuspielen", "Keine Berechtigung");
            }
            else
            {
                Dictionary<string, object> data = new Dictionary<string, object>()
                {
                    {"PlayState", PlayState},
                    {"SongPosition", SongPosition},
                };
                await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(Global.PartyMode_SessionID).SetAsync(data, SetOptions.MergeAll);
            }
        }

        public static void UI_Thread()
        {
            Form1 form1 = Application.OpenForms.OfType<Form1>().Single();
            Global.SongEnded = false;
            int SongDuration = PartyModeSongsVars.SongDuration;
            for (int i = Global.SongPausedTime; i <= SongDuration; i++)
            {
                form1.Invoke((MethodInvoker)delegate
                {
                    form1.label1.Text = TimeSpan.FromSeconds(form1.iTrackBar1.Value).ToString("mm\\:ss");
                });
                i = (int)Math.Round((double)form1.iTrackBar1.Value);
                form1.iTrackBar1.Value++;
                Thread.Sleep(1000);
                if (i == SongDuration)
                    Global.SongEnded = true;
            }
        }

        public static ObjectCache songChache = MemoryCache.Default;

        public static string GetSongPath_ByArtistSongName(string SongName, string Artist)
        {
            string JsonFilePath;
            string JsonSongName;
            string JsonArtistName;

            int playlistCount = DataHandling.Methods.GetJsonCount(Global.playlists_path);

            for (int i = 0; i < playlistCount; i++)
            {
                JsonFilePath = DataHandling.Handler.ReturnJsonFile<DataHandling.Playlist>(Global.playlists_path)[i].JsonPath;
                for (int j = 0; j < DataHandling.Methods.GetJsonCount(JsonFilePath); j++)
                {
                    JsonSongName = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(JsonFilePath)[j].SongName;
                    JsonArtistName = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(JsonFilePath)[j].SongAuthor;
                    if (JsonSongName == SongName && JsonArtistName == Artist)
                    {
                        return DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(JsonFilePath)[j].SongPath;
                    }
                }
            }
            return "";
        }

        public static async void PlaySong(string URL, string SongName, string Artist, int SongDuration, string ImageURL = "")
        {
            try
            {
                Form1 form1 = Application.OpenForms.OfType<Form1>().Single();
                form1.Invoke((MethodInvoker)async delegate
                {
                    //string URL2 = @"C:\Users\HlDE1\source\repos\MusikPlayer\MusikPlayer\bin\Debug\Playlists\FavouriteSongs\Secrets_Somewhere in Hiding\Somewhere in Hiding.mp3";

                    mediaPlayer.URL = URL;

                    if (form1.iTrackBar1.MaxValue == form1.iTrackBar1.Value)
                        SongPlayer.ResetPlayer();

                    form1.iTrackBar1.MaxValue = SongDuration;


                    form1.iCircleButton1.Image = Global.resizeImage(Resources_Images.Images.Icons.pause, new Size(25, 25));

                    form1.label2.Text = TimeSpan.FromSeconds(SongDuration).ToString("mm\\:ss");
                    form1.iLabel7.Text = SongName;
                    form1.iLabel8.Text = Artist;

                    Global.SetToolTip(form1.iLabel7, SongName, SongPlayer.SongNameToolTip);
                    Global.SetToolTip(form1.iLabel8, Artist, SongPlayer.ArtistToolTip);

                    if (ImageURL == "")
                        form1.iPictureBox5.Image = Resources_Images.Images.Icons.music_logo;
                    else

                        form1.iPictureBox5.ImageLocation = ImageURL;



                    //if (PartyMode_SongPlayer.mediaPlayer.playState != WMPPlayState.wmppsPlaying)

                    //mediaPlayer.controls.currentPosition = (double)PartyModeSongsVars.SongPosition;
                    mediaPlayer.controls.play();
                    mediaPlayer.controls.currentPosition = (double)form1.iTrackBar1.Value;

                    await FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).UpdateAsync(new Dictionary<string, object> { { "CurrentSong", $"{Artist} - {SongName}" } });

                    form1.iCircleButton1.Enabled = true;
                    form1.iCircleButton2.Enabled = true;
                    form1.iCircleButton3.Enabled = true;
                    form1.iTrackBar1.Enabled = true;
                    form1.panel10.Visible = true;
                    form1.Text = $"{Artist} - {SongName}";
                });


                if (Global.titleExport == Structures.Obs_SongTitleExport.On)
                    SongPlayer.ExportSongTitle(Artist, SongName);



                songThread.Abort();
                songThread = new Thread(() => UI_Thread());
                songThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, "PartyPlaySong");
            }
        }
        public static async void SetPlayOnServer(int SongPosition = -1)
        {
            if (SongPosition == -1)
                await PartyMode_SetPlayState("Play", (int)Math.Round(mediaPlayer.controls.currentPosition));
            else
                await PartyMode_SetPlayState("Play", SongPosition);

        }

        public static async void SetPlayOnServer()
        {
            await PartyMode_SetPlayState("Play", (int)Math.Round(mediaPlayer.controls.currentPosition));
        }
        public static async void SetPauseOnServer()
        {
            Global.SongPausedTime = (int)Math.Round(mediaPlayer.controls.currentPosition);
            await PartyMode_SetPlayState("Pause", Global.SongPausedTime);
        }
        public static async void SetTrackValueOnServer(int Value)
        {
            await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(Global.PartyMode_SessionID).UpdateAsync(new Dictionary<string, object> { { "SongPosition", Value } });
            mediaPlayer.controls.currentPosition = Value;
        }

        public static void PauseSong()
        {
            Form1 form1 = Application.OpenForms.OfType<Form1>().Single();
            form1.Invoke((MethodInvoker)delegate
            {
                form1.iCircleButton1.Image = Global.resizeImage(Resources_Images.Images.Icons.play, new Size(15, 15));
                mediaPlayer.controls.pause();
            });
            songThread.Abort();
        }
    }
}
