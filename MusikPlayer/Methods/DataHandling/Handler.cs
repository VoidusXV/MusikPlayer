using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusikPlayer.Methods.DataHandling
{
    public class Songs
    {
        public int ID { get; set; }
        public int Index { get; set; }
        public string AddedDate { get; set; }
        public string SongAuthor { get; set; }
        public string SongName { get; set; }
        public string Album { get; set; }
        public string SongDuration { get; set; }
        public string SongPath { get; set; }
        public string ImagePath { get; set; }

    }

    public class Playlist
    {
        public int ID { get; set; }
        public int Index { get; set; }
        public string AddedDate { get; set; }
        public string AddedTime { get; set; }
        public string PlaylistName { get; set; }
        public string PlaylistPath { get; set; }
        public string JsonPath { get; set; }

    }

    public class FormSettings
    {
        public Songs songs { get; set; }
        public Playlist playlist { get; set; }

        public int PlaylistIndex { get; set; }

        public int Volume { get; set; }
        public int SongPausedTime { get; set; }
        public bool Shuffle { get; set; }
        public Structures.Loop Loop { get; set; }
        public string FormWindowState { get; set; }
        public Size FormSize { get; set; }

    }

    public class Authentication
    {
        public string Username { get; set; }
        public string UsernameID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Register_Date { get; set; }

    }

    public class ProfileSettings
    {
        public bool RememberMe { get; set; }
        public bool AutoLogin { get; set; }
    }
    class Handler
    {
        public static void CreateJsonFile(int ID, int Index, string SongDuration, string AddedDate, string SongAuthor, string SongName, string SongPath, string FileName)
        {
            Songs global = new Songs()
            {
                ID = ID,
                Index = Index,
                AddedDate = AddedDate,
                SongAuthor = SongAuthor,
                SongName = SongName,
                SongPath = SongPath,
                SongDuration = SongDuration,

            };
            List<Songs> temp = ReturnJsonFile<Songs>(FileName);
            temp.Add(global);
            string strJson = JsonConvert.SerializeObject(temp, Formatting.Indented);
            File.WriteAllText(FileName, strJson);
        }

        public static void CreateJsonFile(int ID, int Index, string SongDuration, string AddedDate, string SongAuthor, string SongName, string Album, string SongPath, string FileName, string ImagePath)
        {
            Songs global = new Songs()
            {
                ID = ID,
                Index = Index,
                AddedDate = AddedDate,
                SongAuthor = SongAuthor,
                SongName = SongName,
                Album = Album,
                SongDuration = SongDuration,
                SongPath = SongPath,
                ImagePath = ImagePath,

            };
            List<Songs> temp = ReturnJsonFile<Songs>(FileName);
            temp.Add(global);
            string strJson = JsonConvert.SerializeObject(temp, Formatting.Indented);
            File.WriteAllText(FileName, strJson);
        }

        public static void CreateJsonFile_Playlist(int ID, int Index, string AddedDate, string AddedTime, string PlaylistName, string PlaylistPath, string JsonPath)
        {
            Playlist global = new Playlist()
            {
                ID = ID,
                Index = Index,
                AddedDate = AddedDate,
                AddedTime = AddedTime,
                PlaylistName = PlaylistName,
                PlaylistPath = PlaylistPath,
                JsonPath = JsonPath,
            };

            List<Playlist> temp = ReturnJsonFile<Playlist>("Playlists.json");
            temp.Add(global);
            string strJson = JsonConvert.SerializeObject(temp, Formatting.Indented);
            File.WriteAllText("Playlists.json", strJson);

        }

        public static void CreateJsonFile_FormSettings(Songs songs, Playlist playlist, int PlaylistIndex, int Volume, int SongPausedTime, bool Shuffle, Structures.Loop Loop, string FormWindowState, Size FormSize)
        {
            FormSettings formSettings = new FormSettings()
            {
                songs = songs,
                playlist = playlist,
                PlaylistIndex = PlaylistIndex,
                Volume = Volume,
                SongPausedTime = SongPausedTime,
                Shuffle = Shuffle,
                Loop = Loop,
                FormWindowState = FormWindowState,
                FormSize = FormSize
            };

            List<FormSettings> temp = new List<FormSettings>();
            temp.Add(formSettings);
            string strJson = JsonConvert.SerializeObject(temp, Formatting.Indented);
            File.WriteAllText("AppSettings.json", strJson);
        }

        public static void CreateJsonFile_Authentication(string Username, string UsernameID, string Email, string Password)
        {
            Authentication authentication = new Authentication()
            {
                Username = Username,
                UsernameID = UsernameID,
                Email = Email,
                Password = Password,
            };

            List<Authentication> temp = new List<Authentication>();
            temp.Add(authentication);
            string strJson = JsonConvert.SerializeObject(temp, Formatting.Indented);
            File.WriteAllText("Authentication.json", strJson);
        }

        public static void CreateJsonFile_ProfileSettings(bool RememberMe, bool AutoLogin)
        {
            ProfileSettings profileSettings = new ProfileSettings()
            {
                RememberMe = RememberMe,
                AutoLogin = AutoLogin,
            };

            List<ProfileSettings> temp = new List<ProfileSettings>();
            temp.Add(profileSettings);
            string strJson = JsonConvert.SerializeObject(temp, Formatting.Indented);
            File.WriteAllText("ProfileSettings.json", strJson);
        }

        public static void CreateMetaData(string Status, Image image = null)
        {
            Forms.Friendlist.Friends_Handler.Client_Data client_Data = new Forms.Friendlist.Friends_Handler.Client_Data()
            {
                Username = Global.Authentication.Username,
                UsernameID = Global.Authentication.UsernameID,
                Status = Status,
                profileImage = image,
            };
            string jsonData = JsonConvert.SerializeObject(client_Data, Formatting.Indented);
            File.WriteAllText("data.json", jsonData);
        }

        public static List<T> ReturnJsonFile<T>(string FileName)
        {
            if (!File.Exists(FileName))
                File.WriteAllText(FileName, "");
            //using (Global.fileStream = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite)) ;


            List<T> global = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(FileName));
            return (global == null) ? new List<T>() : global;
        }
    }

    class Methods
    {
        public static int GetJsonCount(string fileName)
        {
            int ID = Handler.ReturnJsonFile<Playlist>(fileName).Count;
            return ID;
        }
    }

}
