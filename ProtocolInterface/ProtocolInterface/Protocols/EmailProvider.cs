using System;
using ProtocolInterface.Models;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProtocolInterface.Configuration;

namespace ProtocolInterface.Protocols
{
    /// <summary>
    /// Email as transfer protocol. Sends files as attachment in email.
    /// </summary>
    public class EmailProvider : IProtocolProvider
    {
        private readonly ILogger<EmailProvider> _logger;
        private readonly TransferSettings _transferSettings;

        public EmailProvider(IOptionsMonitor<TransferSettings> settings, ILogger<EmailProvider> logger)
        {
            _transferSettings = settings.CurrentValue;
            _logger = logger;
        }

        public void CloseClient()
        {
            _logger.LogDebug("No client for email transfer needed.");
        }

        public bool OpenConnection(string targetMail)
        {
            _logger.LogDebug("No client for email transfer needed.");
            return true;
        }

        public bool TransferFile(TransferObject transferObject)
        {
            string mailTo = transferObject.EmailAddress;

            string mailFrom = _transferSettings.Email;

            MailMessage mail = new(mailFrom, mailTo);

            var attachment = new Attachment(transferObject.SourceFileName);

            mail.Attachments.Add(attachment);

            mail.Subject = _transferSettings.EmailSubject;

            mail.Body = string.Format("File {0} is sent to you by the ICOS TransferService. You can find the configuration for the mail and the attached document by its HashKey {1}.", transferObject.TargetFileName, transferObject.HashKey);

            var client = new SmtpClient()
            {
                Host = _transferSettings.EmailHost,
                Port = _transferSettings.EmailHostPort,
                Timeout = _transferSettings.EmailTimeout
            };

            try
            {
                client.Send(mail);

                attachment.Dispose();

                client.Dispose();

                mail.Dispose();

                return true;
            }
            catch (Exception exc)
            {
                attachment.Dispose();

                client.Dispose();

                mail.Dispose();

                string errorMessage = string.Format("Failed to send file {0} via email to {1} due to: {2}", transferObject.SourceFileName, transferObject.EmailAddress, exc.Message);

                _logger.LogError(errorMessage, exc);

                return true;
            }
        }
    }
}

