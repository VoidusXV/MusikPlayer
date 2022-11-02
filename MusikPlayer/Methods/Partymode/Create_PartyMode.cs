using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods.Partymode
{
    public partial class Create_PartyMode : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        public Create_PartyMode()
        {
            InitializeComponent();
        }

        private void iLabel3_Click(object sender, EventArgs e)
        {
            PartyMode_Dashboard partyMode_Dashboard = new PartyMode_Dashboard();
            partyMode_Dashboard.Show();
            this.Hide();
        }

        private void iButton23_Click(object sender, EventArgs e)
        {
            CreateRoom();

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

        private void iLabel3_MouseEnter(object sender, EventArgs e)
        {
            iLabel3.AlphaColor = 255;
        }

        private void iLabel3_MouseLeave(object sender, EventArgs e)
        {
            iLabel3.AlphaColor = 200;
        }

        private string GetPrivacy()
        {
            if (comboBox1.SelectedIndex == -1)
                return "-1";
            else if (comboBox1.SelectedIndex == 0)
                return "Open";
            else if (comboBox1.SelectedIndex == 1)
                return "Only Invites";
            else if (comboBox1.SelectedIndex == 2)
                return "Closed";

            return "Nothing";
        }

        private async void CreateRoom()
        {
            if (GetPrivacy() == "-1")
            {
                Global.iMessageBox.Show("Du musst eine Option auswählen", "Fehler");
                return;
            }
            if (string.IsNullOrEmpty(iTextBox1.Texts))
            {
                Global.iMessageBox.Show("Ungültiger Raumname", "Fehler");
                return;
            };


            Dictionary<string, object> sessionFields_Dict = new Dictionary<string, object>()
            {
                {"privacy", GetPrivacy() },
                {"Album","" },
                {"Artist","" },
                {"AutoRole", iSwitchBox1.Checked},
                {"PlayState","" },
                {"RoomKey", new Random().Next(10000, 10000000) },
                {"RoomKeyHide", iSwitchBox2.Checked },
                {"SessionName", iTextBox1.Texts },
                {"SongFromUser","" },
                {"SongName","" },
                {"SongLink","" },
                {"SongPosition","" },
                {"SongDuration","" },
                {"ImageLink","" },

            };

            Dictionary<string, object> emptyDict = new Dictionary<string, object>();

            string DocumentID = Authentication.FirestoreGlobal.firestoreDb.Collection("Sessions").Document().Id;
            await Authentication.FirestoreGlobal.firestoreDb.Collection("Sessions").Document(DocumentID).CreateAsync(sessionFields_Dict);
            await Authentication.FirestoreGlobal.firestoreDb.Collection("Sessions").Document(DocumentID).Collection("Logs").Document("Logs").SetAsync(emptyDict);
            await Authentication.FirestoreGlobal.firestoreDb.Collection("Sessions").Document(DocumentID).Collection("banList").Document("banList").SetAsync(emptyDict);
            await Authentication.FirestoreGlobal.firestoreDb.Collection("Sessions").Document(DocumentID).Collection("members").Document("members").SetAsync(new Dictionary<string, object>() { { Global.Authentication.Email, "Host" } });

            await Authentication.FirestoreGlobal.firestoreDb.Collection("Sessions").Document(DocumentID).Collection("queue").Document("queue").SetAsync(new Dictionary<string, object>() { { Global.Authentication.Email, "Host" } });
            await Authentication.FirestoreGlobal.firestoreDb.Collection("Sessions").Document(DocumentID).Collection("queue").Document("alreadyListened").SetAsync(new Dictionary<string, object>() { { Global.Authentication.Email, "Host" } });

            await Authentication.FirestoreGlobal.firestoreDb.Collection("Users").Document(Global.Authentication.Email).SetAsync(new Dictionary<string, object>() { { "Session", DocumentID } }, SetOptions.MergeAll);

            PartyMode_Room partyMode_Room = new PartyMode_Room();
            partyMode_Room.Show();
            this.Hide();
        }

        private void iButton24_Click(object sender, EventArgs e)
        {
            Console.WriteLine(comboBox1.SelectedIndex);
        }

        private void iButton21_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void iButton22_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
