using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusikPlayer.Methods.Discord
{
    class Window
    {
        static Thread StartHashManagerThread;

        public static void StartDiscordPresence()
        {
            HashManager.discordClientId = "936641009956311092";
            StartHashManagerThread = new Thread(new ThreadStart(HashManager.HashId));
            StartHashManagerThread.Start();
        }
        public void ShutdownPresence()
        {

            Discord.PresenceManager.client.Dispose();
            Discord.PresenceManager.discordPresenceDetail = string.Empty;
            Discord.PresenceManager.discordPresenceState = string.Empty;
            Discord.PresenceManager.discordLargeImageKey = string.Empty;
            Discord.PresenceManager.discordLargeImageText = string.Empty;
            Discord.PresenceManager.discordSmallImageKey = string.Empty;
            Discord.PresenceManager.discordSmallImageText = string.Empty;
        }
        public static void LoadUserStatePresence()
        {
            StartDiscordPresence();
            Discord.PresenceManager.discordLargeImageKey = "";

            //Discord.PresenceManager.UpdatePresence();
            //Discord.HashManager.discordClientId = "488088477343285258";
            //Discord.PresenceManager.discordPresenceDetail = "Game Selection";
        }



    }

}
