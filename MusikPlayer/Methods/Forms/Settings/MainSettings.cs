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

namespace MusikPlayer.Methods.Forms.Settings
{
    public partial class MainSettings : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        public MainSettings()
        {
            InitializeComponent();
            AccountOverview accountOverview = new AccountOverview();
            accountOverview.TopLevel = false;
            accountOverview.Dock = DockStyle.None;
            accountOverview.Show();
            panel2.Controls.Add(accountOverview);

            this.Opacity = 0;
            OpenFormFadeTimer.Start();

        }

        private void FormFadeTimer_Tick(object sender, EventArgs e)
        {
            if (Opacity >= 1)
                OpenFormFadeTimer.Stop();
            this.Opacity += .08;
        }
        private void CloseFormFadeTimer_Tick(object sender, EventArgs e)
        {
            if (Opacity <= 0)
            {
                this.Hide();
                CloseFormFadeTimer.Stop();
            }
            this.Opacity -= .08;
        }
        private void iButton21_Click(object sender, EventArgs e)
        {
            gradientBackground1.Visible = true;
            gradientBackground2.Visible = false;
        }

        private void iButton22_Click(object sender, EventArgs e)
        {
            AccountOverview accountOverview = new AccountOverview();
            accountOverview.TopLevel = false;
            accountOverview.Dock = DockStyle.None;
            accountOverview.Show();
            panel2.Controls.Add(accountOverview);

            gradientBackground1.Visible = false;
            gradientBackground2.Visible = true;
        }
        private void iButton23_Click(object sender, EventArgs e)
        {
            CloseFormFadeTimer.Start();
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                     (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);
                this.Update();
            }
        }

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;

        }

      
    }
}
