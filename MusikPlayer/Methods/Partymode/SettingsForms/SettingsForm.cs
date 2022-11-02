using Google.Cloud.Firestore;
using MusikPlayer.Methods.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods.Partymode.SettingsForms
{
    public partial class SettingsForm : UserControl
    {
        string sessionID = "";
        public SettingsForm(string sessionID)
        {
            InitializeComponent();
            this.sessionID = sessionID;
        }
        private void SettingsForm_Load(object sender, EventArgs e)
        {
           GetSessionInfo();
        }
        int GetPrivacyIndex(string Text)
        {
            if (Text.ToLower() == "open")
                return 0;
            else if (Text.ToLower() == "only invites")
                return 1;
            else if (Text.ToLower() == "closed")
                return 2;
            return -1;
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

        public async Task GetSessionInfo()
        {
            if (sessionID == "")
            {
                Global.iMessageBox.Show("Unbekannte SessionID", "Fehler");
                return;
            }
            var sessionFields = await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(sessionID).GetSnapshotAsync();
            Dictionary<string, object> sessionFieldsDict = sessionFields.ToDictionary();
            iTextBox1.Texts = sessionFieldsDict["SessionName"].ToString();
            string Privacy = sessionFieldsDict["privacy"].ToString();
            comboBox1.SelectedIndex = GetPrivacyIndex(Privacy);
            iSwitchBox1.Checked = (bool)sessionFieldsDict["AutoRole"];
            iSwitchBox2.Checked = (bool)sessionFieldsDict["RoomKeyHide"];
        }

        private void iButton21_Click(object sender, EventArgs e)
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

            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"SessionName", iTextBox1.Texts},
                {"privacy", GetPrivacy()},
                {"AutoRole", iSwitchBox1.Checked},
                {"RoomKeyHide", iSwitchBox2.Checked},
            };

            FirestoreGlobal.firestoreDb.Collection("Sessions").Document(sessionID).SetAsync(data, SetOptions.MergeAll);
        }
    }
}
