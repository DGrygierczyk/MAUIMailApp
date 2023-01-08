namespace MailApp.Services.Interfaces;

public interface ICredentialService
{
    void SetCredentials(string username, string password, string smtpServer, int smtpPort, string imapServer, int imapPort);
    (string username, string password, string smtpServer, int smtpPort, string imapServer, int imapPort) GetCredentials();
}