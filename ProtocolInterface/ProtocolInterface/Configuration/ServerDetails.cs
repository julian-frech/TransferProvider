using System;
namespace ProtocolInterface.Configuration
{
    public class ServerDetails
    {
        public const string Server = "ServerDetails";

        public string IP { get; set; }
        public string Alias { get; set; }
        public string User { get; set; }
        public string KeyFile { get; set; }

    }

    public class ConfiguredServers
    {
        public List<ServerDetails> Servers { get; set; }
    }
}

