using Google.Cloud.Firestore;
using Microsoft.Toolkit.Forms.UI.Controls;
using MusikPlayer.Methods.Authentication;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Streams;
using WMPLib;

namespace MusikPlayer.Methods
{
    public class SongPlayer
    {
        public static Design.iToolTip SongNameToolTip = new Design.iToolTip();
        public static Design.iToolTip ArtistToolTip = new Design.iToolTip();

        string FormatDectection()
        {
            if (Global.songDuration.Split(':').Length - 1 == 1)
                return "mins";
            else if (Global.songDuration.Split(':').Length - 1 == 2)
                return "hours";
            return "Error";
        }

        public int durationInSeconds()
        {
            if (FormatDectection() == "mins")
            {
                int minute = Convert.ToInt32(Global.songDuration.Split(':')[0]);
                int seconds = Convert.ToInt32(Global.songDuration.Split(':')[1]);
                int durationInSeconds = ((minute * 60) + seconds);

                return durationInSeconds;
            }
            return -1;
        }


        public static string currentTimeFormat(int currentTime)
        {
            //float f = 125 % 60;
            double a = currentTime / 60;

            int min = (currentTime - (currentTime % 60)) / 60;
            int sec = (currentTime % 60);

            if (a == 0)
            {
                if (currentTime < 10)
                    return $"00:0{sec}";
                else
                    return $"00:{sec}";
            }
            else
            {
                if (sec < 10)
                    return $"0{min}:0{sec}";
                else
                    return $"0{min}:{sec}";
            }
        }

        public static int SongDurationToSeconds(string Time)
        {
            string[] splitDuration = Time.Split(':');
            int minutes = Convert.ToInt32(splitDuration[0]);
            int seconds = minutes * 60 + Convert.ToInt32(splitDuration[1]);

            return seconds;
        }

        public string SongURLFormat(string songURL)
        {
            int index = songURL.Split('\\').Length;
            return songURL.Split('\\')[index - 1].Split('.')[0];
        }


        public static string GetSongNameFromFile(string filename)
        {
            return filename.Split('_')[0];
        }


        public static string GetArtistFromFile(string filename)
        {
            filename = filename.Split('_')[1];
            return filename.Substring(0, filename.Length - 4);
        }

        public static string GetSongDuration(string filename)
        {
            WindowsMediaPlayer mplayer = new WindowsMediaPlayer();
            IWMPMedia mediaInformation = mplayer.newMedia(filename);
            return mediaInformation.durationString;
        }

        public static int GetPlaylistIndexByPlaylistName(string PlaylistName)
        {
            int count = DataHandling.Handler.ReturnJsonFile<DataHandling.Playlist>("Playlists.json").Count;
            for (int i = 0; i < count; i++)
            {
                if (PlaylistName == DataHandling.Handler.ReturnJsonFile<DataHandling.Playlist>("Playlists.json")[i].PlaylistName)
                    return i;
                else if (PlaylistName == "Lieblingssongs")
                    return 0;
            }
            return -1;
        }

        public static Thread songThread = new Thread(new ThreadStart(UI_Thread));

        public static Thread songSkipThread = new Thread(new ThreadStart(SongSkipThread));

        public static void ExportSongTitle(string Artist, string SongName)
        {
            File.WriteAllText($"{Global.current_path}/currentSong.txt", $"{Artist} - {SongName}");
        }

