using MusikPlayer.Methods.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using Google.Cloud.Firestore;

namespace MusikPlayer.Methods.Partymode
{
    public partial class PartyMode_Dashboard : Form
    {
        private bool mouseDown;
        private Point lastLocation;

        private string sessionID = "";

        public PartyMode_Dashboard()
        {
            InitializeComponent();
            timer1.Start();
            iPanel22.Parent = gradientPanel1;
            iPictureBox1.Image = (Bitmap)rotateImage(Resources_Images.Images.Icons.expand_arrow_50px, -90);
            iPictureBox2.Image = (Bitmap)rotateImage(Resources_Images.Images.Icons.expand_arrow_50px, -90);
        }


        private Bitmap rotateImage(Bitmap b, float angle)

        {

            int maxside = (int)(Math.Sqrt(b.Width * b.Width + b.Height * b.Height));

            //create a new empty bitmap to hold rotated image

            Bitmap returnBitmap = new Bitmap(maxside, maxside);

            //make a graphics object from the empty bitmap

            Graphics g = Graphics.FromImage(returnBitmap);





            //move rotation point to center of image

            g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);

            //rotate

            g.RotateTransform(angle);

            //move image back

            g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);

            //draw passed in image onto graphics object

            g.DrawImage(b, new Point(0, 0));



