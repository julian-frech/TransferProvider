using System;
namespace ProtocolInterface.Models
{
    /// <summary>
    /// Object of the TransferService. Gathers necessary information for the report to be transferred i.e.
    /// file name, transfer server, transfer protocol, status of processing, ...
    /// </summary>
    public class TransferObject
    {
        /// <summary>
        /// HashCode of Filename + TargetServer as key.
        /// </summary>
        public long HashKey { get; set; }

        /// <summary>
        /// Full path plus name of the file to be transferred.
        /// </summary>
        public string SourceFileName { get; set; }

        /// <summary>
        /// Name of the target file.
        /// </summary>
        public string TargetFileName { get; set; }

        /// <summary>
        /// Target server where it should be transferred to.
        /// </summary>
        public string TargetServer { get; set; }

        /// <summary>
        /// Folder name on the target server.
        /// </summary>
        public string TargetFolder { get; set; }

        /// <summary>
        /// Target email address.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Whether the file should be sent when a specific time has come.
        /// </summary>
        public bool TimeTriggered { get; set; } = false;

        /// <summary>
        /// Time when the report should be sent.
        /// </summary>
        public string TransferTime { get; set; }

        /// <summary>
        /// Whether the file should be archived.
        /// </summary>
        public bool Archive { get; set; } = false;

        /// <summary>
        /// Where the file should be archived.
        /// </summary>
        public string ArchiveTarget { get; set; }

        /// <summary>
        /// Error details in case the transfer failed.
        /// </summary>
        public string ? ErrorDetails { get; set; }

        /// <summary>
        /// TransferProtocol used for the TransferObject.
        /// I.e. SFTP (default), FTP, OFTP, ...
        /// </summary>
        public TransferProtocol UsedProtocol { get; set; } = TransferProtocol.SFTP;

        /// <summary>
        /// Process status of the ITransferObject.
        /// Possible are Initial for just added to the collection of TransferObjects, Processing for
        /// process just retrieved this TransferObject from the collection, Error for retrieved and failed as well as
        /// Done for retrieved and transfer completed successfully.
        /// </summary>
        public ProcessStatus Status { get; set; } = ProcessStatus.Initial;

        /// <summary>
        /// When the TransferObject has been added to the collection.
        /// </summary>
        public DateTime TransferInitialized { get; set; } = DateTime.Now;

        /// <summary>
        /// When the TransferObject is transferred successful.
        /// </summary>
        public DateTime TransferEnd { get; set; }

        public TransferObject(long hashKey, string sourceFileName, string targetFileName, string targetFolder, string emailAddress, string targetServer, bool timeTriggered, string transferTime, bool archive, string archiveTarget, string protocol)
        {
            TransferInitialized = DateTime.Now;
            TargetFileName = targetFileName;
            SourceFileName = sourceFileName;
            TargetFolder = targetFolder;
            EmailAddress = emailAddress;
            TargetServer = targetServer;
            HashKey = hashKey;
            TimeTriggered = timeTriggered;
            TransferTime = transferTime;
            Archive = archive;
            ArchiveTarget = archiveTarget;
            UsedProtocol = (TransferProtocol)Enum.Parse(typeof(TransferProtocol), protocol);
        }

    }

    /// <summary>
    /// Enumerator of possible transfer protocols to be used for transferring the TransferObject.
    /// </summary>
    public enum TransferProtocol
    {
        SFTP = 1,//DONE
        FTPS = 2,
        FTP = 3,
        OFTP = 4,
        PeSIT = 5,
        SCP = 6,
        AS2 = 7,
        HTTP = 8,
        HTTPS = 9,
        Email = 10
    }

    /// <summary>
    /// Status of the TransferObject. Possible are Initial for just added to the collection of TransferObjects, Processing for
    /// process just retrieved this TransferObject from the collection, Error for retrieved and failed as well as
    /// Done for retrieved and transfer completed successfully.
    /// </summary>
    public enum ProcessStatus
    {
        FileWait,
        TimeWait,
        Initial,
        Processing,
        Error,
        Done,
        Removed
    }
}

