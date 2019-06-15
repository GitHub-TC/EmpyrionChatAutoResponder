using Eleon.Modding;
using EmpyrionNetAPIAccess;
using EmpyrionNetAPIDefinitions;
using EmpyrionNetAPITools;
using System.IO;
using System.Linq;

namespace EmpyrionChatAutoResponder
{
    public class ChatResponder : EmpyrionModBase
    {
        public ChatResponder()
        {
            EmpyrionConfiguration.ModName = "EmpyrionChatAutoResponder";
        }

        public ModGameAPI DediAPI { get; private set; }
        public ConfigurationManager<ChatResponderConfiguration> Configuration { get; private set; }

        public override void Initialize(ModGameAPI dediAPI)
        {
            DediAPI = dediAPI;
            LogLevel = LogLevel.Message;

            log($"**EmpyrionChatAutoResponder: loaded");

            LoadConfiguration();
            LogLevel = Configuration.Current.LogLevel;

            Event_ChatMessage += ChatResponder_Event_ChatMessage;
        }

        private void LoadConfiguration()
        {
            Configuration = new ConfigurationManager<ChatResponderConfiguration>
            {
                ConfigFilename = Path.Combine(EmpyrionConfiguration.SaveGameModPath, @"ChatRespose.json")
            };

            Configuration.Load();
            Configuration.Save();
        }

        private async void ChatResponder_Event_ChatMessage(ChatInfo chatinfo)
        {
            if (string.IsNullOrEmpty(chatinfo.msg)) return;

            Configuration.Current.ChatCommand.TryGetValue(chatinfo.msg, out string response);

            if (response == null) response = Configuration.Current.ChatCommand.FirstOrDefault(C => chatinfo.msg.StartsWith(C.Key)).Value;
            if (string.IsNullOrEmpty(response)) return;

            await Request_ShowDialog_SinglePlayer(10 * 60000, new DialogBoxData() {
                Id              = chatinfo.playerId,
                MsgText         = response,
                PosButtonText   = "ok"
            });
        }


    }
}
