using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusikPlayer.Methods
{
    public class Structures
    {
        public enum Loop
        {
            Off,
            LoopPlaylist,
            LoopSong,
        }

        public enum Obs_SongTitleExport
        {
            Off,
            On
        }
   
        public struct Client_Credentials
        {
            public string Username { get; set; }
            public string UsernameID { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Register_Date { get; set; }
            public string Last_Login { get; set; }
            public string IP { get; set; }
            public string folderID { get; set; }
        }

        public class Client_Messages
        {
            public string Type { get; set; }
            public string Username { get; set; }
            public string UsernameID { get; set; }
            public string Email { get; set; }
        }

        public struct Client_Data
        {
            public string Username { get; set; }
            public string CurrentSong { get; set; }
            public string Status { get; set; }
            public string FolderID { get; set; }

        }

    }
}
