using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusikPlayer.Methods.Partymode
{
    class PartyMode_Struct
    {
        public struct Users
        {
            public string Username;
            public string UsernameID;
            public string Email;
            public string Role;
            public int index;
            public string FolderID;
        }

        public enum Roles
        {
            Listener = 0,
            SongSelector = 1,
            VIP = 2,
            Host = 3,
        }

        public struct QueueStruct
        {
            public int ID { get; set; }
            public QueueInfo queueInfos { get; set; }
        }
        public struct QueueInfo
        {
            public string SongFromUser { get; set; }
            public string Artist { get; set; }
            public string SongName { get; set; }
            public string Album { get; set; }
            public string SongDuration { get; set; }
            public string SongLink { get; set; }
            public string SongImage { get; set; }
        }
    }
}
