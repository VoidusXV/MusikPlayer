using Google.Cloud.Firestore;
using MusikPlayer.Methods.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods.Forms
{
    public partial class Auth_Placeholder : Form
    {
        public Auth_Placeholder()
        {
            this.Visible = false;
            OpenMain();
            InitializeComponent();
        }
        private async void OpenMain()
        {
            try
            {
                Login_Methods.Auth();
                DocumentSnapshot serverSnapshot = await FirestoreGlobal.firestoreDb.Collection("Server").Document("Server").GetSnapshotAsync();
                string version = serverSnapshot.ToDictionary()["Version"].ToString();
                //Console.WriteLine(version);

                //WebProxy myproxy = new WebProxy("zproxy.lum-superproxy.io", 22225);
                //myproxy.Credentials = new NetworkCredential("lum-customer-hl_758e55cc-zone-zone1-route_err-pass_dyn", "y29oc6wsuy9a");
                //WebClient webClient = new WebClient();
                //webClient.Proxy = myproxy;
                //string version = webClient.DownloadString("https://www.hlde1.online/Musicedy/Server/update.php");

                if (!Assembly.GetExecutingAssembly().GetName().Version.ToString().Contains(version))
                {
                    Updater.UpdateWarning updateWarning = new Updater.UpdateWarning();
                    updateWarning.Show();
                    return;
                }

                Global.SetHandlerVariables();

                if (Global.Authentication.Email == null || Global.Authentication.Password == null)
                {
                    Login login = new Login();
                    login.Show();
                    this.Hide();
                    return;
                }

                string Email = Global.Authentication.Email.Replace("\"", "").Replace("'", "");
                string Password = Global.Authentication.Password.Replace("\"", "").Replace("'", "");
                string result = await Login_Methods.FireBaseAuth(Email, Password, false);

                if (Global.ProfileSettings.AutoLogin == true && result == "Login") // Auto Login
                {
                    DocumentReference UsersEmail_Document = FirestoreGlobal.firestoreDb.Collection("Users").Document(Email).Collection("data").Document("credentials");
                    DocumentSnapshot credentials_Snapshot = await UsersEmail_Document.GetSnapshotAsync();
                    Dictionary<string, object> credentials_Dict = credentials_Snapshot.ToDictionary();
                    string Username = credentials_Dict["Username"].ToString();
                    string UsernameID = credentials_Dict["UsernameID"].ToString();
                    DataHandling.Handler.CreateJsonFile_Authentication(Username, UsernameID, Email, Password);
                    GetClientData(Email);
                    Login_Methods.StorageAuth();
                    Console.WriteLine("Login");
                    Form1 form1 = new Form1();
                    form1.Show();
                    this.Hide();

                }
                else if (Global.ProfileSettings.RememberMe == true && Global.ProfileSettings.AutoLogin == false)
                {
                    Login login = new Login();

                    login.iTextBox1.RemovePlaceholder();
                    login.iTextBox2.RemovePlaceholder();
                    login.iTextBox1.Texts = Global.Authentication.Email;
                    login.iTextBox2.Texts = Global.Authentication.Password;
                    login.Show();
                    this.Hide();

                }
                else
                {
                    Login login = new Login();
                    login.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                Global.Error_Debugger(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }

        private async void GetClientData(string Email)
        {
            DocumentReference UsersInfo_Document = FirestoreGlobal.firestoreDb.Collection("Users").Document(Email);
            DocumentSnapshot UserInfo_Snapshot = await UsersInfo_Document.GetSnapshotAsync();
            Dictionary<string, object> UserInfo_Dict = UserInfo_Snapshot.ToDictionary();
            Global.client_Data.folderID = UserInfo_Dict["folderID"].ToString();

            DocumentReference UsersCredentials_Document = FirestoreGlobal.firestoreDb.Collection("Users").Document(Email).Collection("data").Document("credentials");
            DocumentSnapshot UsersCredentials_Snapshot = await UsersCredentials_Document.GetSnapshotAsync();
            Dictionary<string, object> credentials_Dict = UsersCredentials_Snapshot.ToDictionary();
            Global.client_Data.Register_Date = credentials_Dict["Register_Date"].ToString();

        }
    }
}
