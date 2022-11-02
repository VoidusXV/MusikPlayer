using Google.Cloud.Firestore;
using MusikPlayer.Methods.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods.Partymode
{
    public partial class PartyMode_Room : Form
    {
        FirestoreChangeListener firestoreMemberList_Listener;
        FirestoreChangeListener firestoreSession_Listener;

        private bool mouseDown;
        private Point lastLocation;

        private static string mySessionID = "";
        private string RoomKey = "";
        private string RoomName = "";
        //private string myRole = "";
        private bool RoomKeyHide;

        static Dictionary<string, PartyMode_Struct.Users> Users = new Dictionary<string, PartyMode_Struct.Users>();
        //List<PartyMode_Struct.Users> Users = new List<PartyMode_Struct.Users>();

        int old_KickMemberIndex = -1;
        int old_BanMemberIndex = -1;
        int old_VIPMemberIndex = -1;
        int old_SongSelectorMemberIndex = -1;

        static string myUsername = $"{Global.Authentication.Username}#{Global.Authentication.UsernameID}";
        PartyMode_Queue partyMode_Queue;

        public PartyMode_Room()
        {
            InitializeComponent();
            Main();

            //LayoutAdjust();
            //Animations animations = new Animations();
            //animations.OpenFormFade_AnimationStart(this);
            iScrollBarV1.MovingState = false;
        }
        private void PartyMode_Room_Load(object sender, EventArgs e)
        {
            partyMode_Queue = new PartyMode_Queue();
        }

        void LayoutAdjust()
        {
            iLabel1.Location = new Point((iPanel21.Width - iLabel1.Width) / 2, (iPanel21.Height - iLabel1.Height) / 2);
            iLabel4.Location = new Point((iPanel25.Width - iLabel4.Width) / 2, (iPanel25.Height - iLabel4.Height) / 2);
        }

        async void Main()
        {
            await GetSessionInfo();
            await GetSettings();
            ListenMembers();
            ListenSession();
        }

        async Task GetSessionInfo()
        {
            var myFields = await FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).GetSnapshotAsync();
            mySessionID = myFields.ToDictionary()["Session"].ToString();
            var sessionFields = await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID).GetSnapshotAsync();
            Dictionary<string, object> sessionFieldsDict = sessionFields.ToDictionary();
            RoomKey = sessionFieldsDict["RoomKey"].ToString();
            RoomName = sessionFieldsDict["SessionName"].ToString();
            iLabel4.Text = $"Schlüssel: {RoomKey}";
            iLabel1.Text = RoomName;
            this.Text = $"Party - {RoomName}";
        }

        async Task GetRole()
        {
            var memberListSnapshot = await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID).Collection("members").Document("members").GetSnapshotAsync();
            Dictionary<string, object> memberListDict = memberListSnapshot.ToDictionary();
            Global.PartyModeRole = memberListDict[Global.Authentication.Email].ToString();
        }

        async Task GetSettings()
        {
            var sessionFieldSnapshot = await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID).GetSnapshotAsync();
            Dictionary<string, object> sessionFieldDict = sessionFieldSnapshot.ToDictionary();
            RoomKeyHide = (bool)sessionFieldDict["RoomKeyHide"];
        }
        public static async Task<string> GetSongLink(string Artist, string SongName)
        {
            string PlaylistName = Global.Current_PlayingPlaylist.PlaylistName;
            string SongLink = await FirestoreGlobal.firebaseStorage.Child("Clients").Child(Global.client_Data.folderID).Child("Playlists").Child(PlaylistName).Child($"{Artist}_{SongName}").Child($"{SongName}.mp3").GetDownloadUrlAsync();//Song File ;
            return SongLink;
        }
        void HostMethods()
        {
            iLabel3.Click -= LeaveRoom_Click;
            iLabel3.Click += EndRoom_Click;
            iLabel3.Text = "Raum beenden";
            iButton24.Visible = true;
            iLabel4.Visible = true;
        }
        void VIPMethods()
        {
            iLabel3.Click -= EndRoom_Click;
            iLabel3.Click += LeaveRoom_Click;
            iLabel3.Text = "Raum verlassen";
            iButton24.Visible = true;
            iLabel4.Visible = true;
        }
        void MemberMethods()
        {
            iLabel3.Click -= EndRoom_Click;
            iLabel3.Click += LeaveRoom_Click;
            iLabel3.Text = "Raum verlassen";
            iButton24.Visible = false;
            iLabel4.Visible = !RoomKeyHide;
        }

        void GlobalMethods()
        {
            if (Global.PartyModeRole == "Host")
                HostMethods();
            else if (Global.PartyModeRole == "VIP")
                VIPMethods();
            else
                MemberMethods();
        }


        private async Task<bool> UserExists(string Email)
        {
            var emailDocs = FirestoreGlobal.firestoreDb.Collection("Users").Document(Email);
            DocumentSnapshot emailDocs_snapshot = await emailDocs.GetSnapshotAsync();

            if (emailDocs_snapshot.Exists == true)
                return true;

            return false;
        }
        void ClearLists()
        {
            roomMemberPanels.Clear();
            Users.Clear();

            kickMemberPictureBoxes.Clear();
            banMemberPictureBoxes.Clear();
            VIPMemberPictureBoxes.Clear();
            SongSelectorMemberPictureBoxes.Clear();
            iToolTip1.RemoveAll();
        }

        private async Task<string> GetStorageImageUri(string FolderID)
        {
            try
            {
                var storageChild = FirestoreGlobal.firebaseStorage.Child("Clients").Child(FolderID).Child("profileImage.png");
                string Uri = await storageChild.GetDownloadUrlAsync();
                return Uri;
            }
            catch { return null; }
        }

        private async Task ShowMembers()
        {
            ClearLists();

            var memberListSnapshot = await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID).Collection("members").Document("members").GetSnapshotAsync();
            Dictionary<string, object> memberListDict = memberListSnapshot.ToDictionary();
            int index = 0;
            foreach (KeyValuePair<string, object> valuePair in memberListDict)
            {
                if (await UserExists(valuePair.Key))
                {
                    DocumentSnapshot UserCredentials_Snapshot = await FirestoreGlobal.firestoreDb.Collection("Users").Document(valuePair.Key).Collection("data").Document("credentials").GetSnapshotAsync();
                    Dictionary<string, object> UserCredentials_Dict = UserCredentials_Snapshot.ToDictionary();

                    DocumentSnapshot UserData = await FirestoreGlobal.firestoreDb.Collection("Users").Document(valuePair.Key).GetSnapshotAsync();
                    Dictionary<string, object> UserData_Dict = UserData.ToDictionary();

                    string Username = $"{UserCredentials_Dict["Username"]}#{UserCredentials_Dict["UsernameID"]}";
                    string myUsername = $"{Global.Authentication.Username}#{Global.Authentication.UsernameID}";

                    PartyMode_Struct.Users usersStruct = new PartyMode_Struct.Users();
                    usersStruct.Username = (string)UserCredentials_Dict["Username"];
                    usersStruct.UsernameID = (string)UserCredentials_Dict["UsernameID"];
                    usersStruct.Email = valuePair.Key;
                    usersStruct.Role = (string)valuePair.Value;
                    usersStruct.index = index;
                    usersStruct.FolderID = (string)UserData_Dict["folderID"];

                    if (!Users.ContainsKey(valuePair.Key))
                        Users.Add(valuePair.Key, usersStruct);


                    //CreateMember_Controls(index, Username, myUsername, usersStruct.Role, await GetStorageImageUri(usersStruct.FolderID));

                    CreateMember_Controls(index, Username, myUsername, usersStruct.Role, null);
                    index++;
                }
            }

            SortPanelByRoles();
            for (int i = 0; i < roomMemberPanels.Count; i++)
            {
                Console.WriteLine(roomMemberPanels[i].Item2);
                roomMemberPanels[i].Item1.Location = new Point(0, i * 65);
                roomMemberPanels[i].Item1.Show();
                iPanel22.Controls.Add(roomMemberPanels[i].Item1);
            }
        }

        private async Task<bool> GotKicked(string myEmail)
        {
            DocumentReference docRef = FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID).Collection("members").Document("members");
            DocumentSnapshot documentSnapshot = await docRef.GetSnapshotAsync();
            Dictionary<string, object> memberDict = documentSnapshot.ToDictionary();
            if (!memberDict.ContainsKey(myEmail))
                return true;
            return false;
        }

        public void ListenMembers()
        {
            try
            {
                DocumentReference docRef = FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID).Collection("members").Document("members");
                firestoreMemberList_Listener = docRef.Listen(async snapshot =>
                {
                    if (await GotKicked(Global.Authentication.Email))
                    {
                        Global.iMessageBox.Show("Du wurdest aus dem Raum gekickt", "Gekickt");
                        this.Invoke((MethodInvoker)delegate
                        {
                            this.Hide();
                        });
                        return;
                    }

                    await GetRole();

                    this.Invoke((MethodInvoker)delegate
                    {
                        iPanel22.Controls.Clear();
                        GlobalMethods();
                    });
                    this.Invoke((MethodInvoker)async delegate
                    {
                        await ShowMembers();
                        LayoutAdjust();
                    });

                    if (Global.PartyModeRole == "Host")
                    {
                        await PartyMode_Methods.SetOptionSongPlayer_WhenJoined();
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        public void ListenSession()
        {
            try
            {
                Form1 form1 = Application.OpenForms.OfType<Form1>().Single();

                DocumentReference docRef = FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID);
                firestoreSession_Listener = docRef.Listen(async snapshot =>
                {
                    //Console.WriteLine($"Role: {Global.PartyModeRole}");

                    DocumentSnapshot sessionSnapshot = await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(Global.PartyMode_SessionID).GetSnapshotAsync();
                    Dictionary<string, object> sessionDict = sessionSnapshot.ToDictionary();

                    string Album = sessionDict["Album"].ToString();
                    string Artist = sessionDict["Artist"].ToString();
                    string PlayState = sessionDict["PlayState"].ToString();
                    string SongURL = sessionDict["SongLink"].ToString();
                    string SongName = sessionDict["SongName"].ToString();
                    string SongFromUser = sessionDict["SongFromUser"].ToString();
                    int SongDuration = Convert.ToInt32(sessionDict["SongDuration"]);
                    int SongPosition = Convert.ToInt32(sessionDict["SongPosition"]);
                    string ImageURL = sessionDict["ImageLink"].ToString();

                    this.Invoke((MethodInvoker)delegate
                    {
                        form1.label1.Text = TimeSpan.FromSeconds(SongPosition).ToString("mm\\:ss");
                        form1.iLabel7.Text = SongName;
                        form1.iLabel8.Text = Artist;
                        form1.iTrackBar1.Value = SongPosition;

                        if (ImageURL == "")
                            form1.iPictureBox5.Image = Resources_Images.Images.Icons.music_logo;
                        else
                            form1.iPictureBox5.ImageLocation = ImageURL;

                        //--------------------------------------------//

                        partyMode_Queue.SongPanel_0.ArtistLabel.Text = Artist;
                        partyMode_Queue.SongPanel_0.AlbumLabel.Text = Album;
                        partyMode_Queue.SongPanel_0.SongLabel.Text = SongName;
                        partyMode_Queue.SongPanel_0.UsernameLabel.Text = SongFromUser;
                        partyMode_Queue.SongPanel_0.DurationLabel.Text = TimeSpan.FromSeconds(SongDuration).ToString("mm\\:ss");

                    });
                    form1.iTrackBar1.MaxValue = SongDuration;


                    Global.SongPausedTime = SongPosition;
                    PartyMode_SongPlayer.mediaPlayer.controls.currentPosition = SongPosition;

                    PartyMode_SongPlayer.PartyModeSongsVars.Artist = Artist;
                    PartyMode_SongPlayer.PartyModeSongsVars.SongName = SongName;
                    PartyMode_SongPlayer.PartyModeSongsVars.SongURL = SongURL;
                    PartyMode_SongPlayer.PartyModeSongsVars.SongDuration = SongDuration;
                    PartyMode_SongPlayer.PartyModeSongsVars.SongPosition = SongPosition;
                    PartyMode_SongPlayer.PartyModeSongsVars.SongFromUser = SongFromUser;
                    PartyMode_SongPlayer.PartyModeSongsVars.ImageURL = "";

                    PartyMode_SongPlayer.mediaPlayer.settings.volume = form1.iTrackBar2.Value;

                    //int SongPosition = Convert.ToInt32(sessionDict["SongPosition"]);

                    //Console.WriteLine("ListenSession");
                    //Console.WriteLine(Artist);
                    //Console.WriteLine(SongURL);

                    WebClient webClient = new WebClient();

                    string GetSongPath = GetSongPath_InPartyRoomFolder(Artist, SongName);
                    if (string.IsNullOrWhiteSpace(GetSongPath))
                        GetSongPath = PartyMode_SongPlayer.GetSongPath_ByArtistSongName(SongName, Artist);

                    if (!PartyRoomSong_Check(Artist, SongName) && string.IsNullOrWhiteSpace(GetSongPath))
                    {
                        if (CacheDownloaderDone)
                            CacheDownloader(Artist, SongName, SongURL);
                        Console.WriteLine("CacheDownloader");
                    }
                    if (!string.IsNullOrWhiteSpace(GetSongPath))
                        SongURL = GetSongPath;


                    Console.WriteLine($"2: {SongURL}");

                    if (PlayState != "Play")
                        PartyMode_SongPlayer.PauseSong();
                    else
                        PartyMode_SongPlayer.PlaySong(SongURL, SongName, Artist, SongDuration);

                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, "ListenSession");
            }
        }

        static Thread CacheDownloaderThread;
        static bool CacheDownloaderDone = true;


        static void CacheDownloader(string Artist, string SongName, string SongURL)
        {
            CacheDownloaderThread = new Thread(() =>
            {
                if (!Directory.Exists($"{Global.current_path}/PartyRoom"))
                    Directory.CreateDirectory($"{Global.current_path}/PartyRoom");
                if (!Directory.Exists($"{Global.current_path}/PartyRoom/{Global.PartyMode_SessionID}"))
                    Directory.CreateDirectory($"{Global.current_path}/PartyRoom/{Global.PartyMode_SessionID}");

                CacheDownloaderDone = false;
                WebClient webClient = new WebClient();
                byte[] songData = webClient.DownloadData(SongURL);
                Console.WriteLine("songData Done");
                PartyRoom_Cache(Artist, SongName, songData);
                CacheDownloaderDone = true;
                CacheDownloaderThread.Abort();
            });
            CacheDownloaderThread.Start();
        }


        public static void PartyRoom_Cache(string Artist, string SongName, byte[] songData)
        {
            if (string.IsNullOrEmpty(Global.PartyMode_SessionID))
                return;
            string SongPath = PartyMode_SongPlayer.GetSongPath_ByArtistSongName(SongName, Artist);
            if (!string.IsNullOrEmpty(SongPath))
                return;

            File.WriteAllBytes($"{Global.current_path}/PartyRoom/{Global.PartyMode_SessionID}/{Artist}_{SongName}.mp3", songData);
        }
        public static bool PartyRoomSong_Check(string Artist, string SongName)
        {
            string ArtistSongName = $"{Artist}_{SongName}";
            string Path = $"{Global.current_path}/PartyRoom/{Global.PartyMode_SessionID}/{ArtistSongName}.mp3";
            if (File.Exists(Path))
                return true;
            return false;
        }

        void Delete_PartyRoomCache()
        {
            if (Directory.Exists($"{Global.current_path}/PartyRoom/{Global.PartyMode_SessionID}"))
                Directory.Delete($"{Global.current_path}/PartyRoom", true);
        }

        void End_PartyRoomPlayer()
        {
            if (PartyMode_SongPlayer.songThread != null)
                PartyMode_SongPlayer.songThread.Abort();

            PartyMode_SongPlayer.mediaPlayer.controls.stop();
            SongPlayer.ResetPlayer();
        }

        string GetSongPath_InPartyRoomFolder(string Artist, string SongName)
        {
            if (!Directory.Exists($"{Global.current_path}/PartyRoom/{Global.PartyMode_SessionID}"))
                return "";

            string ArtistSongName = $"{Artist}_{SongName}";
            if (File.Exists($"{Global.current_path}/PartyRoom/{Global.PartyMode_SessionID}/{ArtistSongName}.mp3"))
                return $"{Global.current_path}/PartyRoom/{Global.PartyMode_SessionID}/{ArtistSongName}.mp3";

            return "";
        }

        async Task LeaveRoom()
        {
            try
            {
                Console.WriteLine("LeaveRoom");
                Global.PartyMode = false;
                await firestoreMemberList_Listener.StopAsync();
                await firestoreSession_Listener.StopAsync();
                End_PartyRoomPlayer();
                Delete_PartyRoomCache();
                await Logs($"{myUsername} hat den Raum verlassen");
                await FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).SetAsync(new Dictionary<string, object>() { { "Session", "" } }, SetOptions.MergeAll);

                if (Global.PartyModeRole == "Host")
                    return;

                Users.Remove(Global.Authentication.Email);
                Dictionary<string, object> deleteEmail_Dict = new Dictionary<string, object>()
                {
                    { Global.Authentication.Email, FieldValue.Delete }
                };
                await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID).Collection("members").Document("members").SetAsync(deleteEmail_Dict, SetOptions.MergeAll);
                //this.Hide();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void KickMember(string TargetEmail)
        {


            Users.Remove(TargetEmail);
            FirestoreGlobal.firestoreDb.Collection("Users").Document(TargetEmail).SetAsync(new Dictionary<string, object>() { { "Session", "" } }, SetOptions.MergeAll);
            Dictionary<string, object> deleteEmail_Dict = new Dictionary<string, object>()
            {
                { TargetEmail, FieldValue.Delete }
            };
            FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID).Collection("members").Document("members").SetAsync(deleteEmail_Dict, SetOptions.MergeAll);
        }
        void BanMember(string TargetEmail, string TargetUsername)
        {
            FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID).Collection("banList").Document("banList").SetAsync(new Dictionary<string, object>() { { TargetEmail, TargetUsername } }, SetOptions.MergeAll);
            KickMember(TargetEmail);
        }

        public async static Task Logs(string Action)
        {
            string Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")).ToString("dd.MM.yyyy");
            string Time = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")).ToString("HH:mm:ss");
            string _DateTime = $"{Date} - {Time}";
            DocumentSnapshot logsSnapshot = await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID).Collection("Logs").Document("Logs").GetSnapshotAsync();
            int index = logsSnapshot.ToDictionary().Count;

            Dictionary<string, Dictionary<string, string>> logsInfo = new Dictionary<string, Dictionary<string, string>>
            {
                {
                    index.ToString(),
                    new Dictionary<string, string>
                    {
                        {"Username", $"{Users[Global.Authentication.Email].Username}#{Users[Global.Authentication.Email].UsernameID}"},
                        {"Email", Global.Authentication.Email},
                        {"Action", Action},
                        {"DateTime", _DateTime}
                    }
                },
            };

            await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID).Collection("Logs").Document("Logs").SetAsync(logsInfo, SetOptions.MergeAll);
        }


        private void LeaveRoom_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void EndRoom_Click(object sender, EventArgs e)
        {
            Console.WriteLine("EndRoom");
            for (int i = 0; i < Users.Count; i++)
            {
                FirestoreGlobal.firestoreDb.Collection("Users").Document(Users.ElementAt(i).Value.Email).SetAsync(new Dictionary<string, object>() { { "Session", "" } }, SetOptions.MergeAll);
            }
            FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID).DeleteAsync();
            this.Hide();
        }


        void SortPanelByRoles()
        {
            roomMemberPanels = roomMemberPanels.OrderByDescending(x => x.Item2).ToList();
        }

        PartyMode_Struct.Roles GetRoleEnumID(string Role)
        {
            if (Role == "Host")
                return PartyMode_Struct.Roles.Host;
            else if (Role == "VIP")
                return PartyMode_Struct.Roles.VIP;
            else if (Role == "SongSelector")
                return PartyMode_Struct.Roles.SongSelector;
            else if (Role == "Listener")
                return PartyMode_Struct.Roles.Listener;

            return PartyMode_Struct.Roles.Listener;
        }

        List<(Design.iPanel, PartyMode_Struct.Roles)> roomMemberPanels = new List<(Design.iPanel, PartyMode_Struct.Roles)>();
        //List<Design.iPanel> roomMemberPanels = new List<Design.iPanel>();

        List<Design.iPictureBox> kickMemberPictureBoxes = new List<Design.iPictureBox>();
        List<Design.iPictureBox> banMemberPictureBoxes = new List<Design.iPictureBox>();
        List<Design.iPictureBox> VIPMemberPictureBoxes = new List<Design.iPictureBox>();
        List<Design.iPictureBox> SongSelectorMemberPictureBoxes = new List<Design.iPictureBox>();

        void CreateMember_Controls(int i, string Username, string myUsername, string Role, string ImageUri)
        {
            this.SuspendLayout();

            #region Create Panel
            Design.iPanel iPanel = new Design.iPanel();
            //iPanel.Dock = DockStyle.Top;
            //iPanel.BorderStyle = BorderStyle.FixedSingle;
            iPanel.Location = new Point(0, i * 65);
            iPanel.Size = new Size(iPanel22.Width, 60);
            #endregion

            #region Create Labels
            Design.iLabel Username_iLabel = new Design.iLabel();
            Username_iLabel.AutoSize = true;
            Username_iLabel.Text = Username;
            Username_iLabel.Font = new Font("Microsoft Sans Serif", 11.25f, FontStyle.Regular);
            Username_iLabel.ForeColor = Color.White;
            Username_iLabel.HoverAnimation = false;
            Username_iLabel.Location = new Point(60, 20);

            Design.iLabel You_iLabel = new Design.iLabel();
            You_iLabel.AlphaColor = 150;
            You_iLabel.Text = "Du";
            You_iLabel.Font = new Font("Microsoft Sans Serif", 11.25f, FontStyle.Regular);
            You_iLabel.ForeColor = Color.White;
            You_iLabel.HoverAnimation = false;
            You_iLabel.Location = new Point(iPanel22.Width - You_iLabel.Width, 20);
            #endregion

            #region Create PictureBox


            Design.iRoundedPictureBox iRoundedPictureBox = new Design.iRoundedPictureBox();
            iRoundedPictureBox.Location = new Point(10, 10);
            iRoundedPictureBox.Size = new Size(40, 40);
            if (ImageUri != null)
                iRoundedPictureBox.ImageLocation = ImageUri;
            else
                iRoundedPictureBox.Image = Resources_Images.Images.Icons.profileImage_Placeholder;
            iRoundedPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            #endregion

            #region Create Buttons

            #region VIP_Role
            Design.iPictureBox GiveVIP_Role = new Design.iPictureBox();
            GiveVIP_Role.Name = $"GiveVIPRole_{i}";
            GiveVIP_Role.Cursor = Cursors.Hand;
            if (Role != "VIP")
                GiveVIP_Role.Image = Resources_Images.Images.Icons.VIP_Rang;
            else if (Role == "VIP")
                GiveVIP_Role.Image = Resources_Images.Images.Icons.No_VIP;
            GiveVIP_Role.ImageSize = new Size(20, 20);
            GiveVIP_Role.SizeMode = PictureBoxSizeMode.CenterImage;
            GiveVIP_Role.Size = new Size(40, 40);
            GiveVIP_Role.Location = new Point(400, 10);
            GiveVIP_Role.MouseEnter += GiveVIP_Role_MouseEnter;
            GiveVIP_Role.MouseLeave += GiveVIP_Role_MouseLeave;
            GiveVIP_Role.MouseDown += GiveVIP_Role_MouseDown;
            GiveVIP_Role.MouseUp += GiveVIP_Role_MouseUp;
            GiveVIP_Role.MouseClick += GiveVIP_Role_MouseClick;
            iToolTip1.SetToolTip(GiveVIP_Role, "VIP Rolle");
            #endregion

            #region GiveSongSelector_Role
            Design.iPictureBox GiveSongSelector_Role = new Design.iPictureBox();
            GiveSongSelector_Role.Name = $"GiveSongSelector_{i}";
            GiveSongSelector_Role.Cursor = Cursors.Hand;
            if (Role != "SongSelector")
                GiveSongSelector_Role.Image = Resources_Images.Images.Icons.SongSelector;
            else if (Role == "SongSelector")
                GiveSongSelector_Role.Image = Resources_Images.Images.Icons.No_SongSelector;
            GiveSongSelector_Role.ImageSize = new Size(20, 20);
            GiveSongSelector_Role.SizeMode = PictureBoxSizeMode.CenterImage;
            GiveSongSelector_Role.Size = new Size(40, 40);
            GiveSongSelector_Role.Location = new Point(GiveVIP_Role.Location.X + GiveVIP_Role.Width + 10, 10);
            GiveSongSelector_Role.MouseEnter += GiveSongSelector_Role_MouseEnter;
            GiveSongSelector_Role.MouseLeave += GiveSongSelector_Role_MouseLeave;
            GiveSongSelector_Role.MouseDown += GiveSongSelector_Role_MouseDown;
            GiveSongSelector_Role.MouseUp += GiveSongSelector_Role_MouseUp;
            GiveSongSelector_Role.MouseClick += GiveSongSelector_Role_MouseClick;
            iToolTip1.SetToolTip(GiveSongSelector_Role, "Songauswähler Rolle");
            #endregion

            #region KickMember
            Design.iPictureBox KickMember = new Design.iPictureBox();
            KickMember.Name = $"KickMember_{i}";
            KickMember.Cursor = Cursors.Hand;
            KickMember.Image = Resources_Images.Images.Icons.Leave_Logout;
            KickMember.ImageSize = new Size(20, 20);
            KickMember.SizeMode = PictureBoxSizeMode.CenterImage;
            KickMember.Size = new Size(40, 40);
            KickMember.Location = new Point(GiveSongSelector_Role.Location.X + GiveSongSelector_Role.Width + 10, 10);
            KickMember.MouseEnter += KickMember_MouseEnter;
            KickMember.MouseLeave += KickMember_MouseLeave;
            KickMember.MouseDown += KickMember_MouseDown;
            KickMember.MouseUp += KickMember_MouseUp;
            KickMember.MouseClick += KickMember_MouseClick;
            iToolTip1.SetToolTip(KickMember, "Kicken");
            #endregion

            #region BanMember
            Design.iPictureBox BanMember = new Design.iPictureBox();
            BanMember.Name = $"BanMember_{i}";
            BanMember.Cursor = Cursors.Hand;
            BanMember.Image = Resources_Images.Images.Icons.unavailable;
            BanMember.ImageSize = new Size(25, 25);
            BanMember.SizeMode = PictureBoxSizeMode.CenterImage;
            BanMember.Size = new Size(40, 40);
            BanMember.Location = new Point(KickMember.Location.X + KickMember.Width + 10, 10);
            BanMember.MouseEnter += BanMember_MouseEnter;
            BanMember.MouseLeave += BanMember_MouseLeave;
            BanMember.MouseUp += BanMember_MouseUp;
            BanMember.MouseDown += BanMember_MouseDown;
            BanMember.MouseClick += BanMember_MouseClick;
            iToolTip1.SetToolTip(BanMember, "Bannen");
            #endregion

            #region KickMemberButton
            /*Design.iButton2 KickMember = new Design.iButton2();
            KickMember.Cursor = Cursors.Hand;

            KickMember.PressAnimimation = true;
            KickMember.BackColor = Color.Transparent;
            KickMember.HoverColor = Color.FromArgb(70, Color.Black);
            KickMember.HoverColorEnabled = true;
            KickMember.Image = Resources_Images.Images.Icons.Leave_Logout;
            KickMember.ImageSize = new Size(25, 25);
            KickMember.ImageAlign = Design.iButton2.ImageAlignEnum.MiddleCenter;
            KickMember.Size = new Size(35, 35);
            KickMember.Location = new Point(400, 10);
            iToolTip1.SetToolTip(KickMember, "Aus dem Raum schmeißen");*/
            #endregion

            #region BanButton
            /*Design.iButton2 BanMember = new Design.iButton2();
            BanMember.Cursor = Cursors.Hand;
            BanMember.PressAnimimation = false;
            BanMember.BackColor = Color.Transparent;
            BanMember.HoverColor = Color.Transparent;
            BanMember.HoverColorEnabled = false;
            BanMember.Image = Resources_Images.Images.Icons.unavailable;
            BanMember.ImageSize = new Size(25, 25);
            BanMember.ImageAlign = Design.iButton2.ImageAlignEnum.MiddleCenter;
            BanMember.Size = new Size(35, 35);
            BanMember.Location = new Point(KickMember.Location.X + KickMember.Width + 10, 10);
            iToolTip1.SetToolTip(BanMember, "Bannen");*/
            #endregion

            #endregion

            kickMemberPictureBoxes.Add(KickMember);
            banMemberPictureBoxes.Add(BanMember);
            VIPMemberPictureBoxes.Add(GiveVIP_Role);
            SongSelectorMemberPictureBoxes.Add(GiveSongSelector_Role);

            #region Add To Panel
            iPanel.Controls.Add(iRoundedPictureBox);
            iPanel.Controls.Add(Username_iLabel);

            if (Username == myUsername)
            {
                iPanel.Controls.Add(You_iLabel);
            }
            else if (Role == "Host")
            {
                You_iLabel.Text = "Host";
                iPanel.Controls.Add(You_iLabel);
            }

            if (Global.PartyModeRole == "Host" && Role != "Host")
            {
                iPanel.Controls.Add(KickMember);
                iPanel.Controls.Add(BanMember);
                iPanel.Controls.Add(GiveVIP_Role);
                iPanel.Controls.Add(GiveSongSelector_Role);
            }
            else if (Global.PartyModeRole == "VIP" && (Role != "Host" && Role != "VIP"))
            {
                iPanel.Controls.Add(KickMember);
                iPanel.Controls.Add(BanMember);
                iPanel.Controls.Add(GiveSongSelector_Role);
            }

            roomMemberPanels.Add((iPanel, GetRoleEnumID(Role)));
            //roomMemberPanels[i].Item1.Show();
            //iPanel22.Controls.Add(roomMemberPanels[i].Item1);
            #endregion

            this.ResumeLayout();

        }

        #region VIPRole_Events
        private async void GiveVIP_Role_MouseClick(object sender, MouseEventArgs e)
        {
            string TargetClient_Role = Users.ElementAt(old_VIPMemberIndex).Value.Role;
            string TargetClient_Email = Users.ElementAt(old_VIPMemberIndex).Value.Email;
            string TargetClient_Username = $"{Users.ElementAt(old_VIPMemberIndex).Value.Username}#{Users.ElementAt(old_VIPMemberIndex).Value.UsernameID}";


            if (TargetClient_Role == "VIP")
                await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID).Collection("members").Document("members").SetAsync(new Dictionary<string, object>() { { TargetClient_Email, "Listener" } }, SetOptions.MergeAll);
            else
                await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID).Collection("members").Document("members").SetAsync(new Dictionary<string, object>() { { TargetClient_Email, "VIP" } }, SetOptions.MergeAll);

            await Logs($"{myUsername} hat die Rolle von {TargetClient_Username} zu {(TargetClient_Role == "VIP" ? "Listener" : "VIP")} geändert");
        }

        private void GiveVIP_Role_MouseUp(object sender, MouseEventArgs e)
        {
            VIPMemberPictureBoxes[old_VIPMemberIndex].BackColor = Color.FromArgb(100, Color.Black);

        }

        private void GiveVIP_Role_MouseDown(object sender, MouseEventArgs e)
        {
            VIPMemberPictureBoxes[old_VIPMemberIndex].BackColor = Color.FromArgb(150, Color.Black);
        }

        private void GiveVIP_Role_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                VIPMemberPictureBoxes[old_VIPMemberIndex].BackColor = Color.FromArgb(0, Color.Black);
            }
            catch { }
        }

        private void GiveVIP_Role_MouseEnter(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(GetControl("GiveVIPRole_").Split('_')[1]);
            VIPMemberPictureBoxes[index].BackColor = Color.FromArgb(100, Color.Black);
            old_VIPMemberIndex = index;
        }
        #endregion

        #region GiveSongSelector_Events
        private async void GiveSongSelector_Role_MouseClick(object sender, MouseEventArgs e)
        {
            //Console.WriteLine(old_SongSelectorMemberIndex);
            string TargetClient_Role = Users.ElementAt(old_SongSelectorMemberIndex).Value.Role;
            string TargetClient_Email = Users.ElementAt(old_SongSelectorMemberIndex).Value.Email;
            string TargetClient_Username = $"{Users.ElementAt(old_SongSelectorMemberIndex).Value.Username}#{Users.ElementAt(old_SongSelectorMemberIndex).Value.UsernameID}";

            if (TargetClient_Role == "SongSelector")
                await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID).Collection("members").Document("members").SetAsync(new Dictionary<string, object>() { { TargetClient_Email, "Listener" } }, SetOptions.MergeAll);
            else
                await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID).Collection("members").Document("members").SetAsync(new Dictionary<string, object>() { { TargetClient_Email, "SongSelector" } }, SetOptions.MergeAll);

            await Logs($"{myUsername} hat die Rolle von {TargetClient_Username} zu {(TargetClient_Role == "SongSelector" ? "Listener" : "SongSelector")} geändert");

        }

        private void GiveSongSelector_Role_MouseUp(object sender, MouseEventArgs e)
        {
            SongSelectorMemberPictureBoxes[old_SongSelectorMemberIndex].BackColor = Color.FromArgb(100, Color.Black);
        }

        private void GiveSongSelector_Role_MouseDown(object sender, MouseEventArgs e)
        {
            SongSelectorMemberPictureBoxes[old_SongSelectorMemberIndex].BackColor = Color.FromArgb(150, Color.Black);
        }

        private void GiveSongSelector_Role_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                SongSelectorMemberPictureBoxes[old_SongSelectorMemberIndex].BackColor = Color.FromArgb(0, Color.Black);
            }
            catch { }
        }

        private void GiveSongSelector_Role_MouseEnter(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(GetControl("GiveSongSelector_").Split('_')[1]);
            SongSelectorMemberPictureBoxes[index].BackColor = Color.FromArgb(100, Color.Black);
            old_SongSelectorMemberIndex = index;
        }
        #endregion

        #region GetControl
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point pnt);
        public static string GetControl(string ContainsText)
        {
            try
            {

                IntPtr hWnd = WindowFromPoint(Control.MousePosition);
                if (hWnd != IntPtr.Zero)
                {
                    Control ctl = Control.FromHandle(hWnd);
                    if (ctl != null && ctl.Name.Contains(ContainsText) && ctl.Visible == true)
                    {
                        return ctl.Name;
                    }
                }
            }
            catch { }
            return "-1";

        }
        #endregion

        #region BanMember Events
        private async void BanMember_MouseClick(object sender, MouseEventArgs e)
        {
            var TargetEmail = Users.FirstOrDefault(x => x.Value.index == old_KickMemberIndex).Key;
            string TargetUsername = $"{Users[TargetEmail].Username}#{Users[TargetEmail].UsernameID}";
            string myUsername = $"{Global.Authentication.Username}#{Global.Authentication.UsernameID}";
            await Logs($"{myUsername} hat {TargetUsername} gebannt");
            BanMember(TargetEmail, TargetUsername);
        }

        private void BanMember_MouseDown(object sender, MouseEventArgs e)
        {
            banMemberPictureBoxes[old_BanMemberIndex].BackColor = Color.FromArgb(150, Color.Black);
        }

        private void BanMember_MouseUp(object sender, MouseEventArgs e)
        {
            banMemberPictureBoxes[old_BanMemberIndex].BackColor = Color.FromArgb(100, Color.Black);
        }

        private void BanMember_MouseLeave(object sender, EventArgs e)
        {
            banMemberPictureBoxes[old_BanMemberIndex].BackColor = Color.FromArgb(0, Color.Black);
        }

        private void BanMember_MouseEnter(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(GetControl("BanMember_").Split('_')[1]);
            banMemberPictureBoxes[index].BackColor = Color.FromArgb(100, Color.Black);
            old_BanMemberIndex = index;
        }
        #endregion

        #region KickMember Events
        private async void KickMember_MouseClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("KickedMember");
            var TargetEmail = Users.FirstOrDefault(x => x.Value.index == old_KickMemberIndex).Key;
            string TargetUsername = $"{Users[TargetEmail].Username}#{Users[TargetEmail].UsernameID}";
            string myUsername = $"{Global.Authentication.Username}#{Global.Authentication.UsernameID}";
            await Logs($"{myUsername} hat {TargetUsername} gekickt");
            KickMember(TargetEmail);
        }
        private void KickMember_MouseUp(object sender, MouseEventArgs e)
        {
            kickMemberPictureBoxes[old_KickMemberIndex].BackColor = Color.FromArgb(100, Color.Black);
        }

        private void KickMember_MouseDown(object sender, MouseEventArgs e)
        {
            kickMemberPictureBoxes[old_KickMemberIndex].BackColor = Color.FromArgb(150, Color.Black);
        }

        private void KickMember_MouseEnter(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(GetControl("KickMember_").Split('_')[1]);
            kickMemberPictureBoxes[index].BackColor = Color.FromArgb(100, Color.Black);
            old_KickMemberIndex = index;
        }

        private void KickMember_MouseLeave(object sender, EventArgs e)
        {
            kickMemberPictureBoxes[old_KickMemberIndex].BackColor = Color.FromArgb(0, Color.Black);
        }

        #endregion

        private void iButton24_Click(object sender, EventArgs e)
        {
            //Settings
            PartyMode_Settings partyMode_Settings = new PartyMode_Settings(mySessionID);
            partyMode_Settings.Show($"{RoomName} - Einstellungen");
        }

        private void iButton23_Click(object sender, EventArgs e)
        {
            partyMode_Queue.SongPanel_0.AlbumLabel.Text = "Kok";

        }



        private void iPanel21_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }
        private void iPanel21_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point((this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);
                this.Update();
            }
        }
        private void iPanel21_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void iButton25_Click(object sender, EventArgs e)
        {
            partyMode_Queue.SongPanel_0.AlbumLabel.Text = "Kok";
        }

        private void iPanel26_MouseEnter(object sender, EventArgs e)
        {
            iPanel26.BackColor = Color.FromArgb(100, Color.Black);
            iLabel3.BackColor = Color.Transparent;
            iLabel3.AlphaColor = 255;
        }

        private void iPanel26_MouseLeave(object sender, EventArgs e)
        {
            iPanel26.BackColor = Color.FromArgb(0, Color.Black);
            iLabel3.AlphaColor = 200;
        }

        private void iLabel4_MouseEnter(object sender, EventArgs e)
        {
            iLabel4.AlphaColor = 255;
        }

        private void iLabel4_MouseLeave(object sender, EventArgs e)
        {
            iLabel4.AlphaColor = 200;
        }

        private void iLabel4_Click(object sender, EventArgs e)
        {
            Animations animations = new Animations();
            animations.CopiedAnimation(iLabel4);
            Clipboard.SetText(RoomKey);
        }

        private void iButton21_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void iButton22_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private async void PartyMode_Room_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                await LeaveRoom();
            }
        }
        private async void PartyMode_Room_FormClosing(object sender, FormClosingEventArgs e)
        {
            await LeaveRoom();
        }

        private void iButton25_Click_1(object sender, EventArgs e)
        {
            partyMode_Queue.Show();
        } 
    }
}
