using Newtonsoft.Json;

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
                await client.ConnectAsync("imap.wp.pl", 993, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(username, password);
                client.Disconnect(true);
                return true;
            }
        }
        catch (AuthenticationException ex)
        {
            await Shell.Current.DisplayAlert("Błąd", "Niepoprawne dane logowania", "OK");
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


    public bool IsOauthSupported(string email)
    {
        string emailProvider = email.Split('@')[1];

        //
        // Dictionary<string, bool> EmailProviders = new Dictionary<string, bool>();
        // var json = File.ReadAllText("appsettings.json");
        // var data = JsonConvert.DeserializeObject<Dictionary<string, bool>>(json);
        //
        // foreach (var item in data)
        // {
        //     EmailProviders[item.Key] = item.Value;
        // }
        //
        // if (EmailProviders.ContainsKey(emailProvider) && EmailProviders[emailProvider])
        // {
        //     return true;
        // }

        return false;
    }
}