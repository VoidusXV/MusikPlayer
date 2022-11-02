using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordRPC;
using DiscordRPC.Message;

namespace MusikPlayer.Methods.Discord
{
    public class PresenceManager
    {
        public static string discordClientId { get; set; }
        public static string discordPresenceState { get; set; }
        public static string discordPresenceDetail { get; set; }
        public static string discordLargeImageKey { get; set; }
        public static string discordLargeImageText { get; set; }
        public static string discordSmallImageKey { get; set; }
        public static string discordSmallImageText { get; set; }
        public static bool useTimeStamp { get; set; }

        // DiscordRPC.Core Library
        public static DiscordRpcClient client;

        // Debug only
        static string TAG = "PresenceManager: ";

        // Classes
        public void InitializeDiscordRPC(string ClientID)
        {

            Console.WriteLine(TAG + "Starting Discord Presence");

            client = new DiscordRpcClient(ClientID);
            client.Initialize();
            client.OnReady += OnClientReady;
            client.OnConnectionFailed += OnConnectionFailed;
            client.OnConnectionEstablished += OnConnectionEstablished;
        }
        private void OnClientReady(object sender, ReadyMessage args)
        {
#if DEBUG
            Debug.WriteLine("Received Ready from user {0}", args.User);
#else
#endif
            JsonConfig.settings.discordUsername = args.User.ToString();
            JsonConfig.settings.discordAvatarUri = args.User.GetAvatarURL(User.AvatarFormat.PNG, User.AvatarSize.x128);
            JsonConfig.SaveJson();


        }
        private void OnConnectionFailed(object sender, ConnectionFailedMessage args)
        {

        }
        private void OnConnectionEstablished(object sender, ConnectionEstablishedMessage args)
        {
            client.SetPresence(new RichPresence()
            {
                Details = JsonConfig.settings.discordPresenceDetail,
                State = JsonConfig.settings.discordPresenceState,

                Assets = new Assets()
                {
                    LargeImageKey = JsonConfig.settings.discordLargeImageKey,
                    LargeImageText = JsonConfig.settings.discordLargeImageText,
                    SmallImageKey = JsonConfig.settings.discordSmallImageKey,
                    SmallImageText = JsonConfig.settings.discordSmallImageText,
                }
            });
            client.Invoke();
        }

        public static void UpdatePresence()
        {
            try
            {
                if (client == null)
                    return;

                if (!useTimeStamp)
                {
                    client.SetPresence(new RichPresence()
                    {
                        Details = discordPresenceDetail,
                        State = discordPresenceState,
                        Timestamps = null,

                        Assets = new Assets()
                        {
                            LargeImageKey = discordLargeImageKey,
                            LargeImageText = discordLargeImageText,
                            SmallImageKey = discordSmallImageKey,
                            SmallImageText = discordSmallImageText
                        }
                    });
                    client.Invoke();
                }
                else
                {
                    client.SetPresence(new RichPresence()
                    {
                        Details = discordPresenceDetail,
                        State = discordPresenceState,
                        Timestamps = Timestamps.Now,

                        Assets = new Assets()
                        {
                            LargeImageKey = discordLargeImageKey,
                            LargeImageText = discordLargeImageText,
                            SmallImageKey = discordSmallImageKey,
                            SmallImageText = discordSmallImageText
                        }
                    });
                    client.Invoke();
                }
            }
            catch { }
        }

        public void ShutdownPresence()
        {
            client.Dispose();
            discordPresenceDetail = string.Empty;
            discordPresenceState = string.Empty;
            discordLargeImageKey = string.Empty;
            discordLargeImageText = string.Empty;
            discordSmallImageKey = string.Empty;
            discordSmallImageText = string.Empty;
        }
    }

}
