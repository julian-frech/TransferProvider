using System;
namespace ProtocolInterface.Configuration
{
    public class TransferSettings
    {
        public const string TransferSetting = "TransferSetting";

        public string ReportFolder { get; set; }

        public int SleepPeriodSeconds { get; set; }

        public string ArchiveFolder { get; set; }

        public string EmailHost { get; set; }

        public string Email { get; set; }

        public int EmailHostPort { get; set; }

        public string EmailSubject { get; set; }

        public int EmailTimeout { get; set; }

        public int KeepFileWaitAliveHours { get; set; }

        public int KeepErrorAndDoneAliveHours { get; set; }

        public int MaxFileWaitTimeoutMinutes { get; set; }
    }
}

