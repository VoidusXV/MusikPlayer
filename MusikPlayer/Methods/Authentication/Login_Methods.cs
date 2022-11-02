using Firebase.Auth;
using Firebase.Storage;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MusikPlayer.Methods.Authentication
{
    class Login_Methods
    {



        /*public static WebClient webClient = new WebClient();


        public static string url = "https://www.hlde1.online/Musicedy/Server/login.php";
        private static string lastLogin = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")).ToString("dd.MM.yyyy - HH:mm:ss");
        private static string IP_Address = new WebClient().DownloadString("https://api.ipify.org");

        public static string status = null;
        public static string Response = null;

        public static void Login_Request(string Email, string Password)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebProxy myproxy = new WebProxy("zproxy.lum-superproxy.io", 22225);
            myproxy.Credentials = new NetworkCredential("lum-customer-hl_758e55cc-zone-zone1-route_err-pass_dyn", "y29oc6wsuy9a");
            request.Method = "POST";
            request.Proxy = myproxy;

            string postData = $"Email={Email}&Password={Password}&Last_Login={lastLogin}&IP_Address={IP_Address}";
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
        }

        */
        public static void Auth()
        {
            FirestoreGlobal.firestoreDb = new FirestoreDbBuilder
            {
                ProjectId = "musicedy-29653",
                JsonCredentials = Encoding.UTF8.GetString(Firebase_AuthFile.cloudfire),

            }.Build();
        }

        public async static void StorageAuth()
        {
            var firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyABl27L0bfdCdKgPyhAwq7mLj6HYbch2VM"));
            var signIn = await firebaseAuthProvider.SignInWithEmailAndPasswordAsync("sumobacchus@gmail.com", "1234678");

            FirestoreGlobal.firebaseStorage = new FirebaseStorage("musicedy-29653.appspot.com", new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(signIn.FirebaseToken),
                ThrowOnCancel = true,
            });
        }

        public static async Task<object> GetClientCredentials(string Email, string searchParam)
        {
            DocumentReference documentReference = FirestoreGlobal.firestoreDb.Collection("Users").Document(Email).Collection("data").Document("credentials");
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();
            if (documentSnapshot.Exists == false)
                return false;
            Dictionary<string, object> client_Data = documentSnapshot.ToDictionary();
            return client_Data[searchParam];
        }

        public static async Task<string> FireBaseAuth(string InputEmail, string InputPassword, bool connectToFirestore = true)
        {
            if (connectToFirestore)
                Auth();

            object CheckPassword = await GetClientCredentials(InputEmail, "Password");

            if (Equals(CheckPassword, false))
                return "Not Found";

            if (Equals(CheckPassword, InputPassword) == true)
                return "Login";
            else
                return "Wrong";

        }
    }
}
