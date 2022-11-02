using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusikPlayer.Methods
{
    class Multithreading
    {

        public static Thread Background_UploadData_Thread = new Thread(Background_Upload_MetaData);

        public static void Background_Upload_MetaData()
        {
            //Console.WriteLine("Uploading...");
            Forms.Friendlist.Friendlist_Methods.UploadData<string>("data.json", File.ReadAllText("data.json"));
            //Console.WriteLine("Done...");
            Background_UploadData_Thread.Abort();
        }
    }
}
