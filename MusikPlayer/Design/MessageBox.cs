using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Design
{
    public partial class MessageBox : Form
    {
        private bool mouseDown;
        private Point lastLocation;

        List<Panel> panels = new List<Panel>();
        /* System.Media.SystemSounds.Asterisk,
                                  System.Media.SystemSounds.Beep,
                                  System.Media.SystemSounds.Exclamation,
                                  System.Media.SystemSounds.Hand,
                                  System.Media.SystemSounds.Question*/
        public MessageBox()
        {
            InitializeComponent();
            PanelSettings();
        }




        void PanelSettings()
        {
            panels.Clear();
            panels.Add(panel0);
            panels.Add(panel1);
            panels.Add(panel3);
        }

        void ConfigSize()
        {
            //Console.WriteLine($"{iLabel2.Size.Width} {this.Width}");
            this.Size = new Size(393, 169);
            this.AutoSize = false;

            int Height = panel1.Height + panel3.Height + (iButton2.Height / 2);

            if (iLabel2.Size.Width > iLabel1.Size.Width)
                this.Size = new Size(iLabel2.Size.Width + 20, Height);
            else if (iLabel2.Size.Width < iLabel1.Size.Width)
                this.Size = new Size(iLabel1.Size.Width + 20, Height);

            if (this.Size.Width < iButton2.Width)
                this.Size = new Size(200, Height);

            //Console.WriteLine($"Blob: {this.Height} | {panel3.Size}");
        }
        void CenteringControls()
        {
            //iButton1.Size = new Size(panel1.Width - 1, panel1.Height - 1);
            iButton1.Location = new Point(panel1.Width - iButton1.Height - 15, (panel1.Height - iButton1.Height) / 2);

            iLabel2.Location = new Point(10, (panel3.Height - iLabel2.Height) / 2);
            iButton2.Location = new Point((panel3.Width - iButton2.Width) / 2, panel3.Height - 35);
        }

        private void iButton1_Click(object sender, EventArgs e)
        {
            //this.Hide();
            this.Close();
        }

        private void iButton2_Click(object sender, EventArgs e)
        {
            //this.Hide();
            this.Close();
        }

        private void iButton3_Click(object sender, EventArgs e)
        {

        }


        public void Show(string Message, string Title)
        {
            try
            {
                SystemSounds.Asterisk.Play();
                iLabel1.Text = Title;
                iLabel2.Text = Message;
                ConfigSize();
                CenteringControls();
                this.ShowDialog();
            }
            catch { }
        }

        public void Show(string Message)
        {
            try
            {
                SystemSounds.Asterisk.Play();
                iLabel1.Text = "";
                iLabel2.Text = Message;
                ConfigSize();
                CenteringControls();
                this.ShowDialog();
            }
            catch { }
        }

        private void MessageBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                     (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);
                this.Update();
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
}
