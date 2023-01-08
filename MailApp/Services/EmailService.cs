using System.Collections.ObjectModel;
using MailApp.Model;
using MailKit;
using MailKit.Search;
using Newtonsoft.Json;
using FolderAccess = MailKit.FolderAccess;
using MessageSummaryItems = MailKit.MessageSummaryItems;

namespace MailApp.Services;

using MailKit.Net.Imap;
using MailKit.Security;

public class EmailService
{
    public async Task<bool> VerifyCredentialsAsync(ServerCredentials credentials)
    {
        try
        {
            using (var client = new ImapClient())
            {
                await client.ConnectAsync(credentials.ImapServer, credentials.ImapPort, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(credentials.Username, credentials.Password);
                client.Disconnect(true);
                return true;
            }
        }
        catch (AuthenticationException ex)
        {
            await Shell.Current.DisplayAlert("Error", "Incorrect credentials", "OK");
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
        catch (System.ArgumentNullException)
        {
            await Shell.Current.DisplayAlert("Error", "Incorrect credentials", "OK");
            return false;
        }
    }

    
    
    public async Task<IList<IMailFolder>> GetFoldersAsync(ServerCredentials credentials)
    {
        using (var client = new ImapClient())
        {
            await client.ConnectAsync(credentials.ImapServer, credentials.ImapPort, SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(credentials.Username, credentials.Password);
            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadOnly);
            var folders = client.GetFolders(client.PersonalNamespaces[0]);
            await client.DisconnectAsync(true);
            return folders;
        }
    }
    
    public async Task<List<EmailEnvelope>> FetchAllEmailSummariesAsync(ServerCredentials credentials, string folder)
    {
        List<EmailEnvelope> emailEnvelopes = new();
        
        using (var client = new ImapClient())
        {
            await client.ConnectAsync(credentials.ImapServer, credentials.ImapPort, SecureSocketOptions.SslOnConnect); 
            await client.AuthenticateAsync(credentials.Username, credentials.Password);
            var inbox = await client.GetFolderAsync(folder);
            await inbox.OpenAsync(FolderAccess.ReadOnly);
            var messages = await inbox.FetchAsync(0, -1, MessageSummaryItems.Fast | MessageSummaryItems.Envelope);
            foreach (var message in messages)
            {
                var single_email =  new EmailEnvelope()
                {
                    Subject = message.NormalizedSubject,
                    From =   message.Envelope.From.First().Name,
                    Date = message.Date.DateTime,
                    IsNotRead = !(message.Flags.Value.HasFlag(MessageFlags.Seen)),
                    Id = message.Index
                };
                emailEnvelopes.Add(single_email);
            };
            return emailEnvelopes;
        }
    }
    
    public async Task<MimeKit.MimeMessage> FetchEmailAsync(ServerCredentials credentials, int id)
    {
        using (var client = new ImapClient())
        {
            await client.ConnectAsync(credentials.ImapServer, credentials.ImapPort, SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(credentials.Username, credentials.Password);
            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadWrite);
            var message = await inbox.GetMessageAsync(id);
            await inbox.AddFlagsAsync(id, MessageFlags.Seen, true);
            await client.DisconnectAsync(true);
            return message;
        }
    }

    public async Task<List<EmailEnvelope>> SearchEmailsAsync(ServerCredentials credentials, string searchQuery)
    {
        var emailEnvelopes = new List<EmailEnvelope>();

        using (var client = new ImapClient())
        {
            await client.ConnectAsync(credentials.ImapServer, credentials.ImapPort, SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(credentials.Username, credentials.Password);
            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadOnly);
            // SearchQuery searchQuery = SearchQuery.SubjectContains(searchEmailQuery);

            var uidsSubjects = await inbox.SearchAsync(SearchQuery.SubjectContains(searchQuery));
            var uidsFrom = await inbox.SearchAsync(SearchQuery.FromContains(searchQuery));
            var uidsTo = await inbox.SearchAsync(SearchQuery.ToContains(searchQuery));
            var uidsBody = await inbox.SearchAsync(SearchQuery.BodyContains(searchQuery));
            var uids = uidsSubjects.Concat(uidsFrom).Concat(uidsTo).Concat(uidsBody).Distinct().ToList();
            var messages = await inbox.FetchAsync(uids, MessageSummaryItems.Fast | MessageSummaryItems.Envelope| MessageSummaryItems.UniqueId);

            foreach (var message in messages)
            {
                var singleEmail = new EmailEnvelope
                {
                    Subject = message.NormalizedSubject,
                    From = message.Envelope.From.First().Name,
                    Date = message.Date.DateTime,
                    IsNotRead = !(message.Flags.Value.HasFlag(MessageFlags.Seen)),
                    Id = message.Index
                };
                emailEnvelopes.Add(singleEmail);
            }

            await client.DisconnectAsync(true);
            return emailEnvelopes;
        }
    }

    public async Task<bool> DeleteEmailAsync(ServerCredentials credentials, int id)
    {
        using (var client = new ImapClient())
        {
            await client.ConnectAsync(credentials.ImapServer, credentials.ImapPort, SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(credentials.Username, credentials.Password);
            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadWrite);
            await inbox.AddFlagsAsync(id, MessageFlags.Deleted, true);
            await inbox.ExpungeAsync();
            await client.DisconnectAsync(true);
            return true;
        }
    }

}