        public static void UI_Thread()
        {
            Form1 form1 = Application.OpenForms.OfType<Form1>().Single();
            //Global.SongPlaying = true;
            Global.systemControls.PlaybackStatus = MediaPlaybackStatus.Playing;
            int SongDuration = SongDurationToSeconds(Global.Current_PlayingSong.SongDuration);
            Global.SongEnded = false;
            for (int i = Global.SongPausedTime; i <= SongDuration; i++)
            {
                form1.Invoke((MethodInvoker)delegate
                {
                    form1.label1.Text = currentTimeFormat(form1.iTrackBar1.Value);
                });
                i = (int)Math.Round((double)form1.iTrackBar1.Value); //(int)Math.Round(Global.mplayer.controls.currentPosition);
                Global.Discord_Presence_Duration(currentTimeFormat(i), currentTimeFormat(SongDuration));
                form1.iTrackBar1.Value++;
                Thread.Sleep(1000);
                if (i == SongDuration)
                    Global.SongEnded = true;
            }

            string JsonPath = DataHandling.Handler.ReturnJsonFile<DataHandling.Playlist>("Playlists.json")[Global.Selected_Playlist.ID].JsonPath;
            int SongsCount = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(JsonPath).Count - 1;
            if (Global.CurrentPlayingSongInPlaylist <= SongsCount || Global.LoopSongs == Structures.Loop.LoopPlaylist)
            {
                songSkipThread = new Thread(new ThreadStart(SongSkipThread));
                songSkipThread.Start();
            }
            else
            {
                ResetPlayer();
                PauseSong();
            }
        }

        public static void SongSkipThread()
        {
            if (Global.SongEnded == true && Global.LoopSongs != Structures.Loop.LoopSong && Global.Shuffle == false)
            {
                SkipSongForward();
            }
            else if (Global.LoopSongs == Structures.Loop.LoopSong)
            {
                SkipSongForward(0);
            }
            else if (Global.Shuffle == true && Global.LoopSongs != Structures.Loop.LoopSong)
            {
                ShuffleSong();
            }
            else
            {
                PauseSong();
            }
            songSkipThread.Abort();
        }


        public static async void PlaySong(string URL, string SongName, string Artist, int CurrentPos, string SongDuration, string ImageURL = "")
        {
            if (!File.Exists(URL))
            {
                Global.iMessageBox.Show("Der Song existiert nicht", "Error");
                return;
            }



            Form1 form1 = Application.OpenForms.OfType<Form1>().Single();

            form1.Invoke((MethodInvoker)async delegate
            {
                Global.mediaElement.Source = new Uri(URL);
                //Global.mediaElement.Source = MediaSource.CreateFromUri(new Uri());

                if (form1.iTrackBar1.MaxValue == form1.iTrackBar1.Value)
                    ResetPlayer();

                Design.iToolTip iToolTip = new Design.iToolTip();

                form1.iTrackBar1.MaxValue = SongDurationToSeconds(SongDuration);
                form1.iCircleButton1.Image = Global.resizeImage(Resources_Images.Images.Icons.pause, new Size(25, 25));

                form1.label2.Text = SongDuration;
                form1.iLabel7.Text = SongName;
                form1.iLabel8.Text = Artist;

                Global.SetToolTip(form1.iLabel7, SongName, SongNameToolTip);
                Global.SetToolTip(form1.iLabel8, Artist, ArtistToolTip);

                if (ImageURL == "")
                {
                    form1.iPictureBox5.Image = Resources_Images.Images.Icons.music_logo;

                    MemoryStream memoryStream = new MemoryStream();
                    Resources_Images.Images.Icons.music_logo.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    Global.systemControls.DisplayUpdater.Thumbnail = RandomAccessStreamReference.CreateFromStream(memoryStream.AsRandomAccessStream());

                }
                else if (File.Exists(ImageURL))
                {
                    using (Stream imageStream = File.OpenRead(ImageURL))
                    {
                        form1.iPictureBox5.Image = Image.FromStream(imageStream);
                    }
                }


                SongPlayer.PlayMedia();
                Global.mediaElement.Position = TimeSpan.FromSeconds(form1.iTrackBar1.Value);

                form1.panel6.Controls.Add(form1.CurrentPlaying_Icon);
                form1.CurrentPlaying_Icon.Location = new Point(form1.panel6.Width - 25, (Global.Current_PlayingPlaylist.Index - 1) * 35);

                form1.iCircleButton1.Enabled = true;
                form1.iCircleButton2.Enabled = true;
                form1.iCircleButton3.Enabled = true;
                form1.iTrackBar1.Enabled = true;
                form1.panel10.Visible = true;
                form1.Text = $"{Artist} - {SongName}";
            });


            if (Global.titleExport == Structures.Obs_SongTitleExport.On)
                ExportSongTitle(Artist, SongName);


            //DataHandling.Handler.CreateMetaData($"{Artist} - {SongName}");
            //Multithreading.Background_UploadData_Thread = new Thread(Multithreading.Background_Upload_MetaData);
            //Multithreading.Background_UploadData_Thread.Start();
            //Global.Discord_Presence(SongName, Artist);

            songThread.Abort();
            songThread = new Thread(new ThreadStart(UI_Thread));
            songThread.Start();


            #region MediaElements
            Global.systemControls.DisplayUpdater.Type = MediaPlaybackType.Music;
            StorageFile storageFile = await StorageFile.GetFileFromPathAsync(Path.GetFullPath(ImageURL));
            Global.systemControls.DisplayUpdater.Thumbnail = RandomAccessStreamReference.CreateFromFile(storageFile);
            Global.systemControls.DisplayUpdater.MusicProperties.Title = SongName;
            Global.systemControls.DisplayUpdater.MusicProperties.Artist = Artist;
            Global.systemControls.DisplayUpdater.Update();
            #endregion
        }

