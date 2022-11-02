using Google.Cloud.Firestore;
using MusikPlayer.Methods.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods.Forms.Friendlist
{
    public partial class FriendSearch : Form
    {
        public FriendSearch()
        {
            InitializeComponent();
        }
        async Task<bool> isBlocked(DocumentSnapshot Emails_Snapshot, string myEmail)
        {
            DocumentReference UsersBlock_Documents = FirestoreGlobal.firestoreDb.Collection("Users").Document(Emails_Snapshot.Id).Collection("data").Document("blockList");
            DocumentSnapshot blocks_Snapshot = await UsersBlock_Documents.GetSnapshotAsync();
            Dictionary<string, object> blockList_Dict = blocks_Snapshot.ToDictionary();

            if (blockList_Dict.ContainsKey(myEmail))
                return true;

            return false;
        }

        private async void Send_Message(DocumentSnapshot Emails_Snapshot, string myEmail, string Username, string UsernameID)
        {
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                {"Type", "Friend_Request"},
                {"Username", Username},
                {"UsernameID", UsernameID},
            };
            await FirestoreGlobal.firestoreDb.Collection("Users").Document(Emails_Snapshot.Id).Collection("data").Document("messages").Collection("requests").Document(myEmail).SetAsync(data);
        }

        private async void Send_FriendRequest()
        {
            CollectionReference UsersEmail_Collection = FirestoreGlobal.firestoreDb.Collection("Users");
            QuerySnapshot UsersEmail_Snapshot = await UsersEmail_Collection.GetSnapshotAsync();

            foreach (DocumentSnapshot Emails_Snapshot in UsersEmail_Snapshot.Documents)
            {
                if (Emails_Snapshot.Id != Global.Authentication.Email)
                {
                    DocumentReference UsersEmail_Document = FirestoreGlobal.firestoreDb.Collection("Users").Document(Emails_Snapshot.Id).Collection("data").Document("credentials");
                    DocumentSnapshot credentials_Snapshot = await UsersEmail_Document.GetSnapshotAsync();
                    Dictionary<string, object> credentials_Dict = credentials_Snapshot.ToDictionary();
                    string Username = $"{credentials_Dict["Username"]}#{credentials_Dict["UsernameID"]}";


                    if (iTextBox1.Texts == Username)
                    {
                        if (await isBlocked(Emails_Snapshot, Global.Authentication.Email) == true)
                        {
                            Global.iMessageBox.Show($"Du wurdest vom Benutzer geblockt", "Fehler");
                            return;
                        }
                        Send_Message(Emails_Snapshot, Global.Authentication.Email, Global.Authentication.Username, Global.Authentication.UsernameID);
                        Global.iMessageBox.Show("Die Freundschaftsanfrage wurde versendet", "Versendet");
                        return;
                    }
                }
            }
        }

        private async void iButton24_Click(object sender, EventArgs e)
        {
            if (!iTextBox1.Texts.Contains("#") || iTextBox1.Texts != $"{Global.Authentication.Username}#{Global.Authentication.UsernameID}")
            {
                Global.iMessageBox.Show("Ungültige Eingabe", "Fehler");
                return;
            }

            Send_FriendRequest();


            #region Old
            /*if (iTextBox1.Texts.Contains("#"))
            {
                Friendlist_Methods.Username = iTextBox1.Texts.Split('#')[0];
                Friendlist_Methods.UsernameID = iTextBox1.Texts.Split('#')[1];

                string TargetClient = $"{Friendlist_Methods.Username}#{Friendlist_Methods.UsernameID}";
                string MeClient = $"{Global.Authentication.Username}#{Global.Authentication.UsernameID}";
                if (MeClient == TargetClient)
                {
                    Global.iMessageBox.Show($"Du kannst dir keine Freundschaftsanfrage senden", "Fehler");
                    return;
                }

                Friendlist_Methods.SendMessage("Friend_Request");                
                if (Friendlist_Methods.Response.Contains("Friend_Request sent"))
                {
                    Global.iMessageBox.Show("Die Freundschaftsanfrage wurde gesendet", "Gesendet");
                }
                else if (Friendlist_Methods.Response.Contains("Blocked"))
                {
                    Global.iMessageBox.Show($"Du wurdest vom Benutzer geblockt", "Fehler");
                }
                else if (Friendlist_Methods.Response.Contains("Already Friends"))
                {
                    Global.iMessageBox.Show($"Ihr seid bereits Freunde", "Fehler");
                }
                else if (Friendlist_Methods.Response.Contains("User doesnt exist"))
                {
                    Global.iMessageBox.Show("Benutzer existiert nicht", "Fehler");
                }
                //Console.WriteLine(Friendlist_Methods.Response);             
                //Console.WriteLine(JsonConvert.DeserializeObject<Friendlist_Methods.Send_Message>(Friendlist_Methods.Response).Username);
            }
            else
            {
                Global.iMessageBox.Show("Ungültige Eingabe", "Fehler");
            }*/
            #endregion
        }
    }
}
