using Google.Cloud.Firestore;
using MusikPlayer.Methods.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods.Partymode
{
    class PartyMode_Methods
    {
        private static FirestoreChangeListener firestoreClosed_Listener;
        public static async Task CheckSessionIsClosed(string mySessionID, Form form)
        {
            try
            {
                DocumentReference docRef = FirestoreGlobal.firestoreDb.Collection("Sessions").Document(mySessionID); //mySessionID
                DocumentSnapshot documentSnapshot;
                firestoreClosed_Listener = docRef.Listen(async snapshot =>
                {
                    documentSnapshot = await docRef.GetSnapshotAsync();
                    if (documentSnapshot.Exists == false)
                    {
                        await firestoreClosed_Listener.StopAsync();
                        Global.iMessageBox.Show("Der Raum wurde vom Host beendet", "Beendet");
                        Global.PartyMode = false;
                        form.Hide();
                    }

                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static async Task SetOptionSongPlayer_WhenJoined()
        {
            Form1 form1 = Application.OpenForms.OfType<Form1>().Single();

            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "SongPosition", form1.iTrackBar1.Value }
            };

            await FirestoreGlobal.firestoreDb.Collection("Sessions").Document(Global.PartyMode_SessionID).SetAsync(data, SetOptions.MergeAll);
        }

    }
}
