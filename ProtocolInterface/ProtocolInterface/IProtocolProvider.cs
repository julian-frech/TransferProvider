using System;
using ProtocolInterface.Models;

namespace ProtocolInterface
{
    /// <summary>
    /// Interface for all registered tranfer protocols.
    /// </summary>
    public interface IProtocolProvider
    {
        /// <summary>
        /// Trys to open connection to the target server.
        /// </summary>
        /// <param name="targetServer">Target server.</param>
        /// <returns>True: Connection could be opened. False: Connection could not be opened.</returns>
        bool OpenConnection(string targetServer);

        /// <summary>
        /// Closes the shared protocol client.
        /// </summary>
        void CloseClient();

        /// <summary>
        /// Transfers the file to the target folder on the target server.
        /// </summary>
        /// <param name="transferObject">TransferObject.</param>
        /// <returns>True: TransferObject transferred successfully. False: TransferObject not transferred.</returns>
        bool TransferFile(TransferObject transferObject);
    }

    /// <summary>
    /// Enum of available ProtocolProviders.
    /// </summary>
    public enum ProtocolProviderCollection
    {
        SftpProvider, FtpProvider, EmailProvider
    }
}

