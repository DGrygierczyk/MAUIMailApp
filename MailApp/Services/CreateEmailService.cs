using System.Collections.ObjectModel;
using MailApp.Model;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MimeKit;

namespace MailApp.Services;

public class CreateEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body,
        ObservableCollection<MimeEntity> attachments, ServerCredentials credentials)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(credentials.Username, credentials.Username));
        message.To.Add(new MailboxAddress(to, to));
        message.Subject = subject;
        var multipart = new Multipart("mixed");
        multipart.Add(new TextPart("plain") { Text = body });
        if (attachments != null)
            foreach (var attachment in attachments)
                multipart.Add(attachment);
        message.Body = multipart;
        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(credentials.SmtpServer, credentials.SmtpPort, true);
            await client.AuthenticateAsync(credentials.Username, credentials.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        using (var client = new ImapClient())
        {
            await client.ConnectAsync(credentials.ImapServer, credentials.ImapPort, true);
            await client.AuthenticateAsync(credentials.Username, credentials.Password);
            var inbox = await client.GetFolderAsync("Sent");
            await inbox.OpenAsync(FolderAccess.ReadWrite);
            await inbox.AppendAsync(message);
            await client.DisconnectAsync(true);
        }
    }

    public async Task<ObservableCollection<MimeEntity>> AddAttachmentsAsync()
    {
        var results = await FilePicker.PickMultipleAsync();
        var attachments = new ObservableCollection<MimeEntity>();
        if (results != null)
            foreach (var file in results)
            {
                var stream = await file.OpenReadAsync();
                var attachment = new MimePart("application", "octet-stream")
                {
                    Content = new MimeContent(stream),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = file.FileName
                };
                attachments.Add(attachment);
            }

        return attachments;
    }
}