#nullable enable
using MimeKit;

namespace MailApp.Model;

public class EmailBody
{
    public MimeKit.MimeMessage Body { get; set; }
    public List<MimeEntity> Attachments { get; set; }
}