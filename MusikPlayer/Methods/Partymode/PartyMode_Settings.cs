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

namespace MusikPlayer.Methods.Partymode
{
    public partial class PartyMode_Settings : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        private string sessionID;

        SettingsForms.LogsForm logsForm;
        int iPanel1_PrefSizeHeight = 0;

        public PartyMode_Settings(string sessionID)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ResizeRedraw = false;

            LayoutAdjust();
            iScrollBarV1.MovingState = false;
            this.sessionID = sessionID;
            OpenSettings();
            //logsForm = new SettingsForms.LogsForm(sessionID);

        }

  
        void LayoutAdjust()
        {
            iLabel1.Location = new Point((iPanel21.Width - iLabel1.Width) / 2, (iPanel21.Height - iLabel1.Height) / 2);
        }
        public void Show(string Text)
        {
            this.Text = Text;
            this.Show();
        }
        void CreateEvents(Control button)
        {
            button.MouseEnter += Button_MouseEnter;
            button.MouseLeave += Button_MouseLeave;
            button.MouseDown += Button_MouseDown;
            button.MouseUp += Button_MouseUp;
        }

        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            iButton23.BackColor = Color.FromArgb(30, Color.Black);
        }
        private void Button_MouseLeave(object sender, EventArgs e)
        {
            iButton23.BackColor = Color.FromArgb(0, Color.Black);
        }

        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            iButton23.BackColor = Color.FromArgb(50, Color.Black);
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            iButton23.BackColor = Color.FromArgb(30, Color.Black);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        void NewTab(Control button)
        {
            gradientBackground2.Height = button.Height;
            gradientBackground2.Location = new Point(0, button.Location.Y);
            iPanel1.Controls.Clear();
            iPanel1_PrefSizeHeight = 0;
            ResetScroller(iPanel1);

        }
        void ResetScroller(Panel panel)
        {
            iScrollBarV1.MovingState = false;
            iScrollBarV1.Value = 0;
            panel.VerticalScroll.Value = 0;
            panel.AutoScrollPosition = new Point(0, 0);
            //panelScroll = 0;
        }

        void ScrollerSettings()
        {
            iScrollBarV1.MovingState = false;
            iPanel1.VerticalScroll.Maximum = iPanel1.PreferredSize.Height;
            iPanel1_PrefSizeHeight = iPanel1.PreferredSize.Height;
            if (iPanel1_PrefSizeHeight > iPanel1.Height)
            {
                float a = ((float)iPanel1.Height / (float)iPanel1.PreferredSize.Height);
                int LargeChangeVal = Convert.ToInt32((float)iScrollBarV1.Height * a);
                if (LargeChangeVal < 70)
                    LargeChangeVal = 70;

                iScrollBarV1.MovingState = true;
                iScrollBarV1.LargeChange = LargeChangeVal;
            }
        }

        private void iButton23_Click(object sender, EventArgs e)
        {
            NewTab(iButton23);
            SettingsForms.BannedAccForm bannedAccForm = new SettingsForms.BannedAccForm(sessionID);
            bannedAccForm.Dock = DockStyle.None;
            bannedAccForm.Size = iPanel1.Size;
            bannedAccForm.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            bannedAccForm.Location = new Point((iPanel1.Width - bannedAccForm.Width) / 2, (iPanel1.Height - bannedAccForm.Height) / 2);
            iPanel1.Controls.Add(bannedAccForm);
            bannedAccForm.Show();

            ScrollerSettings();
        }

        private async void iButton24_Click(object sender, EventArgs e)
        {
            NewTab(iButton24);
            logsForm = new SettingsForms.LogsForm(sessionID);
            logsForm.AutoSize = true;
            logsForm.Dock = DockStyle.None;
            iPanel1.Controls.Add(logsForm);
            logsForm.Show();
            await logsForm.Main();
            ScrollerSettings();
        }

        void OpenSettings()
        {
            NewTab(iButton25);
            SettingsForms.SettingsForm settingsForm = new SettingsForms.SettingsForm(sessionID);
            settingsForm.Dock = DockStyle.None;
            settingsForm.Size = iPanel1.Size;
            settingsForm.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            settingsForm.Location = new Point((iPanel1.Width - settingsForm.Width) / 2, (iPanel1.Height - settingsForm.Height) / 2);
            iPanel1.Controls.Add(settingsForm);
            settingsForm.Show();
        }
        private async void iButton25_Click(object sender, EventArgs e)
        {
            NewTab(iButton25);
            OpenSettings();
            ScrollerSettings();
        }

        private void iButton22_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void iButton21_Click(object sender, EventArgs e)
        {
            this.Hide();
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

        private void iScrollBarV1_Scroll()
        {
            try
            {
                var a = iPanel1_PrefSizeHeight - iPanel1.Size.Height;
                var scroll_Value = (iScrollBarV1.Value * a) / (iScrollBarV1.Size.Height - iScrollBarV1.LargeChange);

                iPanel1.VerticalScroll.Value = scroll_Value;
                iPanel1.AutoScrollPosition = new Point(0, scroll_Value);
                iPanel1.Update();
               // Console.WriteLine($"{iPanel1.PreferredSize.Height} | {iPanel1.Height} | {scroll_Value}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"Panel1: {iPanel1.PreferredSize}");
            Console.WriteLine($"Panel2: {iPanel1.Size}");
            Console.WriteLine($"iPanel1.VerticalScroll.Value: {iPanel1.VerticalScroll.Value}");
            Console.WriteLine($"logsForm1: {logsForm.PreferredSize}");
            Console.WriteLine($"logsForm2: {logsForm.Size}");

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Console.WriteLine(logsForm.PreferredSize.Height);
            Console.WriteLine(logsForm.Height);
        }
        List<Design.iPanel> friendForms = new List<Design.iPanel>();

        void Show_FriendControlsNew(int i, string Username, string Status)
        {
            this.Invoke((MethodInvoker)delegate
            {
                #region Create Panel
                Design.iPanel iPanel = new Design.iPanel();
                iPanel.Dock = DockStyle.Top;
                iPanel.BorderStyle = BorderStyle.FixedSingle;
                iPanel.Size = new Size(iPanel1.Width, 80);
                #endregion

                #region Create Labels
                Design.iLabel Username_iLabel = new Design.iLabel();
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

                PictureBox pictureBox = new PictureBox();
                pictureBox.Location = new Point(10, 10);
                pictureBox.Size = new Size(60, 60);
                //pictureBox.Image = ImageStream(@"C:\Users\HlDE1\Pictures\86bd63fbf50b50f5ce688dcf8260373d.png");
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
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
                iPanel.Controls.Add(pictureBox);
                iPanel.Controls.Add(Username_iLabel);
                iPanel.Controls.Add(Status_iLabel);
                iPanel.Controls.Add(PartyInvite_Button);

                friendForms.Add(iPanel);
                friendForms[i].Show();
                iPanel1.Controls.Add(friendForms[i]);
                #endregion
            });

        }

        private void button2_Click(object sender, EventArgs e)
        {
            iPanel1.Controls.Clear();
            for (int i = 0; i < 20; i++)
            {
                Show_FriendControlsNew(i, $"User: {i}", "Kopf");
            }
            iPanel1.VerticalScroll.Maximum = iPanel1.PreferredSize.Height;

            //Console.WriteLine(iPanel1.VerticalScroll.LargeChange);

            //iPanel1.VerticalScroll.Value = 150;
            //iPanel1.AutoScrollPosition = new Point(0, 150);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int val = Convert.ToInt32(textBox1.Text);
            iPanel1.VerticalScroll.Value = val;
            iPanel1.AutoScrollPosition = new Point(0, val);
            iPanel1.Update();
        }

        
    }
}
