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
    public partial class Main_FriendList : Form
    {
        Design.iCircleLabel iCircleLabel = new Design.iCircleLabel();

        //Firestore Variables
        public FirestoreChangeListener firestoreMessagesCountListener;
        public FirestoreChangeListener firestorefriendListListener;

      


        public FriendListSearcher friendListSearcher; //= new FriendListSearcher();



        //------------------------------- Friend-Inbox----------------------------------------------------------//

        public Friend_Inbox friend_Inbox; //= new Friend_Inbox();
        public static List<Friends_Handler.Send_Message> send_Message = new List<Friends_Handler.Send_Message>();
        public static string FilesInput = "";


        //------------------------------- This Form-----------------------------------------------------------//
        public bool isOpen = false;

        public Main_FriendList()
        {
            InitializeComponent();

            friendListSearcher = new FriendListSearcher();
            friendListSearcher.isOpen = true;
            AddFormToPanel(friendListSearcher, panel2);
            CreateCircleLabel();
            ListenMessagesCount();
            // WireMouseEvents(iButton23);
            Console.WriteLine(iButton23.Controls.Count);


        }
        void WireMouseEvents(Control container)
        {
            foreach (Control c in container.Controls)
            {
                c.Click += (s, e) => OnClick(e);
                c.MouseEnter += (s, e) => OnMouseEnter(e);
                c.MouseLeave += (s, e) => OnMouseLeave(e);

                c.MouseClick += (s, e) =>
                {
                    var p = PointToThis((Control)s, e.Location);
                    OnMouseClick(new MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta));
                    iButton23_Click(iButton23, e);
                };
            };
        }
        Point PointToThis(Control c, Point p)
        {
            return PointToClient(c.PointToScreen(p));
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
                        iCircleLabel.Visible = true;
                        iCircleLabel.Text = messagesCount.ToString();
                    }
                    else
                    {
                        iCircleLabel.Visible = false;
                    }
                    if (messagesCount > 99)
                        iCircleLabel.Text = ">99";
                });

            });
            //await firestoreListener.StopAsync();
        }


        public void CreateCircleLabel()
        {
            iCircleLabel.Text = "";
            iCircleLabel.Size = new Size(20, 20);
            iCircleLabel.Location = new Point(317, 15);
            iCircleLabel.BackColor = Color.FromArgb(14, 14, 15);
            iCircleLabel.FillColor = Color.Red;
            iCircleLabel.Visible = false;

            //iCircleLabel.MouseEnter += iButton23_MouseEnter;
            //iCircleLabel.MouseLeave += iButton23_MouseLeave;
           // iCircleLabel.MouseDown +=       ;
            iCircleLabel.MouseClick += iButton23_Click;

            this.Controls.Add(iCircleLabel);
            iCircleLabel.BringToFront();
        }

        private void Main_FriendList_Load(object sender, EventArgs e)
        {
            iButton21.BorderSize = 1;
            //isOpen = true;
        }

        public static string Custom_JsonParser(string text)
        {
            text = text.Insert(0, "[");
            string[] spliText = text.Split('}');
            string EndText = "";

            for (int i = 0; i < spliText.Length - 1; i++)
            {
                spliText[i] = spliText[i].Insert(spliText[i].Length, "},");
                EndText += spliText[i];
            }
            EndText += "]";
            return EndText;
        }


        public void Background_Inbox()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (isOpen == true)
                {
                    FilesInput = Friendlist_Methods.GetMessagesInfo(Global.Authentication.Username, Global.Authentication.UsernameID, "ReadFilesInput");
                    if (FilesInput.Contains("{"))
                    {
                        send_Message = JsonConvert.DeserializeObject<List<Friends_Handler.Send_Message>>(Custom_JsonParser(FilesInput));
                    }
                    else
                    {
                        send_Message.Clear();
                    }
                }
                else
                {
                    break;
                }
            }
        }


        public void Background_Refresh()
        {
            while (true)
            {
                Thread.Sleep(2000);

                if (isOpen == true)
                {
                    string msg = Friendlist_Methods.GetMessagesInfo(Global.Authentication.Username, Global.Authentication.UsernameID, "Count");
                    if (!string.IsNullOrEmpty(msg) || !string.IsNullOrWhiteSpace(msg))
                        Friendlist_Methods.Unread_MessagesCount = Convert.ToInt32(msg);

                    //Console.WriteLine("Refresh_Thread...");
                    this.Invoke((MethodInvoker)delegate
                    {
                        if (Friendlist_Methods.Unread_MessagesCount > 0)
                        {
                            iCircleLabel.Text = Friendlist_Methods.Unread_MessagesCount.ToString();
                            iCircleLabel.Visible = true;
                        }
                        else
                        {
                            iCircleLabel.Visible = false;

                        }
                    });
                }
                else
                {
                    break;
                }
            }
        }


        void AddFormToPanel(Form form, Panel panel)
        {
            panel.Controls.Clear();
            form.Size = panel2.Size;
            form.TopLevel = false;
            form.ShowIcon = false;
            form.Dock = DockStyle.None;
            panel.Controls.Add(form);
            form.Show();
        }





        private void iButton23_Click(object sender, EventArgs e)
        {
            friendListSearcher.isOpen = false;
            iButton21.BorderSize = 0;
            iButton22.BorderSize = 0;
            iButton23.BorderSize = 1;

            friend_Inbox = new Friend_Inbox();
            Friend_Inbox.isOpen = true;
            AddFormToPanel(friend_Inbox, panel2);
        }


        private void iButton21_Click(object sender, EventArgs e)
        {

            iButton21.BorderSize = 1;
            iButton22.BorderSize = 0;
            iButton23.BorderSize = 0;

            friendListSearcher.ShowFriendsTask = new Thread(friendListSearcher.Background_ShowFriends);
            friendListSearcher.isOpen = true;
            AddFormToPanel(friendListSearcher, panel2);
            friendListSearcher.ShowFriendsTask.Start();
        }
        private void iButton22_Click(object sender, EventArgs e)
        {
            friendListSearcher.isOpen = false;

            iButton21.BorderSize = 0;
            iButton22.BorderSize = 1;
            iButton23.BorderSize = 0;


            FriendSearch friendSearch = new FriendSearch();
            AddFormToPanel(friendSearch, panel2);
        }
        private void iButton23_MouseDown(object sender, MouseEventArgs e)
        {
            iCircleLabel.BackColor = iButton23.BackColor;
        }

        private void iButton23_MouseEnter(object sender, EventArgs e)
        {
            iCircleLabel.BackColor = iButton23.BackColor;

        }

        private void iButton23_MouseLeave(object sender, EventArgs e)
        {
            iCircleLabel.BackColor = iButton23.BackColor;
        }
    }
}
