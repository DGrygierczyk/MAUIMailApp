using MailApp.Model;
using MailApp.Services.Interfaces;

namespace MailApp.Services;

public class CredentialService : ICredentialService
{
    private ServerCredentials _credentials;

    public void SetCredentials(ServerCredentials credentials)
    {
        _credentials = credentials;
    }

    public ServerCredentials GetCredentials()
    {
        return _credentials;
    }
}