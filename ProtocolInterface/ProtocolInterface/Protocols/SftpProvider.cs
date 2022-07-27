using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Renci.SshNet;
using ProtocolInterface.Models;
using ProtocolInterface.Configuration;

namespace ProtocolInterface.Protocols
{
    public class SftpProvider : IProtocolProvider
    {
        private readonly ConfiguredServers _serverDetails;

        public SftpClient SharedSftpClient { get; set; }

        private readonly ILogger<SftpProvider> _logger;

        public SftpProvider(ILogger<SftpProvider> logger, IOptionsMonitor<ConfiguredServers> configuredServers)
        {
            _logger = logger;
            _serverDetails = configuredServers.CurrentValue;
        }

        /// <summary>
        /// Open a sftp client to the target machine and register for multiple file usage.
        /// </summary>
        /// <param name="targetServer">IP address of the target server.</param>
        /// <returns>True: Client to the taget successfully opened. False: Could not open client.</returns>
        public bool OpenConnection(string targetServer)
        {

            var server = _serverDetails.Servers.Where(x => x.IP == targetServer).FirstOrDefault();

            var privateKey = new PrivateKeyFile(server.KeyFile);

            SharedSftpClient = new SftpClient(new ConnectionInfo(server.IP, server.User, new PrivateKeyAuthenticationMethod(server.User, privateKey)));

            SharedSftpClient.Connect();

            return SharedSftpClient.IsConnected;
        }

        /// <summary>
        /// Closes the registered client.
        /// </summary>
        public void CloseClient()
        {
            SharedSftpClient.Disconnect();
        }

        /// <summary>
        /// Uses the registered opened client to the target machine and sends a file via SFTP protocol.
        /// </summary>
        /// <param name="transferObject">TransferObject of the collection.</param>
        /// <returns>True: File transfer successful. False: Could not transfer file.</returns>
        public bool TransferFile(TransferObject transferObject)
        {
            using var uploadFileStream = File.OpenRead(transferObject.SourceFileName);
            try
            {
                SharedSftpClient.UploadFile(uploadFileStream, Path.Combine(transferObject.TargetFolder, Path.GetFileName(transferObject.TargetFileName)), true);

                return true;
            }
            catch (Exception exc)
            {
                string errorReason = string.Format("Could not transfer {0} to {1}:{3} due to: {3}", transferObject.SourceFileName, transferObject.TargetFileName, transferObject.TargetServer, exc.Message);
                _logger.LogError(errorReason, exc);
                return false;
            }

        }

    }
}

