using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods.Authentication
{
    public partial class Register : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        public Register()
        {
            InitializeComponent();
            iLabel1.Location = new Point((panel1.Width - iLabel1.Width) / 2, (panel1.Height - iLabel1.Height) / 2);
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 5, 5));
            this.Opacity = 0.1f;
            FormFade.Start();
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

        private void iButton22_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        private void iButton24_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void iButton23_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iButton21_Click(object sender, EventArgs e)
        {
            string Username = iTextBox1.Texts.Replace("\"", "").Replace("'", "");
            string Password = iTextBox2.Texts.Replace("\"", "").Replace("'", "");
            string Email = iTextBox4.Texts.Replace("\"", "").Replace("'", "");

            iTextBox1.BorderColor = Color.Gray;
            iTextBox2.BorderColor = Color.Gray;
            iTextBox3.BorderColor = Color.Gray;
            iCheckBox2.BackColor = Color.White;
            iLabel7.Visible = false;
            Random random = new Random();

            Structures.Client_Credentials client_Data = new Structures.Client_Credentials();

            client_Data.Username = Username;
            client_Data.UsernameID = random.Next(0, 100000).ToString();
            client_Data.Password = Password;
            client_Data.Email = Email;
            client_Data.IP = new WebClient().DownloadString("https://api.ipify.org");
            client_Data.Register_Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")).ToString("dd.MM.yyyy");
            client_Data.Last_Login = "";
            client_Data.folderID = Guid.NewGuid().ToString();


            if (iTextBox1.Texts.Length < 4)
            {
                iLabel7.Text = "Der Benutzername benötigt mindestens 4 Zeichen";
                iLabel7.Visible = true;
                iTextBox1.BorderColor = Color.Red;
                return;
            }
            else if (iTextBox2.Texts.Length < 8)
            {
                iLabel7.Text = "Das Passwort benötigt mindestens 8 Zeichen";
                iLabel7.Visible = true;
                iTextBox2.BorderColor = Color.Red;
                return;
            }
            else if (iTextBox2.Texts != iTextBox3.Texts)
            {
                iLabel7.Text = "Das Passwort stimmt nicht überein";
                iLabel7.Visible = true;
                iTextBox3.BorderColor = Color.Red;
                return;
            }
            else if (iCheckBox2.Checked == false)
            {
                iLabel7.Text = "Stimmen Sie den Nutzungsbedinungen zu";
                iLabel7.Visible = true;
                iCheckBox2.BackColor = Color.Red;
                return;
            }
            Register_Methods.FireBase_Register(client_Data);

            Global.iMessageBox.Show("Du wurdest erfolgreich registiert");
            Login login = new Login();
            login.Show();
            this.Hide();

            #region Old_RegisterMethod
            /* Register_Methods.Register_Request(Username, Email, Password);
             Console.WriteLine(Register_Methods.Response);
             if(Register_Methods.Response.Contains("Account exists"))
             {
                 Global.iMessageBox.Show("Der Account existiert bereits");
             }
             else if (Register_Methods.Response.Contains("created"))
             {
                 Global.iMessageBox.Show("Du wurdest erfolgreich registiert");
                 Login login = new Login();
                 login.Show();
                 this.Hide();
             }
             else if (Register_Methods.Response.Contains("Something went wrong"))
             {
                 Global.iMessageBox.Show("Ein unbekannter Fehler ist aufgetreten");
             }*/
            #endregion
        }

        private void FormFade_Tick(object sender, EventArgs e)
        {
            if (this.Opacity <= 0.99f)
            {
                this.Opacity += 0.1f;
            }
            else
            {
                FormFade.Stop();
            }
        }
    }
}
