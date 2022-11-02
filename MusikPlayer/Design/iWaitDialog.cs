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

namespace MusikPlayer.Design
{
    public partial class iWaitDialog : Form
    {
        public iWaitDialog()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Opacity = 0;
            OpenFormFadeTimer.Start();
        }

        private void OpenFormFadeTimer_Tick(object sender, EventArgs e)
        {
            if (Opacity >= 1)
                OpenFormFadeTimer.Stop();
            this.Opacity += .05;
        }

        private void CloseFormFadeTimer_Tick(object sender, EventArgs e)
        {
            if (Opacity <= 0)
            {
                this.Close();
                CloseFormFadeTimer.Stop();
            }
            this.Opacity -= .05;
        }
        public void Show(string Text)
        {
            iLabel1.Text = Text;
            if (iLabel1.Location.X + iLabel1.Size.Width > this.Width)
                this.Width = iLabel1.Location.X + iLabel1.Size.Width + 10;
            this.Show();
        }
        public void Hide()
        {
            CloseFormFadeTimer.Start();
        }

        private void iLabel1_TextChanged(object sender, EventArgs e)
        {
            if (iLabel1.Location.X + iLabel1.Size.Width > this.Width)
                this.Width = iLabel1.Location.X + iLabel1.Size.Width + 10;
        }

        private void iLabel1_SizeChanged(object sender, EventArgs e)
        {
            if (iLabel1.Location.X + iLabel1.Size.Width > this.Width)
                this.Width = iLabel1.Location.X + iLabel1.Size.Width + 10;
        }
    }
}
