namespace MailApp.Services;
using MailKit.Net.Imap;
using MailKit.Security;

public class EmailService
{
    public async Task<bool> VerifyCredentialsAsync(string username, string password)
    {
        try
        {
            using (var client = new ImapClient())
            {
                await client.ConnectAsync("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(username, password);
                client.Disconnect(true);
                return true;
            }
        }
        catch (AuthenticationException ex)
        {
            return false;
        }
        catch (ImapCommandException)
        {
            return false;
        }
        catch (ImapProtocolException)
        {
            return false;
        }
    }
}
