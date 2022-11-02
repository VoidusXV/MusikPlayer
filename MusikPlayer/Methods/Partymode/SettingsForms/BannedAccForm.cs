using Google.Cloud.Firestore;
using MusikPlayer.Methods.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods.Partymode.SettingsForms
{
    public partial class BannedAccForm : UserControl
    {
        public string mySessionID = "";
        private Dictionary<string, object> bannedUsers;
        List<Design.iLabel> RevokeBan_LabelList = new List<Design.iLabel>();
        public BannedAccForm(string sessionID)
        {
            InitializeComponent();
            this.mySessionID = sessionID;
            Main();
        }

        Design.iPanel2 CreateControls(int index, string Username)
        {
            #region Panel
            Design.iPanel2 BannedPanel = new Design.iPanel2();
            BannedPanel.Border = true;
            BannedPanel.BorderColor = Color.FromArgb(10, Color.Black);
            BannedPanel.Size = new Size(this.Width - 40, 70);
            BannedPanel.Location = new Point(20, 20 + (index * 80));
            BannedPanel.BackColor = Color.FromArgb(30, Color.Black);//Color.Transparent;
            #endregion

            #region ImageBox
            Design.iRoundedPictureBox ClientImageBox = new Design.iRoundedPictureBox();
            ClientImageBox.Size = new Size(50, 50);
            ClientImageBox.Location = new Point(15, (BannedPanel.Height - ClientImageBox.Height) / 2);
            ClientImageBox.Image = Resources_Images.Images.Icons.profileImage_Placeholder;
            ClientImageBox.SizeMode = PictureBoxSizeMode.StretchImage;
            #endregion

            #region Labels
            Design.iLabel UsernameLabel = new Design.iLabel();
            UsernameLabel.AutoSize = true;
            UsernameLabel.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular);
            UsernameLabel.BackColor = Color.Transparent;
            UsernameLabel.ForeColor = Color.White;
            UsernameLabel.Location = new Point(73, (BannedPanel.Height - UsernameLabel.Height) / 2);
            UsernameLabel.Text = Username;
            #endregion

            #region Button
            Design.iLabel RevokeBan_Label = new Design.iLabel();
            RevokeBan_Label.Name = $"RevokeBanLabel_{index}";
            RevokeBan_Label.Cursor = Cursors.Hand;
            RevokeBan_Label.AutoSize = true;
            RevokeBan_Label.AlphaColor = 150;
            RevokeBan_Label.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular);
            RevokeBan_Label.BackColor = Color.Transparent;
            RevokeBan_Label.ForeColor = Color.Red;
            RevokeBan_Label.Location = new Point(300, (BannedPanel.Height - RevokeBan_Label.Height) / 2);
            RevokeBan_Label.Text = "Ban aufheben";
            RevokeBan_Label.MouseEnter += RevokeBan_Label_MouseEnter;
            RevokeBan_Label.MouseLeave += RevokeBan_Label_MouseLeave;
            RevokeBan_Label.MouseClick += RevokeBan_Label_MouseClick;
            #endregion

            RevokeBan_LabelList.Add(RevokeBan_Label);

            #region Add Panel
            BannedPanel.Controls.Add(ClientImageBox);
            BannedPanel.Controls.Add(UsernameLabel);
            BannedPanel.Controls.Add(RevokeBan_Label);
            #endregion
            return BannedPanel;
        }

        #region GetControl

        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point pnt);
        public static string GetControl(string containText)
        {
            try
            {

                IntPtr hWnd = WindowFromPoint(Control.MousePosition);
                if (hWnd != IntPtr.Zero)
                {
                    Control ctl = Control.FromHandle(hWnd);
                    if (ctl != null && ctl.Name.Contains(containText) && ctl.Visible == true)
                    {
                        return ctl.Name;
                    }
                }
            }
            catch { }
            return "-1";

        }
        #endregion

        int old_selectedIndex = -1;
        private async void RevokeBan_Label_MouseClick(object sender, MouseEventArgs e)
        {
            string TargetEmail = bannedUsers.ElementAt(old_selectedIndex).Key;
            Dictionary<string, object> deleteEmail_Dict = new Dictionary<string, object>()
            {
                { TargetEmail, FieldValue.Delete }
            };
            await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID).Collection("banList").Document("banList").SetAsync(deleteEmail_Dict, SetOptions.MergeAll);
        }

        private void RevokeBan_Label_MouseLeave(object sender, EventArgs e)
        {
            RevokeBan_LabelList[old_selectedIndex].AlphaColor = 255;
        }

        private void RevokeBan_Label_MouseEnter(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(GetControl("RevokeBanLabel_").Split('_')[1]);
            RevokeBan_LabelList[index].AlphaColor = 150;
            old_selectedIndex = index;
        }

        private void NoBansControls()
        {
            this.Invoke((MethodInvoker)delegate
            {
                Design.iPanel Panel = new Design.iPanel();
                Panel.BackColor = Color.FromArgb(100, Color.Black);
                Panel.Dock = DockStyle.Top;
                Panel.Size = new Size(this.Width - Panel.Location.X - 20, 100);

                Design.iLabel iLabel = new Design.iLabel();
                iLabel.AutoSize = true;
                iLabel.AlphaColor = 100;
                iLabel.BackColor = Color.Transparent;
                iLabel.Text = "Du hast bisher keinen gebannt";
                iLabel.Location = new Point(20, (Panel.Height - iLabel.Height) / 2);
                iLabel.Font = new Font("Microsoft Sans Serif", 15f, FontStyle.Regular);
                iLabel.ForeColor = Color.White;
                iLabel.HoverAnimation = false;

                Panel.Controls.Add(iLabel);
                this.Controls.Add(Panel);
            });
        }


        private async void Main()
        {
            await GetBannedInfo();
            ShowControls();
        }

        private void ShowControls()
        {
            this.Controls.Clear();
            RevokeBan_LabelList.Clear();

            if (bannedUsers.Count == 0)
            {
                NoBansControls();
                return;
            }

            int index = 0;
            foreach (var item in bannedUsers)
            {
                this.Controls.Add(CreateControls(index, item.Value.ToString()));
                index++;
            }
        }

        private async Task GetBannedInfo()
        {
            DocumentSnapshot banListSnapshot = await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID).Collection("banList").Document("banList").GetSnapshotAsync();
            bannedUsers = banListSnapshot.ToDictionary();
        }
    }
}
