using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MusikPlayer.Methods;
using MusikPlayer.Methods.Authentication;

namespace MusikPlayer
{
    internal static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppDomain currentDomain = default(AppDomain);
            currentDomain = AppDomain.CurrentDomain;
            // Handler for unhandled exceptions.
            currentDomain.UnhandledException += GlobalUnhandledExceptionHandler;
            // Handler for exceptions in threads behind forms.
            Application.ThreadException += GlobalThreadExceptionHandler;

            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Application.Run(new Methods.Forms.Auth_Placeholder());
            //OpenMain();
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var currentAssyembly = Assembly.GetExecutingAssembly();
            var requiredDllName = $"{(new AssemblyName(args.Name).Name)}.dll";
            var resource = currentAssyembly.GetManifestResourceNames().Where(s => s.EndsWith(requiredDllName)).FirstOrDefault();

            if (resource == null)
            {
                using (var stream = currentAssyembly.GetManifestResourceStream(resource))
                {
                    if (stream == null)
                    {
                        return null;
                    }
                    var block = new byte[stream.Length];
                    stream.Read(block, 0, block.Length);
                    return Assembly.Load(block);
                }
            }
            else
            {
                return null;
            }
        }

        private static void OpenMain()
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
                string result = "";
                Thread thread = new Thread(() =>
                {
                    result = Login_Methods.FireBaseAuth(Email, Password).Result;
                    Console.WriteLine($"{result}");
                });

                result = Login_Methods.FireBaseAuth(Email, Password).Result;

                if (Global.ProfileSettings.AutoLogin == true && result == "Login") // Auto Login
                {
                    string Username = "";
                    string UsernameID = "";
                    Methods.DataHandling.Handler.CreateJsonFile_Authentication(Username, UsernameID, Email, Password);
                    Console.WriteLine("Login");
                    Application.Run(new Form1());
                }
                else if (Global.ProfileSettings.RememberMe == true && Global.ProfileSettings.AutoLogin == false)
                {
                    Login login = new Login();
                    login.iTextBox1.RemovePlaceholder();
                    login.iTextBox2.RemovePlaceholder();
                    login.iTextBox1.Texts = Global.Authentication.Email;
                    login.iTextBox2.Texts = Global.Authentication.Password;
                    Application.Run(login);
                }
                else
                {
                    Application.Run(new Methods.Authentication.Login());
                }

                #region OLD_SQL
                /*Methods.Authentication.Login_Methods.Login_Request(Email, Password);

                string Username = Methods.Authentication.Login_Methods.Response.Split('\n')[1];
                string UsernameID = Methods.Authentication.Login_Methods.Response.Split('\n')[2];

                bool loginCheck = Methods.Authentication.Login_Methods.Response.Contains("Login Successful");
                if (Global.ProfileSettings.AutoLogin == true && loginCheck == true) // Auto Login
                {
                    Methods.DataHandling.Handler.CreateJsonFile_Authentication(Username, UsernameID, Email, Password);
                    Global.Register_Date = Methods.Authentication.Login_Methods.Response.Split('\n')[3];
                    Console.WriteLine("Login");
                    Application.Run(new Form1());
                }
                else if (Global.ProfileSettings.RememberMe == true && Global.ProfileSettings.AutoLogin == false)
                {
                    Methods.Authentication.Login login = new Methods.Authentication.Login();
                    login.iTextBox1.RemovePlaceholder();
                    login.iTextBox2.RemovePlaceholder();
                    login.iTextBox1.Texts = Global.Authentication.Email;
                    login.iTextBox2.Texts = Global.Authentication.Password;
                    Application.Run(login);
                }
                else
                {
                    Application.Run(new Methods.Authentication.Login());
                }*/
                #endregion
            }
            catch (Exception ex)
            {
                Global.Error_Debugger(ex.Message);
                Console.WriteLine(ex.Message);
                Application.Run(new Login());
            }
        }

        private static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = default(Exception);
            ex = (Exception)e.ExceptionObject;
            Global.Error_Debugger(ex.Message);

        }

        private static void GlobalThreadExceptionHandler(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = default(Exception);
            ex = e.Exception;
            Global.Error_Debugger(ex.Message);
        }
    }
}
