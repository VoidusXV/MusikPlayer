﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusikPlayer.Methods.Discord
{
    class GetDiscordProcess
    {
        private string discordBuildInfo;
        private bool isDiscordRunning = false;

        public bool IsDiscordRunning
        {
            get { return isDiscordRunning; }
        }

        public string DiscordBuildInfo
        {
            get { return discordBuildInfo; }
        }

        public void DiscordProcessName()
        {

            Process[] DiscordStableProcess = Process.GetProcessesByName("Discord");
            Process[] DiscordPTBProcess = Process.GetProcessesByName("DiscordPTB");
            try
            {

                if (DiscordStableProcess.Length > 0 || DiscordPTBProcess.Length > 0)
                {

                    if (DiscordPTBProcess.Length > 0)
                    {
                        Debug.WriteLine("User is running Discord PTB build");
                        discordBuildInfo = "Public Test Beta (PTB)";
                        isDiscordRunning = true;
                    }
                    else
                    {
                        Debug.WriteLine("User is running Discord STABLE build");
                        discordBuildInfo = "Stable";
                        isDiscordRunning = true;
                    }

                }
                else
                {
                    isDiscordRunning = false;
                }

            }
            catch (Exception exception)
            {
                //XtraMessageBox.Show(exception.ToString(), Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title);
                Debug.WriteLine(exception.Message);
            }
        }
    }

}
