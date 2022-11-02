using Google.Cloud.Firestore;
using MusikPlayer.Methods.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods.Forms.Friendlist
{
    public partial class FriendListSearcher : Form
    {
        public Thread ShowFriendsTask;
        public bool isOpen = false;

        public FirestoreChangeListener firestorefriendListShow_listener;
        public List<object> friend_Emails = new List<object>();

        public Design.iContextStripMenu iContextStripMenu;

        public FriendListSearcher()
        {
            InitializeComponent();
            Show_Friend_Firestore();
            ContextStripSettings();
        }

        public void Background_ShowFriends()
        {

            while (true)
            {
                Console.WriteLine("ege");
                if (isOpen == false)
                {
                    ShowFriendsTask.Abort();
                    break;
                }
                else
                {
                    Show_Friends();
                }
                Thread.Sleep(1000);
            }
        }

        Form1 form1 = Application.OpenForms.OfType<Form1>().Single();


        public void Show_Friends()
        {
            try
            {
                List<string> FriendNames = Friendlist_Methods.GetMessagesInfo(Global.Authentication.Username, Global.Authentication.UsernameID, "ShowFriends").Split('\n').ToList();
                FriendNames.Remove("");

                string ClientDataString = Friendlist_Methods.GetMessagesInfo(Global.Authentication.Username, Global.Authentication.UsernameID, "GetFriendData");
                List<Friends_Handler.Client_Data> clientData = JsonConvert.DeserializeObject<List<Friends_Handler.Client_Data>>(Friends_Handler.Custom_JsonParser(ClientDataString));


                List<Friend_Designs.FriendForm> friendForms = new List<Friend_Designs.FriendForm>();
                friendForms.Clear();
                this.Invoke((MethodInvoker)delegate
                {
                    panel4.Controls.Clear();
                });
                Console.WriteLine("FriendCount: " + FriendNames.Count);
                for (int i = 0; i < FriendNames.Count; i++)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        friendForms.Add(new Friend_Designs.FriendForm());
                        friendForms[i].TopLevel = false;
                        friendForms[i].Dock = DockStyle.Top;
                        friendForms[i].Username_Label.Text = FriendNames[i];
                        if (clientData.Count > 0)
                            friendForms[i].Status_Label.Text = clientData[i].Status;
                        else
                            friendForms[i].Status_Label.Text = "Offline";

                        friendForms[i].Size = new Size(panel4.Width, friendForms[i].Height);
                        friendForms[i].Show();
                        panel4.Controls.Add(friendForms[i]);
                    });
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error");
            }

        }

        List<Friend_Designs.FriendForm> friendForms = new List<Friend_Designs.FriendForm>();

        void ShowFriend_Controls(int i, string Username, string Status)
        {

            this.Invoke((MethodInvoker)delegate
            {
                friendForms.Add(new Friend_Designs.FriendForm());
                friendForms[i].panel1.MouseClick += FriendListSearcher_MouseClick;
                friendForms[i].Username_Label.MouseClick += FriendListSearcher_MouseClick;
                friendForms[i].Status_Label.MouseClick += FriendListSearcher_MouseClick;
                friendForms[i].pictureBox1.MouseClick += FriendListSearcher_MouseClick;


                friendForms[i].TopLevel = false;
                friendForms[i].Dock = DockStyle.Top;
                friendForms[i].Username_Label.Text = Username;
                friendForms[i].Status_Label.Text = Status;

                friendForms[i].Size = new Size(panel4.Width, friendForms[i].Height);
                friendForms[i].Show();
                panel4.Controls.Add(friendForms[i]);
            });

        }

        private void FriendListSearcher_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Console.WriteLine("Right");

                iContextStripMenu.StartPosition = FormStartPosition.Manual;
                iContextStripMenu.ShowInTaskbar = false;
                iContextStripMenu.Location = new Point(MousePosition.X + 10, MousePosition.Y + 10);
                if (iContextStripMenu.Visible == false)
                {
                    ContextMenuStrip_FocusCheckTimer.Start();
                    iContextStripMenu.Show();
                    //Panels[index].BackColor = Color.FromArgb(60, 60, 60);
                    //old_selectedPanel = index;
                }
                else
                {
                    iContextStripMenu.Focus();
                }
            }
        }

        public async void Show_Friend_Firestore()
        {
            try
            {
                DocumentReference listener_doc = FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email);


                DocumentReference collRef = FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).Collection("data").Document("friendList");
                DocumentSnapshot documentSnapshot = await collRef.GetSnapshotAsync();
                Dictionary<string, object> dict_friends = documentSnapshot.ToDictionary();
                int index = 0;


                foreach (KeyValuePair<string, object> valuePair in dict_friends)
                {
                    //Console.WriteLine(valuePair.Key);
                    friend_Emails.Add(valuePair.Key);
                }

                Query friend_listener = FirestoreGlobal.firestoreDb.Collection("Users").WhereIn(FieldPath.DocumentId, friend_Emails);

                firestorefriendListShow_listener = friend_listener.Listen(async snapshot =>
                {
                    //Console.WriteLine("listen");

                    index = 0;
                    this.Invoke((MethodInvoker)delegate
                    {
                        panel4.Controls.Clear();
                        friendForms.Clear();
                        friend_Emails.Clear();
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

                        /*string onStatus = "";
                        if (string.IsNullOrEmpty(Status))
                            onStatus = CurrentSong;
                        else if (string.IsNullOrEmpty(CurrentSong))
                            onStatus = Status;*/

                            ShowFriend_Controls(index, Username, Status);
                            index++;
                        }
                    }
                });
            }
            catch
            {

            }
        }

        void ContextStripSettings()
        {
            iContextStripMenu = new Design.iContextStripMenu();
            iContextStripMenu.AddOption("Freund löschen", DeleteFriend);
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

        private void iButton24_Click(object sender, EventArgs e)
        {

        }

        private void panel4_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void ContextMenuStrip_FocusCheckTimer_Tick(object sender, EventArgs e)
        {
            if (iContextStripMenu.ContainsFocus == false)
            {
                //Panels[old_selectedPanel].BackColor = Color.FromArgb(20, 20, 20);
                iContextStripMenu.Hide();
                ContextMenuStrip_FocusCheckTimer.Stop();
            }
        }
    }
}
