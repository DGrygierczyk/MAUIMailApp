namespace MailApp.Services.Interfaces;

public interface ICredentialService
{
    void SetCredentials(string username, string password);
    (string username, string password) GetCredentials();
}