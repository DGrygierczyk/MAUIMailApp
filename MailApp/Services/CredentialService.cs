using MailApp.Services.Interfaces;

namespace MailApp.Services;

public class CredentialService : ICredentialService
{
    private string _username;
    private string _password;
    private string _imapServer;
    private string _smtpServer;
    private int _imapPort;
    private int _smtpPort;

    public void SetCredentials(string username, string password, string smtpServer, int smtpPort, string imapServer, int imapPort)
    {
        _username = username;
        _password = password;
        _smtpServer = smtpServer;
        _smtpPort = smtpPort;
        _imapServer = imapServer;
        _imapPort = imapPort;
    }

    public (string username, string password, string smtpServer, int smtpPort, string imapServer, int imapPort) GetCredentials()
    {
        return (_username, _password, _imapServer, _imapPort, _smtpServer, _smtpPort);
    }
}