using Firebase.Storage;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusikPlayer.Methods.Authentication
{
    class FirestoreGlobal
    {
        public static FirestoreDb firestoreDb;

        public static FirestoreChangeListener firestoreListener;

        public static FirebaseStorage firebaseStorage;
        public static void ListenFriendlist()
        {
            DocumentReference docRef = firestoreDb.Collection("Users").Document("Bla@gmail.com");
            firestoreListener = docRef.Listen(snapshot =>
            {
                Console.WriteLine("Callback received document snapshot.");
                Console.WriteLine("Document exists? {0}", snapshot.Exists);
                if (snapshot.Exists)
                {
                    Console.WriteLine("Document data for {0} document:", snapshot.Id);
                    Dictionary<string, object> fields = snapshot.ToDictionary();
                    foreach (KeyValuePair<string, object> pair in fields)
                    {
                        Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
                    }
                }
            });

            //await firestoreListener.StopAsync();
        }

        public static async Task StopListening()
        {
            await firestoreListener.StopAsync();
        }

    }
}
