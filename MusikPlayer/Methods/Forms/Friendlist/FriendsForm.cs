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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods.Forms.Friendlist
{
    public partial class FriendsForm : Form
    {
        private bool mouseDown;
        private Point lastLocation;

        public FirestoreChangeListener firestoreMessagesCountListener;
        public FirestoreChangeListener firestorefriendListShow_listener;
        public FirestoreChangeListener firestoreMessagesListener;
        public FirestoreChangeListener firestoreMyStatusListener;
        public int friendsOnline = 0;

        public List<object> friend_Emails = new List<object>();
        public List<Structures.Client_Data> friend_Data = new List<Structures.Client_Data>();


        public Design.iContextStripMenu iContextStripMenu;
        public Design.iContextStripMenu iContextStripMenu_Status;


        Design.iLabel[] buttonLabels = new Design.iLabel[1];

        Design.iButton2 CopyButton = new Design.iButton2();
        Design.iButton2 SendRequest_Button = new Design.iButton2();
        Design.iPanel backgroundPanel = new Design.iPanel();
        Design.iTextBox searchTextbox = new Design.iTextBox();

        List<Structures.Client_Messages> List_client_Messages = new List<Structures.Client_Messages>();
        public static List<Panel> requestPanels = new List<Panel>();

        public FriendsForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ResizeRedraw = false;

            iLabel1.Text = $"{Global.Authentication.Username}#{Global.Authentication.UsernameID}";

            pictureBox2.Size = new Size(10, 10);
            pictureBox2.Location = new Point(StatusLabel.Location.X + StatusLabel.Width + 3, StatusLabel.Location.Y + (StatusLabel.Height - pictureBox2.Height) / 2);
            pictureBox2.BackColor = Color.Transparent;

            panel4.Parent = gradientPanel1;
            buttonLabels = new Design.iLabel[] { yourFriendsLabel, addFriendsLabel, messagesLabel, blockedAccountsLabel };

            iScrollBarV1.MovingState = false;
            panel5.MouseWheel += Panel5_MouseWheel;
        }

        private void FriendsForm_Load(object sender, EventArgs e)
        {
            GetStatus();
            OpenFriendList();
            AdjustControls();
            ContextStripSettings();
            ContextStripStatus();
            ListenMessagesCount();
            WireMouseEvents(iPanel1);
            Client_DataHandling.GetProfileImage(iRoundedPictureBox2);
        }

        async void GetStatus()
        {
            DocumentSnapshot documentSnapshot = await FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).GetSnapshotAsync();
            Dictionary<string, object> document_Dict = documentSnapshot.ToDictionary();
            this.Invoke((MethodInvoker)delegate
            {
                string Status = document_Dict["Status"].ToString();
                StatusImage(Status);
                StatusLabel.Text = Status;
                if (Status == "Busy")
                    StatusLabel.Text = "Beschäftigt";
                else if (Status == "Busy")
                    StatusLabel.Text = "Beschäftigt";
                else if (Status == "Appear Offline")
                    StatusLabel.Text = "Offline anzeigen";
                pictureBox2.Location = new Point(StatusLabel.Location.X + StatusLabel.Width + 3, StatusLabel.Location.Y + (StatusLabel.Height - pictureBox2.Height) / 2);
            });
        }
        void StatusImage(string Status)
        {
            if (Status == "Online")
                iRoundedPictureBox1.Image = Resources_Images.Images.Icons.Online;
            else if (Status == "AFK")
                iRoundedPictureBox1.Image = Resources_Images.Images.Icons.AFK;
            else if (Status == "Busy")
                iRoundedPictureBox1.Image = Resources_Images.Images.Icons.busy;
            else if (Status.Contains("Offline"))
                iRoundedPictureBox1.Image = Resources_Images.Images.Icons.offline;
        }

        Point PointToThis(Control c, Point p)
        {
            return PointToClient(c.PointToScreen(p));
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
                    iPanel1_MouseClick(s, e);
                };
                c.MouseEnter += (s, e) =>
                {
                    iPanel1_MouseEnter(s, e);
                };
                c.MouseLeave += (s, e) =>
                {
                    // iPanel1_MouseLeave(s, e);
                };
            };
        }


        public void ListenMessagesCount()
        {
            CollectionReference collRef = FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).Collection("data").Document("messages").Collection("requests");
            firestoreMessagesCountListener = collRef.Listen(async snapshot =>
            {
                QuerySnapshot querySnapshot = await collRef.GetSnapshotAsync();
                int messagesCount = querySnapshot.Count;
                this.Invoke((MethodInvoker)delegate
                {
                    if (Convert.ToInt32(messagesCount) > 0)
                    {
                        iLabel3.Visible = true;
                        iLabel3.Text = messagesCount.ToString();
                    }
                    else
                    {
                        iLabel3.Visible = false;
                    }
                    if (messagesCount > 99)
                        iLabel3.Text = ">99";
                });

            });
            //await firestoreListener.StopAsync();
        }


        private void AdjustControls()
        {

            int Y = (panel4.Height - yourFriendsLabel.Height) / 2;
            int Y_v2 = (panel4.Height - Y) / 6;

            yourFriendsButton.Location = new Point(yourFriendsButton.Location.X, Y_v2);
            addFriendsButton.Location = new Point(addFriendsButton.Location.X, yourFriendsButton.Location.Y + 60);
            messagesButton.Location = new Point(messagesButton.Location.X, yourFriendsButton.Location.Y + 120);
            blockedAccountsButton.Location = new Point(blockedAccountsButton.Location.X, yourFriendsButton.Location.Y + 180);
            hLine1.Location = new Point(hLine1.Location.X, blockedAccountsButton.Location.Y + blockedAccountsButton.Height + 10);
        }
        private void iButton23_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void iButton21_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        void MinimizeMaximize()
        {
            Rectangle rect = Screen.FromHandle(this.Handle).WorkingArea;
            rect.Location = new Point(0, 0);
            this.MaximizedBounds = rect;
            if (this.WindowState != FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
                iButton2.Image = Global.resizeImage(Resources_Images.Images.Icons.Minimize, new Size(20, 20));
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = new Size(1045, 547);
                iButton2.Image = Global.resizeImage(Resources_Images.Images.Icons.Maximize, new Size(20, 20));
            }
            AdjustControls();
        }



        private void iButton22_Click(object sender, EventArgs e)
        {
            MinimizeMaximize();
        }



        List<Design.iPanel> friendForms = new List<Design.iPanel>();

        Image ImageStream(string ImageURL)
        {
            using (Stream imageStream = File.OpenRead(ImageURL))
            {
                return Image.FromStream(imageStream);
            }
        }

        #region GetFriends_Design
        void Create_FriendControls(int i, string Username, string Status)
        {
            this.Invoke((MethodInvoker)delegate
            {
                #region Create Panel
                Design.iPanel iPanel = new Design.iPanel();
                iPanel.Dock = DockStyle.Top;
                //iPanel.BorderStyle = BorderStyle.FixedSingle;
                iPanel.MouseClick += FriendsForm_MouseClick;
                iPanel.Size = new Size(panel5.Width, 80);
                #endregion

                #region Create Labels
                Design.iLabel Username_iLabel = new Design.iLabel();
                Username_iLabel.AutoSize = true;
                Username_iLabel.Text = Username;
                Username_iLabel.Font = new Font("Microsoft Sans Serif", 11.25f, FontStyle.Regular);
                Username_iLabel.ForeColor = Color.White;
                Username_iLabel.HoverAnimation = false;
                Username_iLabel.Location = new Point(80, 20);

                Design.iLabel Status_iLabel = new Design.iLabel();
                Status_iLabel.Text = Status;
                Status_iLabel.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
                Status_iLabel.ForeColor = Color.White;
                Status_iLabel.HoverAnimation = false;
                Status_iLabel.Location = new Point(80, 45);
                #endregion

                #region Create PictureBox

                //PictureBox pictureBox = new PictureBox();
                Design.iRoundedPictureBox iRoundedPictureBox = new Design.iRoundedPictureBox();
                iRoundedPictureBox.Location = new Point(10, 10);
                iRoundedPictureBox.Size = new Size(50, 50);
                iRoundedPictureBox.Image = Resources_Images.Images.Icons.profileImage_Placeholder; //ImageStream(@"C:\Users\HlDE1\Pictures\86bd63fbf50b50f5ce688dcf8260373d.png");
                iRoundedPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                #endregion

                #region Create Buttons
                Design.iButton2 PartyInvite_Button = new Design.iButton2();
                PartyInvite_Button.Text = "Zur Party einladen";
                PartyInvite_Button.Location = new Point(400, 20);
                PartyInvite_Button.BorderRadius = 3;
                PartyInvite_Button.HoverColor = Color.MediumSeaGreen;
                PartyInvite_Button.BackColor = Color.MediumSeaGreen;

                #endregion

                #region Add To Panel
                iPanel.Controls.Add(iRoundedPictureBox);
                iPanel.Controls.Add(Username_iLabel);
                iPanel.Controls.Add(Status_iLabel);
                iPanel.Controls.Add(PartyInvite_Button);

                friendForms.Add(iPanel);
                friendForms[i].Show();
                panel5.Controls.Add(friendForms[i]);
                #endregion
            });

        }

        void Show_MyUsername_Controls()
        {
            #region Create Panel
            backgroundPanel = new Design.iPanel();

            backgroundPanel.Dock = DockStyle.Top;
            backgroundPanel.Size = panel5.Size;
            backgroundPanel.BackColor = Color.FromArgb(100, Color.Black);

            Design.iPanel Username_Panel = new Design.iPanel();
            Username_Panel.BackColor = Color.FromArgb(30, Color.Gray);
            Username_Panel.Location = new Point(20, 60);
            Username_Panel.Size = new Size(panel5.Width - Username_Panel.Location.X - 20, 60);

            #endregion

            #region Create Labels
            Design.iLabel yourUsername_iLabel = new Design.iLabel();
            yourUsername_iLabel.AutoSize = true;
            yourUsername_iLabel.Text = $"Dein Benutzername";
            yourUsername_iLabel.Font = new Font("Microsoft Sans Serif", 15f, FontStyle.Regular);
            yourUsername_iLabel.ForeColor = Color.White;
            yourUsername_iLabel.HoverAnimation = false;
            yourUsername_iLabel.Location = new Point(Username_Panel.Location.X, Username_Panel.Location.Y - 30);
            yourUsername_iLabel.BackColor = Color.Transparent;

            Design.iLabel Username_iLabel = new Design.iLabel();
            Username_iLabel.AutoSize = true;
            Username_iLabel.Text = $"{Global.Authentication.Username}#{Global.Authentication.UsernameID}";
            Username_iLabel.Font = new Font("Microsoft Sans Serif", 15f, FontStyle.Regular);
            Username_iLabel.ForeColor = Color.White;
            Username_iLabel.HoverAnimation = false;
            Username_iLabel.Location = new Point(10, (Username_Panel.Height - Username_iLabel.Height) / 2);
            Username_iLabel.BackColor = Color.Transparent;
            #endregion

            #region Create Button
            CopyButton.Text = "Kopieren";
            CopyButton.Location = new Point(Username_Panel.Width - CopyButton.Width - 30, (Username_Panel.Height - CopyButton.Height) / 2);

            CopyButton.BackgroundColor = Color.CornflowerBlue;
            CopyButton.HoverColor = Color.CornflowerBlue;

            CopyButton.MouseClick += CopyButton_MouseClick;
            CopyButton.BorderRadius = 2;
            #endregion

            #region Add Controls
            Username_Panel.Controls.Add(Username_iLabel);
            Username_Panel.Controls.Add(CopyButton);

            backgroundPanel.Controls.Add(yourUsername_iLabel);
            backgroundPanel.Controls.Add(Username_Panel);
            panel5.Controls.Add(backgroundPanel);
            #endregion
        }

        void Show_AddFriend_Controls()
        {
            #region Panel
            Design.iPanel iPanel = new Design.iPanel();
            iPanel.BackColor = Color.FromArgb(30, Color.Gray);
            iPanel.Location = new Point(20, 200);
            iPanel.Size = new Size(panel5.Width - iPanel.Location.X - 20, 60);
            #endregion

            #region Label
            Design.iLabel Title_iLabel = new Design.iLabel();
            Title_iLabel.AutoSize = true;
            Title_iLabel.BackColor = Color.Transparent;
            Title_iLabel.Text = $"Freundschaftsanfrage verschicken";
            Title_iLabel.Font = new Font("Microsoft Sans Serif", 15f, FontStyle.Regular);
            Title_iLabel.ForeColor = Color.White;
            Title_iLabel.HoverAnimation = false;
            Title_iLabel.Location = new Point(iPanel.Location.X, iPanel.Location.Y - 30);
            #endregion

            #region TextBox
            searchTextbox = new Design.iTextBox();
            searchTextbox.RemovePlaceholder();
            // iTextBox.BackColor = iPanel.BackColor; //Color.FromArgb(100, Color.Black);
            searchTextbox.PlaceholderText = "Benutzername eingeben";
            searchTextbox.Location = new Point(10, (iPanel.Height - searchTextbox.Height) / 2);
            searchTextbox.Size = new Size(350, 35);
            searchTextbox.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
            searchTextbox.ForeColor = Color.Black;
            searchTextbox.UnderlinedStyle = true;
            #endregion

            #region Create Button

            SendRequest_Button.Text = "Anfrage senden";
            SendRequest_Button.Location = new Point(iPanel.Width - SendRequest_Button.Width - 30, (iPanel.Height - SendRequest_Button.Height) / 2);

            SendRequest_Button.BackgroundColor = Color.CornflowerBlue;
            SendRequest_Button.HoverColor = Color.CornflowerBlue;

            SendRequest_Button.MouseClick += SendRequest_Button_MouseClick;
            SendRequest_Button.BorderRadius = 2;
            #endregion

            iPanel.Controls.Add(searchTextbox);
            iPanel.Controls.Add(SendRequest_Button);

            backgroundPanel.Controls.Add(iPanel);
            backgroundPanel.Controls.Add(Title_iLabel);
            panel5.Controls.Add(backgroundPanel);
        }
        #endregion
        private void SendRequest_Button_MouseClick(object sender, MouseEventArgs e)
        {
            //Console.WriteLine(searchTextbox.Texts);
            if (!searchTextbox.Texts.Contains("#"))
            {
                Global.iMessageBox.Show("Ungültige Eingabe", "Fehler");
                return;
            }
            if (searchTextbox.Texts == $"{Global.Authentication.Username}#{Global.Authentication.UsernameID}")
            {
                Global.iMessageBox.Show("Du kannst dir keine Freundschaftsanfrage schicken", "Fehler");

                return;
            }
            Send_FriendRequest();
        }
        private void CopyButton_MouseClick(object sender, MouseEventArgs e)
        {
            Clipboard.SetText($"{Global.Authentication.Username}#{Global.Authentication.UsernameID}");
            CopyButton_Func();
        }

        #region ButtonAnimation
        Thread copyButton_Thread;

        private void CopyButton_Func()
        {
            CopyButton.Text = "Kopiert";
            copyButton_Thread = new Thread(() =>
            {
                Thread.Sleep(3000);
                this.Invoke((MethodInvoker)delegate
                {
                    CopyButton.Text = "Kopieren";
                });
            });
            copyButton_Thread.Start();
        }

        Thread friendRequetButton_Thread;
        private void Send_FriendRequestText_Thread()
        {
            SendRequest_Button.Text = "Anfrage versendet";
            friendRequetButton_Thread = new Thread(() =>
            {
                Thread.Sleep(3000);
                this.Invoke((MethodInvoker)delegate
                {
                    SendRequest_Button.Text = "Anfrage senden";
                });
            });
            friendRequetButton_Thread.Start();
        }

        #endregion

        #region Send_FriendRequest
        async Task<bool> isBlocked(DocumentSnapshot Emails_Snapshot, string myEmail)
        {
            DocumentReference UsersBlock_Documents = FirestoreGlobal.firestoreDb.Collection("Users").Document(Emails_Snapshot.Id).Collection("data").Document("blockList");
            DocumentSnapshot blocks_Snapshot = await UsersBlock_Documents.GetSnapshotAsync();
            Dictionary<string, object> blockList_Dict = blocks_Snapshot.ToDictionary();

            if (blockList_Dict.ContainsKey(myEmail))
                return true;

            return false;
        }

        private async void Send_Message(DocumentSnapshot Emails_Snapshot, string myEmail, string Username, string UsernameID)
        {
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                {"Type", "Friend_Request"},
                {"Username", Username},
                {"UsernameID", UsernameID},
            };
            await FirestoreGlobal.firestoreDb.Collection("Users").Document(Emails_Snapshot.Id).Collection("data").Document("messages").Collection("requests").Document(myEmail).SetAsync(data);
        }

        private async void Send_FriendRequest()
        {
            CollectionReference UsersEmail_Collection = FirestoreGlobal.firestoreDb.Collection("Users");
            QuerySnapshot UsersEmail_Snapshot = await UsersEmail_Collection.GetSnapshotAsync();

            foreach (DocumentSnapshot Emails_Snapshot in UsersEmail_Snapshot.Documents)
            {

                if (Emails_Snapshot.Id != Global.Authentication.Email)
                {
                    DocumentReference UsersEmail_Document = FirestoreGlobal.firestoreDb.Collection("Users").Document(Emails_Snapshot.Id).Collection("data").Document("credentials");
                    DocumentSnapshot credentials_Snapshot = await UsersEmail_Document.GetSnapshotAsync();
                    Dictionary<string, object> credentials_Dict = credentials_Snapshot.ToDictionary();
                    string Username = $"{credentials_Dict["Username"]}#{credentials_Dict["UsernameID"]}";

                    if (searchTextbox.Texts == Username)
                    {
                        if (await isBlocked(Emails_Snapshot, Global.Authentication.Email) == true)
                        {
                            Global.iMessageBox.Show($"Du wurdest vom Benutzer geblockt", "Fehler");
                            return;
                        }
                        Send_Message(Emails_Snapshot, Global.Authentication.Email, Global.Authentication.Username, Global.Authentication.UsernameID);
                        Global.iMessageBox.Show("Die Freundschaftsanfrage wurde versendet", "Versendet");
                        Send_FriendRequestText_Thread();
                        return;
                    }
                }
            }
            Global.iMessageBox.Show("Der Benutzer existiert nicht", "Existiert nicht");

        }

        #endregion


        public async Task Get_FriendInfo_Firestore()
        {
            try
            {
                #region Get FriendEmails
                DocumentReference listener_doc = FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email);
                DocumentReference collRef = FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).Collection("data").Document("friendList");
                DocumentSnapshot documentSnapshot = await collRef.GetSnapshotAsync();
                Dictionary<string, object> dict_friends = documentSnapshot.ToDictionary();

                foreach (KeyValuePair<string, object> valuePair in dict_friends)
                {
                    friend_Emails.Add(valuePair.Key);
                }
                #endregion

                int index = 0;
                var form1 = Application.OpenForms.OfType<Form1>().Single();

                Query friend_listener = FirestoreGlobal.firestoreDb.Collection("Users").WhereIn(FieldPath.DocumentId, friend_Emails);

                firestorefriendListShow_listener = friend_listener.Listen(async snapshot =>
                {
                    /*foreach (DocumentChange change in snapshot.Changes)
                    {
                        if (change.ChangeType.ToString() == "Modified")
                        {
                            Console.WriteLine("Modified: {0}", change.Document.Id);
                        }
                    }*/

                    index = 0;
                    friendsOnline = 0;
                    form1.Invoke((MethodInvoker)delegate
                    {
                        panel5.Controls.Clear();
                        friendForms.Clear();
                        friend_Emails.Clear();
                        friend_Data.Clear();
                    });

                    foreach (KeyValuePair<string, object> valuePair in dict_friends)
                    {
                        friend_Emails.Add(valuePair.Key);

                        DocumentReference credentials_Document = FirestoreGlobal.firestoreDb.Collection("Users").Document(valuePair.Key).Collection("data").Document("credentials");
                        DocumentSnapshot credentials_Snapshot = await credentials_Document.GetSnapshotAsync();
                        Dictionary<string, object> credentials_Dict = credentials_Snapshot.ToDictionary();

                        if (valuePair.Key != Global.Authentication.Email)
                        {
                            DocumentReference Info_Document = FirestoreGlobal.firestoreDb.Collection("Users").Document(valuePair.Key);
                            DocumentSnapshot Info_Snapshot = await Info_Document.GetSnapshotAsync();
                            Dictionary<string, object> Info_Dict = Info_Snapshot.ToDictionary();

                            string Username = $"{credentials_Dict["Username"]}#{credentials_Dict["UsernameID"]}";
                            string Status = Info_Dict["Status"].ToString();
                            string CurrentSong = Info_Dict["CurrentSong"].ToString();
                            string FolderID = Info_Dict["folderID"].ToString();

                            if (Status != "Offline")
                                friendsOnline++;

                            friend_Data.Add(new Structures.Client_Data { Username = Username, CurrentSong = CurrentSong, Status = Status, FolderID = FolderID });
                            // Create_FriendControls(index, Username, Status);
                            index++;
                        }
                    }

                    if (this.Visible)
                        ShowFriends();
                    Show_FriendOnlineCounter();
                });
            }
            catch
            {

            }
        }

        void ShowFriends()
        {
            for (int i = 0; i < friend_Data.Count; i++)
            {
                Create_FriendControls(i, friend_Data[i].Username, friend_Data[i].Status);
            }
        }

        void Show_FriendOnlineCounter()
        {
            var form1 = Application.OpenForms.OfType<Form1>().Single();
            form1.Invoke((MethodInvoker)delegate
            {
                form1.iLabel4.Text = friendsOnline.ToString();
            });
        }

        void MarkLabel(Design.iLabel label)
        {
            iLabel7.Update();
            panel5.Controls.Clear();
            for (int i = 0; i < buttonLabels.Length; i++)
            {
                buttonLabels[i].AlphaColor = 150;
                buttonLabels[i].HoverAnimation = true;
            }
            label.AlphaColor = 255;
            label.HoverAnimation = false;
            this.Text = label.Text;
        }



        #region GetControl
        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point pnt);
        public static string GetControl(string contrainText)
        {
            try
            {

                IntPtr hWnd = WindowFromPoint(Control.MousePosition);
                if (hWnd != IntPtr.Zero)
                {
                    Control ctl = Control.FromHandle(hWnd);
                    if (ctl != null && ctl.Name.Contains(contrainText) && ctl.Visible == true)
                    {
                        return ctl.Name;
                    }
                }
            }
            catch { }
            return "-1";

        }
        #endregion

        #region Get_Messages

        Panel GetAllMessages_Design(string Text, int Index, string ImageURL, EventHandler blockClick = null, EventHandler declineClick = null, EventHandler acceptClick = null)
        {
            #region Panel
            Design.iPanel iPanel = new Design.iPanel();
            iPanel.Dock = DockStyle.Top;
            iPanel.BorderStyle = BorderStyle.FixedSingle;
            iPanel.MouseClick += FriendsForm_MouseClick;
            iPanel.Size = new Size(panel5.Width, 80);
            #endregion

            #region PictureBox
            Design.iRoundedPictureBox pictureBox = new Design.iRoundedPictureBox();
            pictureBox.Location = new Point(10, 10);
            pictureBox.Size = new Size(60, 60);
            if (!string.IsNullOrEmpty(ImageURL))
                pictureBox.ImageLocation = ImageURL;
            else
                pictureBox.Image = Resources_Images.Images.Icons.profileImage_Placeholder;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            #endregion

            #region Label
            Design.iLabel Request_iLabel = new Design.iLabel();
            Request_iLabel.AutoSize = true;
            Request_iLabel.Text = Text;
            Request_iLabel.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
            Request_iLabel.ForeColor = Color.White;
            Request_iLabel.HoverAnimation = false;
            Request_iLabel.Location = new Point(80, 20);
            Request_iLabel.MaximumSize = new Size(290, iPanel.Height);//pictureBox.Location.X + pictureBox.Width + (iPanel.Width / 2)
            Request_iLabel.WrapText = true;
            #endregion

            #region Buttons
            Design.iButton2 Accept_Button = new Design.iButton2();
            Accept_Button.Name = $"AcceptButton_{Index}";
            Accept_Button.Text = "Annehmen";
            Accept_Button.Location = new Point(Request_iLabel.MaximumSize.Width + 80, 20);
            Accept_Button.Size = new Size(130, Accept_Button.Size.Height);
            Accept_Button.BorderRadius = 3;
            Accept_Button.HoverColor = Color.CornflowerBlue;
            Accept_Button.BackColor = Color.CornflowerBlue;
            Accept_Button.Click += acceptClick;

            Design.iButton2 Decline_Button = new Design.iButton2();
            Decline_Button.Name = $"DeclineButton_{Index}";
            Decline_Button.Text = "Ablehnen";
            Decline_Button.Location = new Point(Accept_Button.Location.X + Accept_Button.Width + 10, 20);
            Decline_Button.Size = new Size(130, Accept_Button.Size.Height);
            Decline_Button.BorderRadius = 3;
            Decline_Button.HoverColor = Color.Indigo;
            Decline_Button.BackColor = Color.Indigo;
            Decline_Button.Click += declineClick;

            Design.iButton2 Block_Button = new Design.iButton2();
            Block_Button.Name = $"BlockButton_{Index}";
            Block_Button.Text = "Blockieren";
            Block_Button.Location = new Point(Decline_Button.Location.X + Decline_Button.Width + 10, 20);
            Block_Button.Size = new Size(130, Accept_Button.Size.Height);
            Block_Button.BorderRadius = 3;
            Block_Button.HoverColor = Color.Firebrick;
            Block_Button.BackColor = Color.Firebrick;
            Block_Button.Click += blockClick;



            #endregion

            iPanel.Controls.Add(pictureBox);
            iPanel.Controls.Add(Request_iLabel);

            iPanel.Controls.Add(Accept_Button);
            iPanel.Controls.Add(Decline_Button);
            iPanel.Controls.Add(Block_Button);

            return iPanel;
        }
        public void Firestore_GetAllMessages()
        {
            CollectionReference collRef = FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).Collection("data").Document("messages").Collection("requests");
            Query query = collRef;
            int index = 0;

            firestoreMessagesListener = query.Listen(async snapshot =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    panel5.Controls.Clear();
                    requestPanels.Clear();
                    List_client_Messages.Clear();
                });

                index = 0;

                foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
                {
                    Dictionary<string, object> fields = documentSnapshot.ToDictionary();
                    string Type = fields["Type"].ToString();
                    string Username = fields["Username"].ToString();
                    string UsernameID = fields["UsernameID"].ToString();
                    string Email = documentSnapshot.Id;

                    List_client_Messages.Add(new Structures.Client_Messages { Type = Type, Username = Username, UsernameID = UsernameID, Email = Email });

                    if (Type == "Friend_Request")
                    {
                        string Text = $"Du hast eine Freundschaftsanfrage von {Username}#{UsernameID} erhalten";
                        //Panel RequestFormPanel = Friend_Designs.Request_Form_V.RequestFormPanel(Text, index, Firestore_BlockFR_Click, Firestore_DeclineFR_Click, Firestore_AcceptFR_Click);
                        Panel RequestFormPanel = GetAllMessages_Design(Text, index, "", Firestore_BlockFR_Click, Firestore_DeclineFR_Click, Firestore_AcceptFR_Click);
                        requestPanels.Add(RequestFormPanel);
                    }
                    else if (Type == "Session_Invitation")
                    {
                        string Text = $"Du wurdest von {Username}#{UsernameID} zur Party eingeladen";

                        //Panel RequestFormPanel = Friend_Designs.Request_Form_V.RequestFormPanel(Text, index, Firestore_BlockFR_Click, Firestore_DeclineFR_Click, Firestore_AcceptFR_Click);

                        requestPanels.Add(Friend_Designs.Request_Form_V.RequestFormPanel(Text, index));
                    }

                    this.Invoke((MethodInvoker)delegate
                    {
                        requestPanels[index].Dock = DockStyle.Top;
                        panel5.Controls.Add(requestPanels[index]);
                    });
                    index++;
                }

                if (index == 0)
                {
                    panel5.Controls.Clear();
                    ResetScroller();
                    NoMessagesControls();
                }
            });
            //await firestoreMessagesListener.StopAsync();
        }

        private void NoMessagesControls()
        {
            this.Invoke((MethodInvoker)delegate
            {
                Design.iPanel Panel = new Design.iPanel();
                Panel.BackColor = Color.FromArgb(100, Color.Black);
                Panel.Dock = DockStyle.Top;
                Panel.Size = new Size(panel5.Width - Panel.Location.X - 20, 100);

                Design.iLabel iLabel = new Design.iLabel();
                iLabel.AutoSize = true;
                iLabel.AlphaColor = 100;
                iLabel.BackColor = Color.Transparent;
                iLabel.Text = "Du hast keine Nachrichten";
                iLabel.Location = new Point(20, (Panel.Height - iLabel.Height) / 2);
                iLabel.Font = new Font("Microsoft Sans Serif", 15f, FontStyle.Regular);
                iLabel.ForeColor = Color.White;
                iLabel.HoverAnimation = false;

                Panel.Controls.Add(iLabel);
                panel5.Controls.Add(Panel);
            });
        }

        private void Firestore_BlockFR_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(GetControl("BlockButton_").Split('_')[1]);
            DocumentReference docRef = FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).Collection("data").Document("blockList");

            Dictionary<string, string> blockData = new Dictionary<string, string>()
            {
                { List_client_Messages[ID].Email, "" },
            };

            docRef.SetAsync(blockData, SetOptions.MergeAll);

            CollectionReference collRef = FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).Collection("data").Document("messages").Collection("requests");
            collRef.Document(List_client_Messages[ID].Email).DeleteAsync();
        }
        private void Firestore_DeclineFR_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(GetControl("DeclineButton_").Split('_')[1]);
            CollectionReference collRef = FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).Collection("data").Document("messages").Collection("requests");
            collRef.Document(List_client_Messages[ID].Email).DeleteAsync();
        }
        private void Firestore_AcceptFR_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(GetControl("AcceptButton_").Split('_')[1]);
            Console.WriteLine("accepted");
            DocumentReference docRef = FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).Collection("data").Document("friendList");
            DocumentReference senderClient = FirestoreGlobal.firestoreDb.Collection("Users").Document(List_client_Messages[ID].Email).Collection("data").Document("friendList");

            Dictionary<string, string> friendListData = new Dictionary<string, string>()
            {
                { List_client_Messages[ID].Email, "" },
            };

            docRef.SetAsync(friendListData, SetOptions.MergeAll);
            senderClient.SetAsync(friendListData, SetOptions.MergeAll);

            CollectionReference collRef = FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).Collection("data").Document("messages").Collection("requests");
            collRef.Document(List_client_Messages[ID].Email).DeleteAsync();


        }

        void ResetScroller()
        {
            iScrollBarV1.MovingState = false;
            iScrollBarV1.Value = 0;
            panel5.VerticalScroll.Value = 0;
            panel5.AutoScrollPosition = new Point(0, 0);
            panelScroll = 0;
        }

        private void iLabel4_Click(object sender, EventArgs e)
        {
            iLabel7.Text = "Deine Nachrichten";
            MarkLabel(messagesLabel);
            Firestore_GetAllMessages();
        }

        #endregion

        void OpenFriendList()
        {
            iLabel7.Text = "Deine Freunde";

            MarkLabel(yourFriendsLabel);
            ShowFriends();
            /*Thread thread = new Thread(() =>
            {
                Get_FriendInfo_Firestore();
            });
            thread.Start();*/
            panel5.VerticalScroll.Maximum = panel5.PreferredSize.Height;
        }
        private void iLabel2_Click(object sender, EventArgs e)
        {
            OpenFriendList();
        }

        private void addFriendsLabel_Click(object sender, EventArgs e)
        {
            iLabel7.Text = "Freunde hinzufügen";
            ResetScroller();

            MarkLabel(addFriendsLabel);

            Show_MyUsername_Controls();
            Show_AddFriend_Controls();
        }
        private void blockedAccountsLabel_Click(object sender, EventArgs e)
        {
            iLabel7.Text = "Blockierte Accounts";
            MarkLabel(blockedAccountsLabel);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }

        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }


        private void iScrollBarV1_Scroll()
        {
            try
            {
                var a = panel5.PreferredSize.Height - panel5.Size.Height;
                var scroll_Value = (iScrollBarV1.Value * a) / (iScrollBarV1.Size.Height - iScrollBarV1.LargeChange);

                panel5.VerticalScroll.Value = scroll_Value;
                panel5.AutoScrollPosition = new Point(0, scroll_Value);

                panel5.Update();
                //panel5.ScrollControlIntoView(panel5);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        void ContextStripSettings()
        {
            iContextStripMenu = new Design.iContextStripMenu();
            iContextStripMenu.AddOption("Freund entfernen", DeleteFriend);
            iContextStripMenu.AddOption("Blockieren", BlockFriend);
            iContextStripMenu.AddLine();
            iContextStripMenu.AddOption("Zur Party einladen", InviteToParty);
        }

        private void InviteToParty(object sender, EventArgs e)
        {

        }

        private void BlockFriend(object sender, EventArgs e)
        {

        }

        private void DeleteFriend(object sender, EventArgs e)
        {

        }
        private void FriendsForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Console.WriteLine("eterttertertrt");
                iContextStripMenu.StartPosition = FormStartPosition.Manual;
                iContextStripMenu.ShowInTaskbar = false;
                iContextStripMenu.Location = new Point(MousePosition.X + 10, MousePosition.Y + 10);

                if (iContextStripMenu.Visible == false)
                {
                    ContextMenuStrip_FocusCheckTimer.Start();
                    iContextStripMenu.Show();
                }
                else
                {
                    iContextStripMenu.Focus();
                }
            }

        }


        int panelScroll = 0;
        private void Panel5_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                int mousedeltaval = e.Delta / 120;
                int scrollVal = 70;


                if (mousedeltaval == 1) // up 
                    panelScroll -= scrollVal;

                if (mousedeltaval == -1) // down 
                    panelScroll += scrollVal;


                if (panel5.VerticalScroll.Value < 0 || panel5.VerticalScroll.Value + panelScroll < 0)
                {
                    //Console.WriteLine("<0kok");
                    panel5.AutoScrollPosition = new Point(0, 0);
                    panel5.VerticalScroll.Value = 0;
                    panelScroll = 0;
                    iScrollBarV1.Value = 0;
                    return;
                }
                if (panel5.VerticalScroll.Value + panelScroll > panel5.PreferredSize.Height - panel5.Size.Height)
                {
                    panel5.VerticalScroll.Value = panel5.PreferredSize.Height - panel5.Size.Height;
                    panelScroll = panel5.PreferredSize.Height - panel5.Size.Height;
                    return;
                }


                float k = (float)panelScroll / (float)panel5.PreferredSize.Height;
                iScrollBarV1.Value = Convert.ToInt32(iScrollBarV1.Height * k);


                panel5.VerticalScroll.Value = panelScroll;
                panel5.AutoScrollPosition = new Point(0, panelScroll);

                iScrollBarV1.Update();
                //panel5.Focus();
                panel5.Update();

                //panel5.ScrollControlIntoView(panel5);

            }
            catch { }
        }



        private void iButton21_Click_1(object sender, EventArgs e)
        {
            panelScroll = 0;
            panel5.VerticalScroll.Value = 0;
            panel5.AutoScrollPosition = new Point(0, 0);
            panel5.Update();
            panel5.ScrollControlIntoView(panel5);
        }

        private void iButton22_Click_1(object sender, EventArgs e)
        {
            DocumentReference docRef = FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).Collection("data").Document("friendList");
            Dictionary<string, object> friendListData = new Dictionary<string, object>()
            {
                { "KOK@GMAIL2.com", "" },
            };
            docRef.SetAsync(friendListData, SetOptions.MergeAll);
        }

        bool isOnStatusPanel = false;

        private void iPanel1_MouseEnter(object sender, EventArgs e)
        {
            iPanel1.BackColor = Color.FromArgb(100, Color.Black);
            StatusLabel.AlphaColor = 255;
            //if (!isOnStatusPanel)
            //ExpandArrow_AnimationOn();

            isOnStatusPanel = true;
        }

        private void iPanel1_MouseLeave(object sender, EventArgs e)
        {
            if (iContextStripMenu_Status.Visible)
                return;

            iPanel1.BackColor = Color.FromArgb(0, Color.Black);
            StatusLabel.AlphaColor = 200;
            //if (isOnStatusPanel)
            //ExpandArrow_AnimationOff();

            isOnStatusPanel = false;
        }

        Thread ExpandArrow_AnimationThreadOn;
        Thread ExpandArrow_AnimationThreadOff;

        void ExpandArrow_AnimationOn()
        {
            ExpandArrow_AnimationThreadOn = new Thread(() =>
            {
                for (int i = 0; i < 3; i++)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        pictureBox2.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y + i);
                    });
                    Thread.Sleep(40);
                }
            });
            ExpandArrow_AnimationThreadOn.Start();
        }
        void ExpandArrow_AnimationOff()
        {
            ExpandArrow_AnimationThreadOff = new Thread(() =>
            {
                for (int i = 0; i < 3; i++)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        pictureBox2.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y - i);
                    });
                    Thread.Sleep(40);
                }
            });
            ExpandArrow_AnimationThreadOff.Start();
        }

        void ContextStripStatus()
        {
            iContextStripMenu_Status = new Design.iContextStripMenu();
            iContextStripMenu_Status.AddOption("Online", Profile_SetOnline, Resources_Images.Images.Icons.Online, Design.iButton2.ImageAlignEnum.MiddleLeft, new Size(20, 20));
            iContextStripMenu_Status.AddOption("Abwesend", Profile_SetAFK, Resources_Images.Images.Icons.AFK, Design.iButton2.ImageAlignEnum.MiddleLeft, new Size(20, 20));
            iContextStripMenu_Status.AddOption("Beschäftigt", Profile_SetBusy, Resources_Images.Images.Icons.busy, Design.iButton2.ImageAlignEnum.MiddleLeft, new Size(20, 20));
            iContextStripMenu_Status.AddOption("Offline anzeigen", Profile_SetOffline, Resources_Images.Images.Icons.offline, Design.iButton2.ImageAlignEnum.MiddleLeft, new Size(20, 20));
            iContextStripMenu_Status.MouseEnter += IContextStripMenu_Status_MouseEnter;
            iContextStripMenu_Status.VisibleChanged += IContextStripMenu_Status_VisibleChanged;
            //iContextStripMenu_Status.AddLine();
        }

        private void IContextStripMenu_Status_VisibleChanged(object sender, EventArgs e)
        {
            if (iContextStripMenu_Status.Visible == false)
            {
                iPanel1.BackColor = Color.FromArgb(0, Color.Black);
                StatusLabel.AlphaColor = 200;
                ExpandArrow_AnimationOff();
            }
            else
            {
                iPanel1.BackColor = Color.FromArgb(100, Color.Black);
                StatusLabel.AlphaColor = 255;
                ExpandArrow_AnimationOn();
            }
        }

        private void IContextStripMenu_Status_MouseEnter(object sender, EventArgs e)
        {
            iPanel1.BackColor = Color.FromArgb(50, Color.Black);
            StatusLabel.AlphaColor = 255;
        }
        #region SetProfile

        void MyStatusListener()
        {
            DocumentReference docRef = FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email);
            firestoreMyStatusListener = docRef.Listen(async snapshot =>
            {
                GetStatus();
                firestoreMyStatusListener.StopAsync();
            });
        }

        void Profile_SetOnline(object sender, EventArgs e)
        {
            FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).UpdateAsync(new Dictionary<string, object>() { { "Status", "Online" } });
            iContextStripMenu_Status.Hide();
            ContextStripStatusTimer.Stop();
            MyStatusListener();
        }
        void Profile_SetAFK(object sender, EventArgs e)
        {
            FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).UpdateAsync(new Dictionary<string, object>() { { "Status", "AFK" } });
            iContextStripMenu_Status.Hide();
            ContextStripStatusTimer.Stop();
            MyStatusListener();

        }
        void Profile_SetBusy(object sender, EventArgs e)
        {
            FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).UpdateAsync(new Dictionary<string, object>() { { "Status", "Busy" } });
            iContextStripMenu_Status.Hide();
            ContextStripStatusTimer.Stop();
            MyStatusListener();
        }
        void Profile_SetOffline(object sender, EventArgs e)
        {
            FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).UpdateAsync(new Dictionary<string, object>() { { "Status", "Appear Offline" } });
            iContextStripMenu_Status.Hide();
            ContextStripStatusTimer.Stop();
            MyStatusListener();
        }
        #endregion

        private void iPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            iContextStripMenu_Status.StartPosition = FormStartPosition.Manual;
            iContextStripMenu_Status.ShowInTaskbar = false;
            iContextStripMenu_Status.Location = new Point(this.PointToScreen(iPanel1.Location).X, this.PointToScreen(iPanel1.Location).Y + iPanel1.Height + 30);

            if (iContextStripMenu_Status.Visible == false)
            {
                ContextStripStatusTimer.Start();
                iContextStripMenu_Status.Show();
            }
            else
            {
                iContextStripMenu_Status.Focus();

            }
        }

        private void ContextMenuStrip_FocusCheckTimer_Tick(object sender, EventArgs e)
        {

        }

        private void ContextStripStatusTimer_Tick(object sender, EventArgs e)
        {
            if (iContextStripMenu_Status.ContainsFocus == false)
            {
                iContextStripMenu_Status.Hide();
                ContextStripStatusTimer.Stop();
            }
        }


    }
}
