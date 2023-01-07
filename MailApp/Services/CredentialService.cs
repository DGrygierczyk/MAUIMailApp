using MailApp.Services.Interfaces;

namespace MailApp.Services;

public class CredentialService : ICredentialService
{
    private string _username;
    private string _password;

    public void SetCredentials(string username, string password)
    {
        _username = username;
        _password = password;
    }

    public (string username, string password) GetCredentials()
    {
        return (_username, _password);
    }
}