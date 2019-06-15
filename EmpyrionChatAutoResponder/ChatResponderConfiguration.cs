using EmpyrionNetAPIDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmpyrionChatAutoResponder
{
    public class ChatResponderConfiguration
    {
        public LogLevel LogLevel { get; set; }
        public Dictionary<string, string> ChatCommand { get; set; } = new Dictionary<string, string>(
            new[] { new KeyValuePair<string, string>("chatcmd", "response") }.ToDictionary(K => K.Key, K => K.Value),
            StringComparer.OrdinalIgnoreCase);
    }
}