            return returnBitmap;

        }

        Thread ArrowAnimationThreadOn;
        Thread ArrowAnimationThreadOff;

        public void ArrowAnimationOn(PictureBox pictureBox)
        {

            ArrowAnimationThreadOn = new Thread(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        pictureBox.Location = new Point(pictureBox.Location.X + i, pictureBox.Location.Y);
                    });
                    Thread.Sleep(20);
                }
            });

            /*if (ArrowAnimationThreadOn != null)
            {
                if (ArrowAnimationThreadOn.IsAlive == true)
                {
                    return;
                }
            }
            */
            ArrowAnimationThreadOn.Start();
        }
        public void ArrowAnimationOff(PictureBox pictureBox)
        {
            ArrowAnimationThreadOff = new Thread(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        pictureBox.Location = new Point(pictureBox.Location.X - i, pictureBox.Location.Y);
                    });
                    Thread.Sleep(20);
                }
            });

            ArrowAnimationThreadOff.Start();
        }

        private void iLabel2_Click(object sender, EventArgs e)
        {

        }
        private void iPanel22_MouseEnter(object sender, EventArgs e)
        {
            ArrowAnimationOn(iPictureBox1);
            iLabel3.AlphaColor = 255;
        }

        private void iPanel22_MouseLeave(object sender, EventArgs e)
        {
            ArrowAnimationOff(iPictureBox1);
            iLabel3.AlphaColor = 150;
        }

        private void iPanel23_MouseEnter(object sender, EventArgs e)
        {
            ArrowAnimationOn(iPictureBox2);
            iLabel4.AlphaColor = 255;

        }

        private void iPanel23_MouseLeave(object sender, EventArgs e)
        {
            ArrowAnimationOff(iPictureBox2);
            iLabel4.AlphaColor = 150;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Console.WriteLine(iPanel22.ClientRectangle.Contains(iPanel22.PointToClient(Cursor.Position)));
        }

        void PressAnimationOn(Control control)
        {
            control.Location = new Point(control.Location.X + 2, control.Location.Y + 2);
            control.Size = new Size(control.Size.Width - 4, control.Size.Height - 4);
        }
        void PressAnimationOff(Control control)
        {
            control.Location = new Point(control.Location.X - 2, control.Location.Y - 2);
            control.Size = new Size(control.Size.Width + 4, control.Size.Height + 4);
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


        private void iPanel22_MouseDown_IgnoreChildren()
        {
            PressAnimationOn(iPanel22);
            PressAnimationOn(iPictureBox1);
        }

        private void iPanel22_MouseUp_IgnoreChildren()
        {
            PressAnimationOff(iPanel22);
            PressAnimationOff(iPictureBox1);

            Create_PartyMode create_PartyMode = new Create_PartyMode();
            create_PartyMode.Show();
            this.Hide();
        }
        private async void iPanel23_MouseUp(object sender, MouseEventArgs e)
        {
            PressAnimationOff(iPanel23);
            PressAnimationOff(iPictureBox2);
            Console.WriteLine("Test");
            if (!string.IsNullOrEmpty(iTextBox1.Texts))
                await JoinRoom(iTextBox1.Texts);
            else
                Global.iMessageBox.Show("Ungültige Eingabe", "Fehler");
        }

        private void iPanel23_MouseDown(object sender, MouseEventArgs e)
        {
            PressAnimationOn(iPanel23);
            PressAnimationOn(iPictureBox2);
        }

        private async Task<bool> roomExists(string myRoomKey)
        {
            CollectionReference sessionRef = FirestoreGlobal.firestoreDb.Collection("Sessions");
            QuerySnapshot sessionSnapshot = await sessionRef.GetSnapshotAsync();
            sessionID = "";
            foreach (var item in sessionSnapshot)
            {
                DocumentSnapshot docRoomKey_Snapshot = await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(item.Id).GetSnapshotAsync();
                Dictionary<string, object> docRoomKey_Dict = docRoomKey_Snapshot.ToDictionary();
                if (docRoomKey_Dict.ContainsKey("RoomKey"))
                {
                    string RoomKey = docRoomKey_Dict["RoomKey"].ToString();
                    Console.WriteLine(RoomKey);
                    if (myRoomKey == RoomKey)
                    {
                        sessionID = item.Id;
                        return true;
                    }
                }
            }
            return false;
        }

        private async Task<bool> roomIsClosed(string sessionID)
        {
            DocumentSnapshot docRoomKey_Snapshot = await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(sessionID).GetSnapshotAsync();
            Dictionary<string, object> docRoomKey_Dict = docRoomKey_Snapshot.ToDictionary();
            string privacy = docRoomKey_Dict["privacy"].ToString();
            if (privacy == "Closed")
                return true;
            return false;
        }
        private async Task<bool> roomIsInviteOnly(string sessionID)
        {
            DocumentSnapshot docRoomKey_Snapshot = await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(sessionID).GetSnapshotAsync();
            Dictionary<string, object> docRoomKey_Dict = docRoomKey_Snapshot.ToDictionary();
            string privacy = docRoomKey_Dict["privacy"].ToString();
            if (privacy.Contains("invite"))
                return true;
            return false;
        }
        private async Task<bool> isHost(string sessionID, string myEmail)
        {
            DocumentSnapshot membersList_Snapshot = await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(sessionID).Collection("members").Document("members").GetSnapshotAsync();
            Dictionary<string, object> membersListDict = membersList_Snapshot.ToDictionary();
            if (!membersListDict.ContainsKey(myEmail))
                return false;
            if (membersListDict[myEmail].Equals("Host"))
                return true;

            return false;
        }

        private async Task<bool> isBanned(string myEmail)
        {
            DocumentSnapshot membersList_Snapshot = await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(sessionID).Collection("banList").Document("banList").GetSnapshotAsync();
            Dictionary<string, object> bannedListDict = membersList_Snapshot.ToDictionary();
            if (bannedListDict.ContainsKey(myEmail))
                return true;

            return false;
        }

        private async Task JoinRoom(string RoomKey)
        {

            if (await roomExists(RoomKey) == false)
            {
                Global.iMessageBox.Show($"Der Raum wurde nicht gefunden", "Nicht gefunden");
                return;
            }
            bool _isHost = await isHost(sessionID, Global.Authentication.Email);
            if (await isBanned(Global.Authentication.Email) && _isHost == false)
            {
                Global.iMessageBox.Show($"Du wurdest gebannt", "Gebannt");
                return;
            }
            if (await roomIsClosed(sessionID) && _isHost == false)
            {
                Global.iMessageBox.Show($"Der Raum ist geschlossen", "Geschlossen");
                return;
            }
            if (await roomIsInviteOnly(sessionID) && _isHost == false)
            {
                Global.iMessageBox.Show($"Der Raum ist nur für Personen zugänglich mit einer Einladung", "Keine Einladung");
                return;
            }

            var sessionFields = await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(sessionID).GetSnapshotAsync();
            Dictionary<string, object> sessionFieldsDict = sessionFields.ToDictionary();
            bool isAutoRole = (bool)sessionFieldsDict["AutoRole"];

            await FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).SetAsync(new Dictionary<string, object>() { { "Session", sessionID } }, SetOptions.MergeAll);
            PartyMode_Room partyMode_Room = new PartyMode_Room();

            if (_isHost == false)
            {
                if (isAutoRole == false)
                    await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(sessionID).Collection("members").Document("members").SetAsync(new Dictionary<string, object>() { { Global.Authentication.Email, "Listener" } }, SetOptions.MergeAll);
                else
                    await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(sessionID).Collection("members").Document("members").SetAsync(new Dictionary<string, object>() { { Global.Authentication.Email, "SongSelector" } }, SetOptions.MergeAll);

                await PartyMode_Methods.CheckSessionIsClosed(sessionID, partyMode_Room);
            }
            Global.PartyMode_SessionID = sessionID;
            Global.PartyMode = true;
            partyMode_Room.Show();
            this.Hide();
        }

        private void iButton22_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void iButton21_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }

}
