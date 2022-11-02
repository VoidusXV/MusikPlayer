using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MusikPlayer.Methods.Authentication
{
    class Register_Methods
    {
        public static WebClient webClient = new WebClient();
        public static FirestoreDb firestoreDb;

        #region Old_RegisterMethods

        /*public static string url = "https://www.hlde1.online/Musicedy/Server/register.php";
        private static string register_date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")).ToString("dd.MM.yyyy");
        private static string IP_Address = new WebClient().DownloadString("https://api.ipify.org");

        public static string Response = null;


        public static void Register_Request(string Username, string Email, string Password)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebProxy myproxy = new WebProxy("zproxy.lum-superproxy.io", 22225);
            myproxy.Credentials = new NetworkCredential("lum-customer-hl_758e55cc-zone-zone1-route_err-pass_dyn", "y29oc6wsuy9a");
            request.Method = "POST";
            request.Proxy = myproxy;

            string postData = $"Username={Username}&Email={Email}&Password={Password}&Register_Date={register_date}&IP_Address={IP_Address}";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            Response = responseFromServer;
            //Console.WriteLine(Response);
        }

        public static async Task<string> SecureRequest(string url)
        {
            HttpClient httpClient = new HttpClient();
            //specify to use TLS 1.2 as default connection
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            httpClient.BaseAddress = new Uri(url);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            var response = httpClient.GetStringAsync(url);
            return await response;
        }*/
        #endregion

        public async static Task<List<DocumentSnapshot>> GetAllUsers(string collectionName)
        {
            CollectionReference collectionReference = firestoreDb.Collection(collectionName);
            QuerySnapshot querySnapshot = await collectionReference.GetSnapshotAsync();
            List<DocumentSnapshot> AllUsersData = querySnapshot.ToList();
            return AllUsersData;
        }

        static void Auth()
        {
            firestoreDb = new FirestoreDbBuilder
            {
                ProjectId = "musicedy-29653",
                JsonCredentials = Encoding.UTF8.GetString(Firebase_AuthFile.cloudfire),

            }.Build();
        }

        public static async void FireBase_Register(Structures.Client_Credentials client_Data)
        {
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "Username", client_Data.Username},
                { "UsernameID", client_Data.UsernameID},
                { "Password", client_Data.Password },
                { "Register_Date", client_Data.Register_Date},
                { "Last_Login", "" },
                { "IP", client_Data.IP },

            };
            Auth();

            Dictionary<string, string> emptyData = new Dictionary<string, string>();
            List<DocumentSnapshot> allUsers = await GetAllUsers("Users");

            for (int i = 0; i < allUsers.Count; i++)
            {
                if (allUsers[i].Id == $"{client_Data.Email}")
                {
                    Global.iMessageBox.Show("Der Account existiert bereits");
                    return;
                }
            }


            await firestoreDb.Collection("Users").Document($"{client_Data.Email}").Collection("data").Document("credentials").SetAsync(data); //credentials
            await firestoreDb.Collection("Users").Document($"{client_Data.Email}").Collection("data").Document("friendList").SetAsync(emptyData); //friendList
            await firestoreDb.Collection("Users").Document($"{client_Data.Email}").Collection("data").Document("blockList").SetAsync(emptyData); //blockList
            await firestoreDb.Collection("Users").Document($"{client_Data.Email}").Collection("data").Document("messages").SetAsync(emptyData);//messages


            Dictionary<string, string> userData = new Dictionary<string, string>();
            userData.Add("folderID", client_Data.folderID);
            userData.Add("Status", "Offline");
            userData.Add("Session", "");
            userData.Add("CurrentSong", "");

            await firestoreDb.Collection("Users").Document($"{client_Data.Email}").SetAsync(userData);
        }
    }
}
