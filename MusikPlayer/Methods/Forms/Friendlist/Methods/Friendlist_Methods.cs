using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MusikPlayer.Methods.Forms.Friendlist
{
    public class Friendlist_Methods
    {
        public static string Username = "";
        public static string UsernameID = "";
        public static string Type = "";

        public static string SendMessage_URL = "https://www.hlde1.online/Musicedy/Server/sendMessage.php";
        public static string ReadMessage_URL = "https://www.hlde1.online/Musicedy/Server/readMessage.php";
        public static string MoveMessage_URL = "https://www.hlde1.online/Musicedy/Server/moveMessage.php";
        public static string MessageActions_URL = "https://www.hlde1.online/Musicedy/Server/MessageAction.php";
        public static string UploadData_URL = "https://www.hlde1.online/Musicedy/Server/uploadData.php";
        public static string Response = "";


        public static int Unread_MessagesCount = 0;


        public static void SendMessage(string Type)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                             SecurityProtocolType.Tls11 |
                                                             SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SendMessage_URL);
            request.KeepAlive = false;
            request.Timeout = -1;

            WebProxy myproxy = new WebProxy("zproxy.lum-superproxy.io", 22225);
            myproxy.Credentials = new NetworkCredential("lum-customer-hl_758e55cc-zone-zone1-route_err-pass_dyn", "y29oc6wsuy9a");
            request.Method = "POST";
            request.Proxy = myproxy;

            string postData = $"Username={Global.Authentication.Username}&UsernameID={Global.Authentication.UsernameID}&TargetUsername={Username}&TargetUsernameID={UsernameID}&Type={Type}";

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

        public static string GetMessagesInfo(string Username, string UsernameID, string Type)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                   SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ReadMessage_URL);
            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;

            WebProxy myproxy = new WebProxy("zproxy.lum-superproxy.io", 22225);
            myproxy.Credentials = new NetworkCredential("lum-customer-hl_758e55cc-zone-zone1-route_err-pass_dyn", "y29oc6wsuy9a");
            request.Method = "POST";
            request.Proxy = myproxy;
            request.Timeout = -1;


            string postData = $"Username={Username}&UsernameID={UsernameID}&{Type}";

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
            return Response;
        }

        public static void MoveMessages(string Username, string UsernameID)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                             SecurityProtocolType.Tls11 |
                                                             SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(MoveMessage_URL);
            request.KeepAlive = false;
            request.Timeout = -1;

            WebProxy myproxy = new WebProxy("zproxy.lum-superproxy.io", 22225);
            myproxy.Credentials = new NetworkCredential("lum-customer-hl_758e55cc-zone-zone1-route_err-pass_dyn", "y29oc6wsuy9a");
            request.Method = "POST";
            request.Proxy = myproxy;
            string postData = $"Username={Username}&UsernameID={UsernameID}";

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

        public static void MessageActions(string Username, string UsernameID, string Type, string fileName)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                             SecurityProtocolType.Tls11 |
                                                             SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(MessageActions_URL);
            request.KeepAlive = false;

            WebProxy myproxy = new WebProxy("zproxy.lum-superproxy.io", 22225);
            myproxy.Credentials = new NetworkCredential("lum-customer-hl_758e55cc-zone-zone1-route_err-pass_dyn", "y29oc6wsuy9a");
            request.Method = "POST";
            request.Proxy = myproxy;
            string postData = $"Username={Username}&UsernameID={UsernameID}&fileName={fileName}&{Type}";

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

        public static void UploadData<T>(string fileName, T content)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                             SecurityProtocolType.Tls11 |
                                                             SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UploadData_URL);
            request.KeepAlive = false;

            WebProxy myproxy = new WebProxy("zproxy.lum-superproxy.io", 22225);
            myproxy.Credentials = new NetworkCredential("lum-customer-hl_758e55cc-zone-zone1-route_err-pass_dyn", "y29oc6wsuy9a");

            request.Timeout = -1;
            request.Method = "POST";
            request.Proxy = myproxy;
            string postData = $"Username={Global.Authentication.Username}&UsernameID={Global.Authentication.UsernameID}&fileName={fileName}&content={content}";

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
    }
}