        public static async void PauseSong()
        {
            Form1 form1 = Application.OpenForms.OfType<Form1>().Single();
            PauseMedia();
            form1.Invoke((MethodInvoker)delegate
            {
                Global.SongPausedTime = (int)Math.Round(Global.mediaElement.Position.TotalSeconds);
                form1.iCircleButton1.Image = Global.resizeImage(Resources_Images.Images.Icons.play, new Size(15, 15));
                form1.panel6.Controls.Remove(form1.CurrentPlaying_Icon);
            });

            //DataHandling.Handler.CreateMetaData($"Online");
            //Multithreading.Background_UploadData_Thread = new Thread(Multithreading.Background_Upload_MetaData);
            //Multithreading.Background_UploadData_Thread.Start();

            //Global.SongPlaying = false;
            songThread.Abort();
        }
        public static void UpdateCurrentPlaylist(int index)
        {

            string JsonPath = DataHandling.Handler.ReturnJsonFile<DataHandling.Playlist>("Playlists.json")[index].JsonPath;
            int SongCount = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(JsonPath).Count;
            string SongPath = "";
            Global.CurrentPlaylistSongsPath.Clear();
            for (int i = 0; i < SongCount; i++)
            {
                SongPath = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(JsonPath)[i].SongPath;
                Global.CurrentPlaylistSongsPath.Add(SongPath);
            }
        }
        public static void ResetPlayer()
        {
            Form1 form1 = Application.OpenForms.OfType<Form1>().Single();
            form1.Invoke((MethodInvoker)delegate
            {
                form1.label1.Text = "00:00";
                form1.label2.Text = "00:00";
                form1.iTrackBar1.Value = 0;
                form1.panel10.Visible = false;
                form1.iCircleButton1.Enabled = false;
                form1.iCircleButton2.Enabled = false;
                form1.iCircleButton3.Enabled = false;
                form1.iTrackBar1.Enabled = false;
                form1.iTrackBar1.Value = 0;
            });
            Global.SongPausedTime = 0;
        }

        public static void SkipSongForward(int Skip = 1)
        {
            if (Global.Selected_Playlist.Index == -1)
                return;


            if (Forms.MainPlayer.Title_Labels.Count > Global.CurrentPlayingSongInPlaylist) //&& DataHandling.Handler.ReturnJsonFile<DataHandling.FormSettings>("AppSettings.json")[0].playlist.PlaylistName == Global.Selected_Playlist.PlaylistName)
                Forms.MainPlayer.Title_Labels[Global.CurrentPlayingSongInPlaylist].ForeColor = Color.White;

            Global.CurrentPlayingSongInPlaylist += Skip;
            if (Global.CurrentPlayingSongInPlaylist >= Global.CurrentPlaylistSongsPath.Count)
                Global.CurrentPlayingSongInPlaylist = 0;

            PlaySongByIndex(Global.CurrentPlayingSongInPlaylist);

        }

