# TransferProvider
Library to provide the IProtocolProvider to send files via sftp,ftp and email via the IProtocolInterface.

Usage:

* Adding needed protocols to the service collection in your startup.
```
services.AddTransient<IProtocolProvider, SftpProvider>();
services.AddTransient<IProtocolProvider, EmailProvider>();
services.AddTransient<ITransferService, TransService>();
```

* Adding needed Configuration for email and accessible (s)ftp servers. The IOptions pattern is highly recommended.
```
services.Configure<TransferSettings>(Configuration.GetSection(
                                TransferSettings.TransferSetting));

ServerDetails[] serverDetails = Configuration.GetSection("ConfiguredServers:ServerDetails").Get<ServerDetails[]>();
services.Configure<ConfiguredServers>(options => {
                options.Servers = serverDetails.ToList();
            });
```

* Sample transfer with switch
```
public class SomeClass : SomeClassInterface
{
    private protected IProtocolProvider ProtocolProviderSwitch(TransferProtocol transferProtocol)
    {

        switch (transferProtocol)
        {
            case (TransferProtocol.SFTP):
                _logger.LogDebug(string.Format("Used Protocol: {0}", TransferProtocol.SFTP));

                var feedback = _protocolProviderCollection.FirstOrDefault(h => h.GetType().Name == nameof(ProtocolProviderCollection.SftpProvider));
                return _protocolProviderCollection.FirstOrDefault(h => h.GetType().Name == nameof(ProtocolProviderCollection.SftpProvider));

            case (TransferProtocol.FTP):
                _logger.LogDebug(string.Format("Used Protocol: {0}", TransferProtocol.FTP));
                return _protocolProviderCollection.FirstOrDefault(h => h.GetType().Name == nameof(ProtocolProviderCollection.FtpProvider));

            case (TransferProtocol.Email):
                _logger.LogDebug(string.Format("Used Protocol: {0}", TransferProtocol.Email));
                return _protocolProviderCollection.FirstOrDefault(h => h.GetType().Name == nameof(ProtocolProviderCollection.EmailProvider));

            default:
                if (_protocolProviderCollection.FirstOrDefault() is null)
                {
                    _logger.LogCritical("There is no class injected for IProtocolProvider");
                    throw new KeyNotFoundException("There is no class injected for IProtocolProvider");
                }
                else
                {
                    var availableService = _protocolProviderCollection.FirstOrDefault().GetType();

                    _logger.LogCritical(string.Format("Could not find matching IProtocolClientProvider and used first available: {0}.", availableService));

                    return _protocolProviderCollection.FirstOrDefault();
                }
        }
    }
    private readonly ILogger<SomeClass> _logger;

    private readonly IEnumerable<IProtocolProvider> _protocolProviderCollection;

    private readonly TransferSettings _transferSettings;

    public SomeClass(IOptionsMonitor<TransferSettings> settings, IEnumerable<IProtocolProvider> protocolProviderCollection, ILogger<SomeClass> logger)
    {
        _transferSettings = settings.CurrentValue;
        _logger = logger;
        _protocolProviderCollection = protocolProviderCollection;
    }

    public async Task TransferFileAsync(TransferObject transferObject)
    {

        IProtocolProvider protocol = ProtocolProviderSwitch(transferObject.UsedProtocol);
        bool connectionOpened = protocol.OpenConnection(transferObject.TargetServer);
        if (connectionOpened)
        {
            bool transferred = protocol.TransferFile(transferObject);
            protocol.CloseClient();
        }
    }
}
```
            

            