using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusikPlayer.Methods.Forms.Friendlist
{
    public class Friends_Handler
    {
        public class Send_Message
        {
            public string Username { get; set; }
            public string UsernameID { get; set; }
            public string Type { get; set; }
        }

        public class Client_Data
        {
            public string Username { get; set; }
            public string UsernameID { get; set; }
            public string Status { get; set; }
            public Image profileImage { get; set; }
        }


        public static string Custom_JsonParser(string text)
        {
            text = text.Insert(0, "[");
            string[] spliText = text.Split('}');
            string EndText = "";

            for (int i = 0; i < spliText.Length - 1; i++)
            {
                spliText[i] = spliText[i].Insert(spliText[i].Length, "},");
                EndText += spliText[i];
            }
            EndText += "]";
            return EndText;
        }
    }
}
