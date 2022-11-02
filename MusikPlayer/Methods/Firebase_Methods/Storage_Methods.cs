using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusikPlayer.Methods.Firebase_Methods
{
    class Storage_Methods
    {
        public async static void UploadSong(string SongPath, string ImagePath, string PlaylistName, string SongName, string SongArtistName, bool uploadImage = false)
        {
            try
            {
                var SongStream = File.Open(SongPath, FileMode.Open);
                await Authentication.FirestoreGlobal.firebaseStorage.Child("Clients").Child(Global.client_Data.folderID).Child("Playlists").Child(PlaylistName).Child(SongArtistName).Child($"{SongName}.mp3").PutAsync(SongStream);//Song File
                if (uploadImage == true)
                {
                    var ImageStream = File.Open(ImagePath, FileMode.Open);
                    await Authentication.FirestoreGlobal.firebaseStorage.Child("Clients").Child(Global.client_Data.folderID).Child("Playlists").Child(PlaylistName).Child(SongArtistName).Child($"{SongName}.png").PutAsync(ImageStream);// Image File
                }
            }
            catch { }
        }

        public async static void DeleteSong(string PlaylistName, string SongName, string Artist)
        {
            try
            {
                await Authentication.FirestoreGlobal.firebaseStorage.Child("Clients").Child(Global.client_Data.folderID).Child("Playlists").Child(PlaylistName).Child($"{Artist}_{SongName}").Child($"{SongName}.mp3").DeleteAsync(); //Song File
                await Authentication.FirestoreGlobal.firebaseStorage.Child("Clients").Child(Global.client_Data.folderID).Child("Playlists").Child(PlaylistName).Child($"{Artist}_{SongName}").Child($"{SongName}.png").DeleteAsync();// Image File
            }
            catch { }
        }

        public static async void DeletePlaylist(string PlaylistName, string SongName, string Artist)
        {
            try
            {
                await Authentication.FirestoreGlobal.firebaseStorage.Child("Clients").Child(Global.client_Data.folderID).Child("Playlists").Child(PlaylistName).Child($"{Artist}_{SongName}").Child($"{SongName}.mp3").DeleteAsync();
                await Authentication.FirestoreGlobal.firebaseStorage.Child("Clients").Child(Global.client_Data.folderID).Child("Playlists").Child(PlaylistName).Child($"{Artist}_{SongName}").Child($"{SongName}.png").DeleteAsync();
            }
            catch { }
        }

        public static async void RenamePlaylist(string PlaylistName)
        {
            //await Authentication.FirestoreGlobal.firebaseStorage.Child("Clients").Child(Global.client_Data.folderID).Child("Playlists").Child(PlaylistName).
        }
        public void CopyFile(string sourceBucketName = "source-bucket-name", string sourceObjectName = "source-file", string destBucketName = "destination-bucket-name", string destObjectName = "destination-file-name")
        {

            var storage = StorageClient.Create();
            storage.CopyObject(sourceBucketName, sourceObjectName, destBucketName, destObjectName);

            Console.WriteLine($"Copied {sourceBucketName}/{sourceObjectName} to " + $"{destBucketName}/{destObjectName}.");
        }
    }
}
