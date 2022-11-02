using Google.Cloud.Firestore;
using MusikPlayer.Methods.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods.Forms.Friendlist
{
    public partial class Friend_Inbox : Form
    {
        public static string FilesInput = "";

        public static bool isOpen = false;

        public static List<Panel> requestPanels = new List<Panel>();

        public FirestoreChangeListener firestoreMessagesListener;

        List<Structures.Client_Messages> List_client_Messages = new List<Structures.Client_Messages>();

        public Friend_Inbox()
        {
            InitializeComponent();
            Firestore_GetAllMessages();
            this.DoubleBuffered = true;
            this.ResizeRedraw = false;
        }

        private void Friend_Inbox_Load(object sender, EventArgs e)
        {

        }
        public void ShowPanels()
        {
            for (int i = 0; i < requestPanels.Count; i++)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    requestPanels[i].Visible = true;
                    this.Update();
                });
            }
        }

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


        void GetAllMessages()
        {
            try
            {
                List<Friend_Designs.RequestForm> requestForms = new List<Friend_Designs.RequestForm>();

                requestForms.Clear();
                this.Invoke((MethodInvoker)delegate
                {
                    panel1.Controls.Clear();
                    for (int i = 0; i < Main_FriendList.send_Message.Count; i++)
                    {
                        requestForms.Add(new Friend_Designs.RequestForm());
                        requestForms[i].TopLevel = false;
                        requestForms[i].Dock = DockStyle.Top;

                        if (Main_FriendList.send_Message[i].Type == "Friend_Request")
                        {
                            requestForms[i].label1.Text = $"Du hast eine Freundschaftsanfrage von {Main_FriendList.send_Message[i].Username}#{Main_FriendList.send_Message[i].UsernameID} erhalten";
                        }
                        else if (Main_FriendList.send_Message[i].Type == "Session_Invitation")
                        {
                            requestForms[i].label1.Text = $"Du wurdest von {Main_FriendList.send_Message[i].Username}#{Main_FriendList.send_Message[i].UsernameID} zur Party eingeladen";
                        }

                        requestForms[i].Size = new Size(this.Width, requestForms[i].label1.Height + requestForms[i].panel1.Height + 20);
                        requestForms[i].Button_AcceptFR.Click += Button_AcceptFR_Click;
                        requestForms[i].Button_AcceptFR.Name = $"AcceptButton_{i}";

                        requestForms[i].Button_DeclineFR.Click += Button_DeclineFR_Click;
                        requestForms[i].Button_DeclineFR.Name = $"DeclineButton_{i}";

                        requestForms[i].Button_BlockFR.Click += Button_BlockFR_Click;
                        requestForms[i].Button_BlockFR.Name = $"BlockButton_{i}";

                        requestForms[i].Show();
                        panel1.Controls.Add(requestForms[i]);

                    }
                });
            }
            catch { }
        }



        public void PerformanceTest()
        {
            panel1.Controls.Clear();
            requestPanels.Clear();
            for (int i = 0; i < Main_FriendList.send_Message.Count; i++)
            {
                if (Main_FriendList.send_Message[i].Type == "Friend_Request")
                {
                    string Text = $"Du hast eine Freundschaftsanfrage von {Main_FriendList.send_Message[i].Username}#{Main_FriendList.send_Message[i].UsernameID} erhalten";
                    //var RequestFormPanel = Friend_Designs.Request_Form_V.RequestFormPanel($"Du hast eine Freundschaftsanfrage von {Main_FriendList.send_Message[i].Username}#{Main_FriendList.send_Message[i].UsernameID} erhalten");
                    //requestPanels.Add(Friend_Designs.Request_Form_V.RequestFormPanel($"Du hast eine Freundschaftsanfrage von {Main_FriendList.send_Message[i].Username}#{Main_FriendList.send_Message[i].UsernameID} erhalten"));
                }
                else if (Main_FriendList.send_Message[i].Type == "Session_Invitation")
                {
                    // requestPanels.Add(Friend_Designs.Request_Form_V.RequestFormPanel($"Du wurdest von {Main_FriendList.send_Message[i].Username}#{Main_FriendList.send_Message[i].UsernameID} zur Party eingeladen"));
                }

                requestPanels[i].Dock = DockStyle.Top;
                requestPanels[i].Visible = false;
                this.Controls.Add(requestPanels[i]);
            }
        }


        public void Firestore_GetAllMessages()
        {
            // DocumentReference docRef = FirestoreGlobal.firestoreDb.Collection("Users").Document("Bla@gmail.com").Collection("messages").Document("a@b.com");


            CollectionReference collRef = FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).Collection("data").Document("messages").Collection("requests");
            Query query = collRef;
            int index = 0;

            firestoreMessagesListener = query.Listen(async snapshot =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    panel1.Controls.Clear();
                    requestPanels.Clear();
                    List_client_Messages.Clear();
                });

                index = 0;

                foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
                {
                    Console.WriteLine(index);
                    Dictionary<string, object> fields = documentSnapshot.ToDictionary();

                    string Type = fields["Type"].ToString();
                    string Username = fields["Username"].ToString();
                    string UsernameID = fields["UsernameID"].ToString();
                    string Email = documentSnapshot.Id;

                    List_client_Messages.Add(new Structures.Client_Messages { Type = Type, Username = Username, UsernameID = UsernameID, Email = Email });

                    if (Type == "Friend_Request")
                    {
                        string Text = $"Du hast eine Freundschaftsanfrage von {Username}#{UsernameID} erhalten";
                        Panel RequestFormPanel = Friend_Designs.Request_Form_V.RequestFormPanel(Text, index, Firestore_BlockFR_Click, Firestore_DeclineFR_Click, Firestore_AcceptFR_Click);
                        requestPanels.Add(RequestFormPanel);
                    }
                    else if (Type == "Session_Invitation")
                    {
                        requestPanels.Add(Friend_Designs.Request_Form_V.RequestFormPanel($"Du wurdest von {Username}#{UsernameID} zur Party eingeladen", index));
                    }
                    this.Invoke((MethodInvoker)delegate
                    {
                        requestPanels[index].Dock = DockStyle.Top;
                        panel1.Controls.Add(requestPanels[index]);
                    });
                    index++;
                }
            });


            //await firestoreMessagesListener.StopAsync();
        }

        private void Firestore_BlockFR_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(GetControl("BlockButton_").Split('_')[1]);
            DocumentReference docRef = FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).Collection("data").Document("blockList");

            Dictionary<string, string> blockData = new Dictionary<string, string>()
            {
                { "Email", List_client_Messages[ID].Email },
            };

            docRef.CreateAsync(blockData);
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

            DocumentReference docRef = FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).Collection("data").Document("friendList");
           
            Dictionary<string, string> friendListData = new Dictionary<string, string>()
            {
                { "Email", List_client_Messages[ID].Email },
            };

            docRef.CreateAsync(friendListData);
            CollectionReference collRef = FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).Collection("data").Document("messages").Collection("requests");
            collRef.Document(List_client_Messages[ID].Email).DeleteAsync();
        }


        private void Button_BlockFR_Click(object sender, EventArgs e)
        {

            int ID = Convert.ToInt32(GetControl("Button_").Split('_')[1]);
            string clientMessage = "";
            if (Main_FriendList.send_Message[ID].Type == "Friend_Request")
                clientMessage = $"FriendRequest_{Main_FriendList.send_Message[ID].Username}#{Main_FriendList.send_Message[ID].UsernameID}";
            else if (Main_FriendList.send_Message[ID].Type == "Session_Invitation")
                clientMessage = $"SessionInvitation_{Main_FriendList.send_Message[ID].Username}#{Main_FriendList.send_Message[ID].UsernameID}";

            panel1.Controls.RemoveAt(ID);

            Friendlist_Methods.MessageActions(Global.Authentication.Username, Global.Authentication.UsernameID, "Block_FriendRequest", clientMessage);
        }

        private void Button_DeclineFR_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(GetControl("Button_").Split('_')[1]);
            string clientMessage = "";
            if (Main_FriendList.send_Message[ID].Type == "Friend_Request")
                clientMessage = $"FriendRequest_{Main_FriendList.send_Message[ID].Username}#{Main_FriendList.send_Message[ID].UsernameID}";
            else if (Main_FriendList.send_Message[ID].Type == "Session_Invitation")
                clientMessage = $"SessionInvitation_{Main_FriendList.send_Message[ID].Username}#{Main_FriendList.send_Message[ID].UsernameID}";

            //panel1.Controls.RemoveAt(ID);
            Friendlist_Methods.MessageActions(Global.Authentication.Username, Global.Authentication.UsernameID, "Decline_FriendRequest", clientMessage);
        }

        private void Button_AcceptFR_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(GetControl("Button_").Split('_')[1]);
            string clientMessage = "";
            if (Main_FriendList.send_Message[ID].Type == "Friend_Request")
                clientMessage = $"FriendRequest_{Main_FriendList.send_Message[ID].Username}#{Main_FriendList.send_Message[ID].UsernameID}";
            else if (Main_FriendList.send_Message[ID].Type == "Session_Invitation")
                clientMessage = $"SessionInvitation_{Main_FriendList.send_Message[ID].Username}#{Main_FriendList.send_Message[ID].UsernameID}";

            panel1.Controls.RemoveAt(ID);
            Friendlist_Methods.MessageActions(Global.Authentication.Username, Global.Authentication.UsernameID, "Accept_FriendRequest", clientMessage);
        }

    }
}
