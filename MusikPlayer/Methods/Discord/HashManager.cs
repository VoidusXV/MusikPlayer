using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MusikPlayer.Methods.Discord
{
    public class HashManager
    {
        public static PresenceManager presenceManager = new PresenceManager();
        public static string discordClientId { get; set; }
        public static void HashId()
        {
            using (SHA256 sha256hash = SHA256.Create())
            {
                string createHash = GetHash(sha256hash, discordClientId);
                VerifyClientIdHash(createHash);
                Console.WriteLine("HashId:" + createHash);
            }
        }

        public static void VerifyClientIdHash(string discordClientIDHash)
        {
            using (SHA256 sha256hash = SHA256.Create())
            {
                if (VerifyHash(sha256hash, discordClientId, discordClientIDHash))
                {
                    Console.WriteLine("InitializeDiscordRPC");

                    presenceManager.InitializeDiscordRPC(discordClientId);
                }
                else
                {
                    Global.iMessageBox.Show("An error has occured while validating hash.", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title);
                }
            }
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }


        // Verify a hash against a string.
        private static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
        {
            // Hash the input.
            var hashOfInput = GetHash(hashAlgorithm, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }

}
