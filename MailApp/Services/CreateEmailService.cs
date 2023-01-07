using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MimeKit;

namespace MailApp.Services;

public class CreateEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body, string username, string password, List<MimeEntity> attachments)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(username, username));
        message.To.Add(new MailboxAddress(to, to));
        message.Subject = subject;
        var multipart = new Multipart("mixed");
        multipart.Add(new TextPart("plain") { Text = body });
        if (attachments != null)
        {
            foreach (var attachment in attachments)
            {
                multipart.Add(attachment);
            }
        }
        message.Body = multipart;
        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.wp.pl", 465, true);
            await client.AuthenticateAsync(username, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        //add emial to send folder
        using (var client = new ImapClient())
        {
            await client.ConnectAsync("imap.wp.pl", 993, true);
            await client.AuthenticateAsync(username, password);
            var inbox = await client.GetFolderAsync("Sent");
            await inbox.OpenAsync(FolderAccess.ReadWrite);
            await inbox.AppendAsync(message);
            await client.DisconnectAsync(true);
        }
    }

    public async Task<List<MimeEntity>> AddAttachmentsAsync()
    {
        var results = await FilePicker.PickMultipleAsync();
        var attachments = new List<MimeEntity>();
        if (results != null)
        {
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
        }

        return attachments;
    }
}