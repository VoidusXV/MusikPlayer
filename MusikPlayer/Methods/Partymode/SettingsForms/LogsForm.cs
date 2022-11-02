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
    public partial class LogsForm : UserControl
    {
        private Dictionary<string, object> logsInfo;
        public string mySessionID = "";
        public LogsForm(string mySessionID)
        {
            InitializeComponent();
            this.mySessionID = mySessionID;
            //Main();
        }
        private async void LogsForm_Load(object sender, EventArgs e)
        {

        }

        public async Task Main()
        {
            await GetLogsInfo();
            ShowControls();
        }

        Design.iPanel2 CreateControls(int index, string Username, string Action, string TargetClientName, string DateTime)
        {
            #region Panel
            Design.iPanel2 LogPanel = new Design.iPanel2();
            LogPanel.Border = true;
            LogPanel.BorderColor = Color.FromArgb(10, Color.Black);
            LogPanel.Size = new Size(this.Width - 40, 70);
            LogPanel.Location = new Point(20, index * 80);
            LogPanel.BackColor = Color.FromArgb(30, Color.Black);//Color.Transparent;
            #endregion

            #region ImageBox
            Design.iRoundedPictureBox ClientImageBox = new Design.iRoundedPictureBox();
            ClientImageBox.Size = new Size(50, 50);
            ClientImageBox.Location = new Point(15, (LogPanel.Height - ClientImageBox.Height) / 2);
            ClientImageBox.Image = Resources_Images.Images.Icons.profileImage_Placeholder;
            ClientImageBox.SizeMode = PictureBoxSizeMode.StretchImage;
            #endregion

            #region Labels

            int textDistance = 3;
            int TextY = 13;

            Design.iLabel ActionLabel = new Design.iLabel();
            ActionLabel.AutoSize = true;
            ActionLabel.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular);
            ActionLabel.BackColor = Color.Transparent;
            ActionLabel.ForeColor = Color.White;
            ActionLabel.Location = new Point(73, TextY);
            ActionLabel.Text = Action;

            #region Still working on
            /*Design.iLabel UsernameLabel = new Design.iLabel();
            UsernameLabel.AutoSize = true;
            UsernameLabel.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular);
            UsernameLabel.Text = Username;
            UsernameLabel.BackColor = Color.Transparent;
            UsernameLabel.ForeColor = Color.CornflowerBlue;
            UsernameLabel.Location = new Point(73, TextY);
            Size UsernameLabelSize = UsernameLabel.CreateGraphics().MeasureString(UsernameLabel.Text, UsernameLabel.Font).ToSize();


            Design.iLabel ActionLabel = new Design.iLabel();
            ActionLabel.AutoSize = true;
            ActionLabel.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular);
            ActionLabel.BackColor = Color.Transparent;
            ActionLabel.ForeColor = Color.White;
            ActionLabel.Location = new Point(UsernameLabel.Location.X + UsernameLabelSize.Width + textDistance, TextY);
            ActionLabel.Text = Action;
            Size ActionSize = ActionLabel.CreateGraphics().MeasureString(ActionLabel.Text, ActionLabel.Font).ToSize();

            Design.iLabel TargetClientNameLabel = new Design.iLabel();
            TargetClientNameLabel.AutoSize = true;
            TargetClientNameLabel.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular);
            TargetClientNameLabel.BackColor = Color.Transparent;
            TargetClientNameLabel.ForeColor = Color.CornflowerBlue;
            TargetClientNameLabel.Location = new Point(ActionLabel.Location.X + ActionSize.Width + textDistance, TextY);
            TargetClientNameLabel.Text = TargetClientName;*/

            Design.iLabel DateTimeLabel = new Design.iLabel();
            DateTimeLabel.AutoSize = true;
            DateTimeLabel.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular);
            DateTimeLabel.Text = DateTime;
            DateTimeLabel.BackColor = Color.Transparent;
            DateTimeLabel.ForeColor = Color.White;
            DateTimeLabel.Location = new Point(74, 42);

            #endregion

            #endregion

            #region Add Panel
            LogPanel.Controls.Add(ClientImageBox);
            //LogPanel.Controls.Add(UsernameLabel);
            LogPanel.Controls.Add(ActionLabel);
            //LogPanel.Controls.Add(TargetClientNameLabel);
            LogPanel.Controls.Add(DateTimeLabel);
            #endregion

            return LogPanel;
            //this.Controls.Add(LogPanel);
        }

        public void ShowControls()
        {
            for (int i = 0; i < logsInfo.Count; i++)
            {
                int HeadKey = i;
                string Action = GetNestedDictionary(logsInfo, HeadKey, "Action");
                string _DateTime = GetNestedDictionary(logsInfo, HeadKey, "DateTime");
                //string Email = GetNestedDictionary(logsInfo, HeadKey, "Email");
                //string Username = GetNestedDictionary(logsInfo, HeadKey, "Username");

                this.Controls.Add(CreateControls(i, "", Action, "", _DateTime));
                this.Update();
            }
            //Console.WriteLine(this.Height);
            //Console.WriteLine(this.PreferredSize.Height);

        }
        string GetNestedDictionary(Dictionary<string, object> dict, int headKey, string Key)
        {
            var a = (Dictionary<string, object>)dict[headKey.ToString()];
            return a[Key].ToString();
        }

       public async Task GetLogsInfo()
        {
            DocumentSnapshot logsSnapshot = await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID).Collection("Logs").Document("Logs").GetSnapshotAsync();
            logsInfo = logsSnapshot.ToDictionary();
            /*int HeadKey = 0;
            string Action = GetNestedDictionary(logsInfo, HeadKey, "Action");
            string _DateTime = GetNestedDictionary(logsInfo, HeadKey, "DateTime");
            string Email = GetNestedDictionary(logsInfo, HeadKey, "Email");
            string Username = GetNestedDictionary(logsInfo, HeadKey, "Username");

            Console.WriteLine(Action);
            Console.WriteLine(_DateTime);
            Console.WriteLine(Email);
            Console.WriteLine(Usernam);*/
        }


    }
}