        public static void SkipSongBackward()
        {
            if (Global.Selected_Playlist.Index == -1)
                return;

            if (Forms.MainPlayer.Title_Labels.Count > Global.CurrentPlayingSongInPlaylist)
                Forms.MainPlayer.Title_Labels[Global.CurrentPlayingSongInPlaylist].ForeColor = Color.White;

            Global.CurrentPlayingSongInPlaylist--;
            if (Global.CurrentPlayingSongInPlaylist < 0)
                Global.CurrentPlayingSongInPlaylist = Global.CurrentPlaylistSongsPath.Count - 1;

            PlaySongByIndex(Global.CurrentPlayingSongInPlaylist);
        }

        public static void ShuffleSong()
        {
            if (Global.Selected_Playlist.Index == -1)
                return;

            if (Forms.MainPlayer.Title_Labels.Count > Global.CurrentPlayingSongInPlaylist)
                Forms.MainPlayer.Title_Labels[Global.CurrentPlayingSongInPlaylist].ForeColor = Color.White;

            Random random = new Random();
            int randomIndex = random.Next(0, Global.CurrentPlaylistSongsPath.Count - 1);
            Global.CurrentPlayingSongInPlaylist = randomIndex;
            PlaySongByIndex(Global.CurrentPlayingSongInPlaylist);

        }
        public static void PlaySongByIndex(int index)
        {
            if (Global.Selected_Playlist.Index == -1 || index > Global.CurrentPlaylistSongsPath.Count - 1)
                return;

            //string JsonPath = DataHandling.Handler.ReturnJsonFile<DataHandling.Playlist>("Playlists.json")[GetPlaylistIndexByPlaylistName(Global.Playing_Playlist)].JsonPath;
            string JsonPath = DataHandling.Handler.ReturnJsonFile<DataHandling.Playlist>("Playlists.json")[Global.Selected_Playlist.ID].JsonPath;
            DataHandling.Songs Song = DataHandling.Handler.ReturnJsonFile<DataHandling.Songs>(JsonPath)[index];
            Global.Current_PlayingSong = Song;

            ResetPlayer();
            PlaySong(Global.CurrentPlaylistSongsPath[index], Song.SongName, Song.SongAuthor, 0, Song.SongDuration, Song.ImagePath);

            if (Forms.MainPlayer.Title_Labels.Count > index)
                Forms.MainPlayer.Title_Labels[index].ForeColor = Color.FromArgb(255, 0, 200, 0);

        }

        public static void SystemControls_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            Form1 form1 = Application.OpenForms.OfType<Form1>().Single();

            switch (args.Button)
            {

                case SystemMediaTransportControlsButton.Play:
                    PlaySong(Global.Current_PlayingSong.SongPath, Global.Current_PlayingSong.SongName, Global.Current_PlayingSong.SongAuthor, Global.SongPausedTime, Global.Current_PlayingSong.SongDuration, Global.Current_PlayingSong.ImagePath);
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    PauseSong();
                    break;
                case SystemMediaTransportControlsButton.Next:
                    SkipSongForward();
                    UpdateCurrentPlaylist(Global.Current_PlayingPlaylist.Index);
                    break;
                case SystemMediaTransportControlsButton.Previous:
                    if (form1.iTrackBar1.Value <= 2)
                        SongPlayer.SkipSongBackward();
                    else
                        form1.iTrackBar1.Value = 0;
                    form1.Invoke((MethodInvoker)delegate
                    {
                        SongPlayer.UpdateCurrentPlaylist(Global.Current_PlayingPlaylist.Index);
                        Global.mediaElement.Position = TimeSpan.FromSeconds(form1.iTrackBar1.Value);
                    });

                    break;
                default:
                    break;
            }
        }


        public static async void PlayMedia()
        {
            Global.mediaElement.Dispatcher.Invoke(() =>
            {
                Global.mediaElement.Play();
                Global.systemControls.PlaybackStatus = MediaPlaybackStatus.Playing;
            });

        }

        static async void PauseMedia()
        {
            Global.mediaElement.Dispatcher.Invoke(() =>
            {
                Global.mediaElement.Pause();
                Global.systemControls.PlaybackStatus = MediaPlaybackStatus.Paused;
            });
        }
    }
}
