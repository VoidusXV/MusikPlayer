using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MusikPlayer.Methods.Forms
{
    public partial class CreatePlaylist : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        public CreatePlaylist()
        {
            InitializeComponent();
            //Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 5, 5));

            this.Opacity = .5;
            OpeningTransitionTimer.Start();
        }

        #region RoundedBorder
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
     (
         int nLeftRect,     // x-coordinate of upper-left corner
         int nTopRect,      // y-coordinate of upper-left corner
         int nRightRect,    // x-coordinate of lower-right corner
         int nBottomRect,   // y-coordinate of lower-right corner
         int nWidthEllipse, // width of ellipse
         int nHeightEllipse // height of ellipse
     );
        #endregion
        private void OpeningTransitionTimer_Tick(object sender, EventArgs e)
        {
            if (Opacity >= 1)
                OpeningTransitionTimer.Stop();

            this.Opacity += .05;

        }

        private void iButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void iButton2_Click(object sender, EventArgs e)
        {
            var form1 = Application.OpenForms.OfType<Form1>().Single();
            if (!(string.IsNullOrWhiteSpace(iTextBox1.Texts) || string.IsNullOrEmpty(iTextBox1.Texts)))
            {
                int PlaylistLength = DataHandling.Handler.ReturnJsonFile<Methods.DataHandling.Playlist>("Playlists.json").Count;

                CreateControls.Create_iLabel(CreateControls.iLabels, PlaylistLength, iTextBox1.Texts, new Font("Microsoft Sans Serif", 10, FontStyle.Regular), new Point(0, 0 + (CreateControls.count * 35)),
                   new Size(form1.panel6.Width, 50), false, true);


                string Date = DateTime.Now.ToString("MM/dd/yyyy");
                string Time = DateTime.Now.ToString("HH:mm");
                int Count = DataHandling.Methods.GetJsonCount("Playlists.json");
                string PlaylistPath = Global.current_path + $"/Playlists/{iTextBox1.Texts}";
                DataHandling.Handler.CreateJsonFile_Playlist(Count, Count, Date, Time, iTextBox1.Texts, PlaylistPath, $"{PlaylistPath}/{iTextBox1.Texts}.json");
                if (!Directory.Exists(Global.current_path + "/Playlists"))
                    Directory.CreateDirectory(Global.current_path + "/Playlists");
                Directory.CreateDirectory(Global.current_path + $"/Playlists/{iTextBox1.Texts}");

                form1.panel6.Controls.Clear();
                Global.ReadPlaylists(form1.panel6, CreateControls.iLabels);

                form1.Remove_PlaylistLabel_Controls();
                form1.Add_PlaylistLabel_Controls();

                Console.WriteLine("Created");
                this.Hide();
            }
            else
            {
                Global.iMessageBox.Show("Please enter a valid Name", "Error");
            }
        }

        private void CreatePlaylist_Load(object sender, EventArgs e)
        {

        }

        private void iTextBox1__TextChanged(object sender, EventArgs e)
        {

        }

        private void iPanel2_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void iPanel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                     (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);
                this.Update();
            }

        }

        private void iPanel2_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
}
