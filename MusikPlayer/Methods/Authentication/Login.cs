using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods.Authentication
{
    public partial class Login : Form
    {
        private bool mouseDown;
        private Point lastLocation;

        public Login()
        {
            //OpenMain();
            InitializeComponent();

            iLabel5.Location = new Point((panel1.Width - iLabel5.Width) / 2, (panel1.Height - iLabel5.Height) / 2);
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
            this.WindowState = FormWindowState.Minimized;
        }

        private void iButton21_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iButton24_Click(object sender, EventArgs e)
        {
            this.Hide();
            Register register = new Register();
            register.Show();
        }

        private async void iButton23_Click(object sender, EventArgs e)
        {
            try
            {
                iTextBox1.BorderColor = Color.Gray;
                iTextBox2.BorderColor = Color.Gray;
                iLabel7.Visible = false;

                string Email = iTextBox1.Texts.Replace("\"", "").Replace("'", "");
                string Password = iTextBox2.Texts.Replace("\"", "").Replace("'", "");

                #region SQL
                /*Login_Methods.Login_Request(Email, Password);
                if (Login_Methods.Response.Contains("Login Successful"))
                {
                    string Username = Login_Methods.Response.Split('\n')[1];
                    string UsernameID = Login_Methods.Response.Split('\n')[2];
                    Global.Register_Date = Login_Methods.Response.Split('\n')[3];
                    DataHandling.Handler.CreateJsonFile_ProfileSettings(iCheckBox1.Checked, iCheckBox2.Checked);
                    DataHandling.Handler.CreateJsonFile_Authentication(Username, UsernameID, iTextBox1.Texts, iTextBox2.Texts);
                    Form1 form1 = new Form1();
                    form1.Show();
                    this.Hide();
                }
                else if (Login_Methods.Response.Contains("Email or Password is invalid!"))
                {
                    iTextBox1.BorderColor = Color.Red;
                    iTextBox2.BorderColor = Color.Red;
                    iLabel7.Text = "Benutzername oder Passwort ist falsch";
                    iLabel7.Visible = true;
                }
                else
                {
                    iLabel7.Text = "Unbekannter Fehler";
                    iLabel7.Visible = true;
                }*/
                #endregion

                string result = await Login_Methods.FireBaseAuth(Email, Password);
                if (result == "Login")
                {
                    string Username = "";//Login_Methods.Response.Split('\n')[1];
                    string UsernameID = ""; //Login_Methods.Response.Split('\n')[2];
                    //Global.Register_Date = "";//Login_Methods.Response.Split('\n')[3];

                    DataHandling.Handler.CreateJsonFile_ProfileSettings(iCheckBox1.Checked, iCheckBox2.Checked);
                    DataHandling.Handler.CreateJsonFile_Authentication(Username, UsernameID, iTextBox1.Texts, iTextBox2.Texts);
                    Form1 form1 = new Form1();
                    form1.Show();
                    this.Hide();
                }
                else if (result == "Wrong")
                {
                    iTextBox1.BorderColor = Color.Red;
                    iTextBox2.BorderColor = Color.Red;
                    iLabel7.Text = "Benutzername oder Passwort ist falsch";
                    iLabel7.Visible = true;
                }
                else if (result == "Not Found")
                {
                    iTextBox1.BorderColor = Color.Red;
                    iTextBox2.BorderColor = Color.Red;
                    iLabel7.Text = "Die Email-Addresse existiert nicht";
                    iLabel7.Visible = true;
                }
                else
                {
                    iLabel7.Text = "Unbekannter Fehler";
                    iLabel7.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Global.iMessageBox.Show("Ein unbekannter Fehler ist aufgetreten", "Fehler");
            }
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



        private async void OpenMain()
        {
            try
            {

                WebProxy myproxy = new WebProxy("zproxy.lum-superproxy.io", 22225);
                myproxy.Credentials = new NetworkCredential("lum-customer-hl_758e55cc-zone-zone1-route_err-pass_dyn", "y29oc6wsuy9a");
                WebClient webClient = new WebClient();
                webClient.Proxy = myproxy;
                string version = webClient.DownloadString("https://www.hlde1.online/Musicedy/Server/update.php");

                if (!Assembly.GetExecutingAssembly().GetName().Version.ToString().Contains(version))
                {
                    Application.Run(new Methods.Updater.UpdateWarning());
                    return;
                }

                Global.SetHandlerVariables();

                string Email = Global.Authentication.Email.Replace("\"", "").Replace("'", "");
                string Password = Global.Authentication.Password.Replace("\"", "").Replace("'", "");
                string result = await Login_Methods.FireBaseAuth(Email, Password);

                if (Global.ProfileSettings.AutoLogin == true && result == "Login") // Auto Login
                {
                    string Username = "";
                    string UsernameID = "";
                    DataHandling.Handler.CreateJsonFile_Authentication(Username, UsernameID, Email, Password);
                    //Global.Register_Date = "";
                    Console.WriteLine("Login");
                    Form1 form1 = new Form1();
                    form1.Show();
                    this.Hide();
                }
                else if (Global.ProfileSettings.RememberMe == true && Global.ProfileSettings.AutoLogin == false)
                {
                    iTextBox1.RemovePlaceholder();
                    iTextBox2.RemovePlaceholder();
                    iTextBox1.Texts = Global.Authentication.Email;
                    iTextBox2.Texts = Global.Authentication.Password;
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Global.Error_Debugger(ex.Message);
                Console.WriteLine(ex.Message);
                //Application.Run(new Login());
            }
        }

    }
}
