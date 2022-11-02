using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods
{
    public class Client_DataHandling
    {
        public static byte[] profileImage = null;

        public static async void GetProfileImage(PictureBox pictureBox)
        {
            try
            {
                if (Authentication.FirestoreGlobal.firebaseStorage == null)
                {
                    pictureBox.Image = Resources_Images.Images.Icons.profileImage_Placeholder;
                    return;
                }
                var storageChild = Authentication.FirestoreGlobal.firebaseStorage.Child("Clients").Child(Global.client_Data.folderID).Child("profileImage.png");
                Client_DataHandling.profileImage = new WebClient().DownloadData(await storageChild.GetDownloadUrlAsync());
                pictureBox.Image = Image.FromStream(new MemoryStream(Client_DataHandling.profileImage));
            }
            catch
            {
                profileImage = null;
            }
            finally
            {
                if (profileImage == null)
                    pictureBox.Image = Resources_Images.Images.Icons.profileImage_Placeholder;
            }
        }
    }
}
