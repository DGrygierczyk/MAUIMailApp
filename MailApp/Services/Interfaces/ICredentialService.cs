using MailApp.Model;

namespace MailApp.Services.Interfaces;

public interface ICredentialService
{
    void SetCredentials(ServerCredentials credentials);
    ServerCredentials GetCredentials();
}